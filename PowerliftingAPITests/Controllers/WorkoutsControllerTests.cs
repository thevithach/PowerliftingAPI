using Xunit;
using Moq;
using PowerliftingAPI.Controllers;
using PowerliftingAPI.Repositories;
using PowerliftingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using PowerliftingAPI.Dto;

namespace PowerliftingAPITests.Controllers;

public class WorkoutsControllerTests
{
    private readonly Mock<IWorkoutRepository> _mockRepo;
    private readonly WorkoutsController _controller;

    public WorkoutsControllerTests()
    {
        _mockRepo = new Mock<IWorkoutRepository>();
        _controller = new WorkoutsController(null, _mockRepo.Object);
    }

    [Fact]
    public async Task GetAllWorkouts_ReturnsOkResult_WhenWorkoutsExist()
    {
        // Arrange
        var workouts = new List<WorkoutsDTO> 
        { 
            new WorkoutsDTO { Id = 1, Title = "Workout 1" }, 
            new WorkoutsDTO { Id = 2, Title = "Workout 2" } 
        };
        _mockRepo.Setup(repo => repo.GetAllWorkouts()).ReturnsAsync(workouts);

        // Act
        var result = await _controller.GetAllWorkouts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ApiResponse>(okResult.Value);
        var workoutsInResponse = Assert.IsAssignableFrom<IEnumerable<WorkoutsDTO>>(returnValue.Result);
        Assert.Equal(workouts.Count, ((ICollection<WorkoutsDTO>)workoutsInResponse).Count);
    }
}