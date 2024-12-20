using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PowerliftingAPI.Controllers;
using PowerliftingAPI.Dto;
using PowerliftingAPI.Models;

namespace PowerliftingAPITests.Controllers
{
    public class WorkoutsControllerTests
    {
        private readonly Mock<IWorkoutRepository> _workoutRepositoryMock;
        private readonly WorkoutsController _controller;

        public WorkoutsControllerTests()
        {
            _workoutRepositoryMock = new Mock<IWorkoutRepository>();
            // We don't actually need a real context here since we're mocking the repository.
            // Passing null for the context because it's not used directly by the controller (repository is used instead).
            _controller = new WorkoutsController(null, _workoutRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllWorkouts_WhenNoWorkouts_ReturnsBadRequest()
        {
            // Arrange
            _workoutRepositoryMock.Setup(repo => repo.GetAllWorkoutsAsync())
                                  .ReturnsAsync(new List<WorkoutsDTO>());  // no workouts

            // Act
            var result = await _controller.GetAllWorkouts();

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                  .Which.Value.Should().BeAssignableTo<ApiResponse>()
                  .Which.ErrorsMessages.Should().Contain("There are no workouts");
        }

        [Fact]
        public async Task GetAllWorkouts_WhenWorkoutsExist_ReturnsOkWithWorkouts()
        {
            // Arrange
            var workouts = new List<WorkoutsDTO>
            {
                new WorkoutsDTO { Id = 1, Title = "Workout 1", Notes = "Notes 1", UserId = "user1", isActive = false, Date = DateTime.Now, WorkoutExercises = new List<WorkoutExercisesDTO>() }
            };
            _workoutRepositoryMock.Setup(repo => repo.GetAllWorkoutsAsync())
                                  .ReturnsAsync(workouts);

            // Act
            var result = await _controller.GetAllWorkouts();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().BeEquivalentTo(workouts);
        }

        [Fact]
        public async Task GetWorkoutById_ZeroId_ReturnsNotFound()
        {
            // Arrange & Act
            var result = await _controller.GetWorkoutById(0);

            // Assert
            var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            var response = notFound.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.ErrorsMessages.Should().Contain("Nothing associated with Id 0");
        }

        [Fact]
        public async Task GetWorkoutById_WorkoutDoesNotExist_ReturnsBadRequest()
        {
            // Arrange
            _workoutRepositoryMock.Setup(repo => repo.GetWorkoutById(It.IsAny<int>()))
                                  .ReturnsAsync((Workouts)null);

            // Act
            var result = await _controller.GetWorkoutById(10);

            // Assert
            var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var response = badRequest.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.ErrorsMessages.Should().Contain("Nothing associated with Id 0");
        }

        [Fact]
        public async Task GetWorkoutById_WorkoutExists_ReturnsOk()
        {
            // Arrange
            var workout = new Workouts { Id = 2, Title = "Some Workout", UserId = "user1", Notes = "Some notes" };
            _workoutRepositoryMock.Setup(repo => repo.GetWorkoutById(2))
                                  .ReturnsAsync(workout);

            // Act
            var result = await _controller.GetWorkoutById(2);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().Be(workout);
        }

        [Fact]
        public async Task GetActiveWorkout_NoActiveWorkout_ReturnsOkWithNullResult()
        {
            // Arrange
            _workoutRepositoryMock.Setup(repo => repo.GetActiveWorkoutById("user1"))
                                  .ReturnsAsync((Workouts)null);

            // Act
            var result = await _controller.GetActiveWorkout("user1");

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.IsSuccess.Should().BeTrue();
            response.Result.Should().BeNull();
        }

        [Fact]
        public async Task GetActiveWorkout_ActiveWorkoutExists_ReturnsOkWithDTO()
        {
            // Arrange
            var workout = new Workouts 
            { 
                Id = 3, Title = "Active W", UserId = "user2", Notes = "notes", isActive = true,
                WorkoutExercises = new List<WorkoutExercises>()
            };
            _workoutRepositoryMock.Setup(repo => repo.GetActiveWorkoutById("user2"))
                                  .ReturnsAsync(workout);

            // Act
            var result = await _controller.GetActiveWorkout("user2");

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.IsSuccess.Should().BeTrue();
            response.Result.Should().BeAssignableTo<WorkoutsDTO>();

            var dto = response.Result as WorkoutsDTO;
            dto.Id.Should().Be(3);
            dto.Title.Should().Be("Active W");
            dto.isActive.Should().BeTrue();
        }

        [Fact]
        public async Task GetNonActiveWorkouts_NoWorkouts_ReturnsNotFound()
        {
            // Arrange
            _workoutRepositoryMock.Setup(repo => repo.GetNonActiveWorkouts("userX"))
                                  .ReturnsAsync(new List<object>());

            // Act
            var result = await _controller.GetNonActiveWorkouts("userX");

            // Assert
            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            var response = notFoundResult.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.ErrorsMessages.Should().Contain("No non-active workouts found for the user");
        }

        [Fact]
        public async Task GetNonActiveWorkouts_WorkoutsExist_ReturnsOk()
        {
            // Arrange
            var workouts = new List<object>
            {
                new { Id = 5, Title = "Old Workout" }
            };
            _workoutRepositoryMock.Setup(repo => repo.GetNonActiveWorkouts("userY"))
                                  .ReturnsAsync(workouts);

            // Act
            var result = await _controller.GetNonActiveWorkouts("userY");

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.IsSuccess.Should().BeTrue();
            response.Result.Should().BeEquivalentTo(workouts);
        }

        [Fact]
        public async Task CreateWorkout_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.CreateWorkout(new WorkoutCreateDTO());

            // Assert
            var badRequest = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var response = badRequest.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.ErrorsMessages.Should().Contain("Model is not valid");
        }

        [Fact]
        public async Task CreateWorkout_ActiveWorkoutExists_ReturnsBadRequest()
        {
            // Arrange
            var dto = new WorkoutCreateDTO { UserId = "user3", isActive = true };
            _workoutRepositoryMock.Setup(repo => repo.HasActiveWorkout("user3"))
                                  .ReturnsAsync(true);

            // Act
            var result = await _controller.CreateWorkout(dto);

            // Assert
            var badRequest = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var response = badRequest.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.ErrorsMessages.Should().Contain("A workout is already active, finish your current one before creating a new active workout");        }

        [Fact]
        public async Task CreateWorkout_Valid_ReturnsOkWithCreatedWorkout()
        {
            // Arrange
            var dto = new WorkoutCreateDTO { UserId = "user4", isActive = false, Title = "New Workout", Notes = "Some notes", Date = DateTime.Now };
            _workoutRepositoryMock.Setup(repo => repo.HasActiveWorkout("user4")).ReturnsAsync(false);
            _workoutRepositoryMock.Setup(repo => repo.CreateWorkout(dto)).ReturnsAsync(new Workouts { Id = 10 });

            // Act
            var result = await _controller.CreateWorkout(dto);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Result.Should().Be(dto);
        }

        [Fact]
        public async Task DeleteWorkoutById_WorkoutDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _workoutRepositoryMock.Setup(repo => repo.WorkoutExists(999)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteWorkoutById(999);

            // Assert
            var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            var response = notFound.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.ErrorsMessages.Should().Contain("The workout does not exist");
        }

        [Fact]
        public async Task DeleteWorkoutById_WorkoutIsNull_ReturnsBadRequest()
        {
            // Arrange
            _workoutRepositoryMock.Setup(repo => repo.WorkoutExists(1)).ReturnsAsync(true);
            _workoutRepositoryMock.Setup(repo => repo.GetWorkoutById(1)).ReturnsAsync((Workouts)null);

            // Act
            var result = await _controller.DeleteWorkoutById(1);

            // Assert
            var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            var response = badRequest.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.ErrorsMessages.Should().Contain("The record is empty (workout)");
        }

        [Fact]
        public async Task DeleteWorkoutById_Valid_ReturnsOkWithNoContentStatus()
        {
            // Arrange
            _workoutRepositoryMock.Setup(repo => repo.WorkoutExists(2)).ReturnsAsync(true);
            _workoutRepositoryMock.Setup(repo => repo.GetWorkoutById(2)).ReturnsAsync(new Workouts { Id = 2 });

            // Act
            var result = await _controller.DeleteWorkoutById(2);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task EndActiveWorkout_NoActiveWorkout_ReturnsNotFound()
        {
            // Arrange
            _workoutRepositoryMock.Setup(repo => repo.EndActiveWorkout(It.IsAny<EndActiveWorkoutDTO>()))
                                  .ReturnsAsync((Workouts)null);

            // Act
            var result = await _controller.EndActiveWorkout(new EndActiveWorkoutDTO { UserId = "user5" });

            // Assert
            var notFound = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            var response = notFound.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.ErrorsMessages.Should().Contain("No active workout found for the user");
        }

        [Fact]
        public async Task EndActiveWorkout_Valid_ReturnsOk()
        {
            // Arrange
            var endedWorkout = new Workouts { Id = 50, UserId = "user6", isActive = false, Title = "Ended Workout" };
            _workoutRepositoryMock.Setup(repo => repo.EndActiveWorkout(It.IsAny<EndActiveWorkoutDTO>()))
                                  .ReturnsAsync(endedWorkout);

            // Act
            var result = await _controller.EndActiveWorkout(new EndActiveWorkoutDTO { UserId = "user6", FinalTitle = "Ended Workout" });

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value.Should().BeAssignableTo<ApiResponse>().Subject;
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Should().Be(endedWorkout);
        }
    }
}