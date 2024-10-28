using TaskService.Data;
using TaskService.Controllers;
using TaskService.Services;
using TaskService.Models;

using Moq;
using MockQueryable.Moq;
using Moq.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TaskService.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TaskService.UnitTests;

public class ToDoTaskControllerTests
{
    private ToDoTaskContext DatabaseContext()
    {
        var options = new DbContextOptionsBuilder<ToDoTaskContext>()
            .UseSqlite("Data Source=testdatabase.dat")
            .Options;

        var databaseContext = new ToDoTaskContext(options);

        databaseContext.Database.EnsureDeleted();
        databaseContext.Database.EnsureCreated();

        return databaseContext;
    }

    [Fact]
    public async Task GetEmptyArrayWhenNoTasksExist()
    {
        var dbContextMock = new Mock<ToDoTaskContext>();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContextMock.Object);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var tasksSet = new List<ToDoTask>();

        dbContextMock.Setup(x => x.ToDoTasks).ReturnsDbSet(tasksSet);

        var allToDoTasks = await toDoTaskController.GetToDoTasks();

        var okResult = Assert.IsType<OkObjectResult>(allToDoTasks.Result);
        var tasks = Assert.IsAssignableFrom<List<ToDoTask>>(okResult.Value);
        Assert.Empty(tasks);
    }

    [Fact]
    public async Task GetAllTasksReturnsCorrectAmountSingle()
    {
        var dbContextMock = new Mock<ToDoTaskContext>();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContextMock.Object);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var tasksSet = new List<ToDoTask>()
        {
            new ToDoTask
            {
                Id = 1,
                Title = "Task 1",
                DueDate = DateTime.Parse("2025-10-27T15:23:59.689Z")
            }
        };

        dbContextMock.Setup(x => x.ToDoTasks).ReturnsDbSet(tasksSet);

        var allToDoTasks = await toDoTaskController.GetToDoTasks();

        var okResult = Assert.IsType<OkObjectResult>(allToDoTasks.Result);
        var tasks = Assert.IsAssignableFrom<List<ToDoTask>>(okResult.Value);
        Assert.Single(tasks);
    }
    [Fact]
    public async Task GetAllTasksReturnsCorrectAmountMultiple()
    {
        var dbContextMock = new Mock<ToDoTaskContext>();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContextMock.Object);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var tasksSet = new List<ToDoTask>()
        {
            new ToDoTask
            {
                Id = 1,
                Title = "Task 1",
                DueDate = DateTime.Parse("2025-10-27T15:23:59.689Z")
            },
            new ToDoTask
            {
                Id = 2,
                Title = "Task 2",
                DueDate = DateTime.Parse("2025-10-27T15:23:59.689Z")
            }
        };

        dbContextMock.Setup(x => x.ToDoTasks).ReturnsDbSet(tasksSet);

        var allToDoTasks = await toDoTaskController.GetToDoTasks();

        var okResult = Assert.IsType<OkObjectResult>(allToDoTasks.Result);
        var tasks = Assert.IsAssignableFrom<List<ToDoTask>>(okResult.Value);
        Assert.Equal(2, tasks.Count);
    }

    [Fact]
    public async Task GetCorrectTaskBasedOnId()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var toDoTaskResult = await toDoTaskController.GetToDoTask(1);

        Assert.IsType<ToDoTask>(toDoTaskResult.Value);
        Assert.Equal(1, toDoTaskResult.Value.Id);
    }

    [Fact]
    public async Task GetNotFoundWhenNoTaskExists()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var toDoTaskResult = await toDoTaskController.GetToDoTask(999);

        Assert.IsType<NotFoundResult>(toDoTaskResult.Result);
    }

    [Fact]
    public async Task GetNewTaskWhenValidTaskCreated()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        ToDoTask newToDoTask = new ToDoTask
        {
            Title = "A new task",
            Description = "A new description",
            DueDate = DateTime.Parse("2025-10-27T15:23:59.689Z"),
            Completed = false
        };

        var result = await toDoTaskController.PostToDoTask(newToDoTask);

        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdToDoTask = Assert.IsType<ToDoTask>(actionResult.Value);
        Assert.Equal(newToDoTask.Title, createdToDoTask.Title);
        Assert.Equal(newToDoTask.Description, createdToDoTask.Description);
        Assert.Equal(newToDoTask.DueDate, createdToDoTask.DueDate);
        Assert.Equal(newToDoTask.Completed, createdToDoTask.Completed);
    }

    [Fact]
    public async Task DueDatesInThePastNotValidated()
    {
        FutureDateAttribute futureDateAttribute = new FutureDateAttribute();

        ToDoTask newToDoTask = new ToDoTask
        {
            Title = "A new task",
            Description = "A new description",
            DueDate = DateTime.Parse("2022-10-27T15:23:59.689Z"),
            Completed = false
        };

        ValidationContext validationContext = new ValidationContext(newToDoTask.DueDate);
        ValidationResult? validationResult = futureDateAttribute.GetValidationResult(newToDoTask.DueDate, validationContext);

        Assert.Equal("Date must be in the future", validationResult?.ErrorMessage);
    }
}