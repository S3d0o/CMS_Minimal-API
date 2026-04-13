using CMS.Application.DTOs.StudentDtos;
using CMS.Application.Interfaces;
using CMS.Application.Validators.Filters;
using CMS.Presentation.Extensions;

namespace CMS.Presentation.Endpoints
{
    public static class StudentEndpoints
    {
        public static void MapStudentEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/students")
                .WithTags("Student")
                .AddEndpointFilter<ValidationFilter>();

            // GET ALL
            group.MapGet("", GetStudents)
                .WithName("GetStudents")
                .WithSummary("Retrieve all students")
                .Produces<IEnumerable<StudentReadDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            // GET BY ID
            group.MapGet("/{id:int}", GetStudentById)
                .WithName("GetStudentById")
                .WithSummary("Retrieve a student by ID")
                .Produces<StudentReadDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            // CREATE
            group.MapPost("", CreateStudent)
                .WithName("CreateStudent")
                .WithSummary("Create a new student")
                .Produces<StudentReadDto>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            // UPDATE
            group.MapPut("/{id:int}", UpdateStudent)
                .WithName("UpdateStudent")
                .WithSummary("Update an existing student")
                .Produces<StudentReadDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            // DELETE
            group.MapDelete("/{id:int}", DeleteStudent)
                .WithName("DeleteStudent")
                .WithSummary("Delete a student")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }

        // ===================== Handlers =====================

        private static async Task<IResult> GetStudents(IStudentService studentService)
        {
            var result = await studentService.GetAllAsync();
            return ResultHandler.Handle(result);
        }

        private static async Task<IResult> GetStudentById(IStudentService studentService, int id)
        {
            var result = await studentService.GetByIdAsync(id);
            return ResultHandler.Handle(result);
        }

        private static async Task<IResult> CreateStudent(
            IStudentService studentService,
            StudentCreateOrUpdateDto studentCreateDto)
        {
            var result = await studentService.CreateAsync(studentCreateDto);

            if (!result.IsSuccess)
                return ResultHandler.Handle(result);

            return Results.CreatedAtRoute(
                "GetStudentById",
                new { id = result.Value.Id },
                result.Value);
        }

        private static async Task<IResult> UpdateStudent(
            IStudentService studentService,
            int id,
            StudentCreateOrUpdateDto studentUpdateDto)
        {
            var result = await studentService.UpdateAsync(id, studentUpdateDto);
            return ResultHandler.Handle(result);
        }

        private static async Task<IResult> DeleteStudent(IStudentService studentService, int id)
        {
            var result = await studentService.DeleteAsync(id);
            return ResultHandler.Handle(result);
        }
    }
}