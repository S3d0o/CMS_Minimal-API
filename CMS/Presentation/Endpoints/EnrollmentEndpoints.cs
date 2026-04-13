using CMS.Application.DTOs.CourseDTOs;
using CMS.Application.DTOs.StudentDtos;
using CMS.Application.Interfaces;
using CMS.Application.Validators.Filters;
using CMS.Common.Enums;
using CMS.Presentation.Extensions;

namespace CMS.Presentation.Endpoints
{
    public static class EnrollmentEndpoints
    {
        public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/enrollments")
                .WithTags("Enrollment")
                .AddEndpointFilter<ValidationFilter>();

            // ENROLL STUDENT
            group.MapPost("", EnrollStudent)
                .WithName("EnrollStudent")
                .WithSummary("Enroll a student in a course")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            // COMPLETE ENROLLMENT
            group.MapPatch("/{studentId:int}/{courseId:int}/complete", CompleteEnrollment)
                .WithName("CompleteEnrollment")
                .WithSummary("Mark an enrollment as completed")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            // DROP ENROLLMENT
            group.MapPatch("/{studentId:int}/{courseId:int}/drop", DropEnrollment)
                .WithName("DropEnrollment")
                .WithSummary("Drop an active enrollment")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            // CANCEL ENROLLMENT
            group.MapPatch("/{studentId:int}/{courseId:int}/cancel", CancelEnrollment)
                .WithName("CancelEnrollment")
                .WithSummary("Cancel an active enrollment")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            // GET COURSES BY STUDENT
            group.MapGet("/students/{studentId:int}/courses", GetCoursesByStudent)
                .WithName("GetCoursesByStudent")
                .WithSummary("Retrieve courses for a specific student")
                .Produces<IEnumerable<CourseReadDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            // GET STUDENTS BY COURSE
            group.MapGet("/courses/{courseId:int}/students", GetStudentsByCourse)
                .WithName("GetStudentsByCourse")
                .WithSummary("Retrieve students enrolled in a specific course")
                .Produces<IEnumerable<StudentReadDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }

        // ===================== Handlers =====================

        private static async Task<IResult> EnrollStudent(
            IEnrollmentService enrollmentService,
            int studentId,
            int courseId)
        {
            var result = await enrollmentService.EnrollAsync(studentId, courseId);
            return ResultHandler.Handle(result);
        }

        private static async Task<IResult> CompleteEnrollment(
            IEnrollmentService enrollmentService,
            int studentId,
            int courseId)
        {
            var result = await enrollmentService.CompleteEnrollmentAsync(studentId, courseId);
            return ResultHandler.Handle(result);
        }

        private static async Task<IResult> DropEnrollment(
            IEnrollmentService enrollmentService,
            int studentId,
            int courseId)
        {
            var result = await enrollmentService.DropEnrollmentAsync(studentId, courseId);
            return ResultHandler.Handle(result);
        }

        private static async Task<IResult> CancelEnrollment(
            IEnrollmentService enrollmentService,
            int studentId,
            int courseId)
        {
            var result = await enrollmentService.CancelEnrollmentAsync(studentId, courseId);
            return ResultHandler.Handle(result);
        }

        private static async Task<IResult> GetCoursesByStudent(
            IEnrollmentService enrollmentService,
            int studentId,
            EnrollmentStatus? status)
        {
            var result = await enrollmentService.GetCoursesByStudentIdAsync(studentId, status);
            return ResultHandler.Handle(result);
        }

        private static async Task<IResult> GetStudentsByCourse(
            IEnrollmentService enrollmentService,
            int courseId)
            {
            var result = await enrollmentService.GetStudentsByCourseIdAsync(courseId);
            return ResultHandler.Handle(result);
        }
    }
}