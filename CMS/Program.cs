
namespace CMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Serilog configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Reduce noise from framework logs
                .Enrich.FromLogContext() 
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateBootstrapLogger(); // catches logs during app startup

            try
            {
                Log.Information("Starting CMS API...");

                var builder = WebApplication.CreateBuilder(args);

                // Use Serilog with configuration support
                builder.Host.UseSerilog((context, services, config) =>
                {
                    config
                        .ReadFrom.Configuration(context.Configuration) // appsettings.json
                        .ReadFrom.Services(services)
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithEnvironmentName();
                });

                #region Services Configuration

                builder.Services.AddControllers();

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                // JSON Enum as string
                builder.Services.Configure<JsonOptions>(options =>
                {
                    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

                // DbContext
                builder.Services.AddDbContext<CMS_DBContext>(options =>
                {
                    options.UseSqlServer(
                        builder.Configuration.GetConnectionString("DefaultConnection"));
                });

                // AutoMapper
                builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);

                // Dependency Injection
                builder.Services.AddScoped<ICourseService, CourseService>();
                builder.Services.AddScoped<IStudentService, StudentService>();
                builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

                // FluentValidation
                builder.Services.AddValidatorsFromAssemblyContaining<Program>();

                #endregion

                var app = builder.Build();

                #region Middleware Pipeline

                // Serilog request logging (MUST be early)
                app.UseSerilogRequestLogging(opt =>
                {
                    opt.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                    {
                        diagnosticContext.Set("TraceId", httpContext.TraceIdentifier);
                        diagnosticContext.Set("RequestPath", httpContext.Request.Path);
                        diagnosticContext.Set("RequestMethod", httpContext.Request.Method);
                        diagnosticContext.Set("UserIP", httpContext.Connection.RemoteIpAddress?.ToString());
                    };
                });

                // Global Exception Handling
                app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();

                #endregion

                #region Endpoints

                app.MapControllers();
                app.MapCourseEndpoints();
                app.MapStudentEndpoints();
                app.MapEnrollmentEndpoints();

                #endregion

                app.Run();
            }
            catch (Exception ex)
            {
                // Critical failure
                Log.Fatal(ex, "Application failed to start");
            }
            finally
            {
                // Ensures all logs are flushed
                Log.CloseAndFlush();
            }
        }
    }
}