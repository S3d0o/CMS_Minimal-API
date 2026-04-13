
using CMS.Application.Validators.Filters;
using CMS.Presentation.Extensions;

namespace CMS.Presentation.Endpoints
{
    public static class CourseEndpoints
    {
        public static void MapCourseEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/courses").WithTags("Course")
                .AddEndpointFilter<ValidationFilter>();

            group.MapGet("", GetCourses)
                .WithName("GetCourses")
                .WithSummary("Retrieve all courses")
                .Produces<IEnumerable<CourseReadDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPost("", CreateCourse)
                .WithName("CreateCourse")
                .WithSummary("Create a new course")
                .Produces<CourseReadDto>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapGet("/{id}", GetCourseById)
                .WithName("GetCourseById")
                .WithSummary("Retrieve a course by ID")
                .Produces<CourseReadDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPut("/{id}", UpdateCourse)
                .WithName("UpdateCourse")
                .WithSummary("Update an existing course")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapDelete("/{id}", DeleteCourse)
                .WithName("DeleteCourse")
                .WithSummary("Delete a course")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }

        internal static async Task<IResult> DeleteCourse(int id, ICourseService service)
        {
               var result = await service.DeleteCourseAsync(id);
                return ResultHandler.Handle(result);
        }

        internal static async Task<IResult> UpdateCourse(
                                                      int id,
                                                      CourseUpdateDto courseDto,
                                                      ICourseService service)
        {

            var result = await service.UpdateCourseAsync(id, courseDto);

            return ResultHandler.Handle(result);
        }

        internal static async Task<IResult> GetCourseById(ICourseService service, int id)
        {
            var result = await service.GetCourseByIdAsync(id);
            return ResultHandler.Handle(result);
        }

        internal static async Task<IResult> GetCourses(ICourseService service)
        {
            var result = await service.GetCoursesAsync();
            return ResultHandler.Handle(result);
        }
        internal static async Task<IResult> CreateCourse(ICourseService service, CourseCreateDto courseDto)
        {
            var result = await service.CreateCourseAsync(courseDto);
            if (!result.IsSuccess)
                return ResultHandler.Handle(result);
            return Results.CreatedAtRoute("GetCourseById", new { id = result.Value.Id }, result.Value);

        }

    }
}
