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
    public void DueDatesInThePastNotValidated()
    {
        FutureDateAttribute futureDateAttribute = new FutureDateAttribute();

        ToDoTask newToDoTask = new ToDoTask
        {
            Title = "A new task",
            Description = "A new description",
            DueDate = DateTime.Parse("2022-10-27T15:23:59.689Z"),
            Completed = false
        };

        bool dueDateInTheFuture = futureDateAttribute.IsValid(newToDoTask.DueDate);

        Assert.False(dueDateInTheFuture);
    }

    [Fact]
    public async Task GetBadRequestWhenCreatingInvalidTask()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        ToDoTask newToDoTask = new ToDoTask
        {
            Title = "A new task",
            Description = "A new description",
            DueDate = DateTime.Parse("2020-10-27T15:23:59.689Z"),
            Completed = false
        };

        toDoTaskController.ModelState.AddModelError("DueDate", "Date must be in the future");

        var result = await toDoTaskController.PostToDoTask(newToDoTask);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetNoContentWhenUpdatingValidTask()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var newTitle = "Task 1 Updated";
        var newDescription = "Description updated";
        var newDueDate = DateTime.Parse("2026-10-27T15:23:59.689Z");
        var newCompletedStatus = true;

        var getResult = await toDoTaskController.GetToDoTask(1);

        ToDoTask? oldTask = getResult.Value;

        oldTask.Title = newTitle;
        oldTask.Description = newDescription;
        oldTask.DueDate = newDueDate;
        oldTask.Completed = newCompletedStatus;


        var result = await toDoTaskController.PutToDoTask(1, oldTask);

        var newGetResult = await toDoTaskController.GetToDoTask(1);
        ToDoTask? newTask = newGetResult.Value;

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(newTask.Title, newTitle);
        Assert.Equal(newTask.Description, newDescription);
        Assert.Equal(newTask.DueDate, newDueDate);
        Assert.Equal(newTask.Completed, newCompletedStatus);
    }

    [Fact]
    public async Task GetBadRequestWhenUpdatingAndIdInvalid()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var getResult = await toDoTaskController.GetToDoTask(1);

        ToDoTask? existingTask = getResult.Value;

        var result = await toDoTaskController.PutToDoTask(999, existingTask);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task GetBadRequestWhenUpdatingInvalidTask()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);
        toDoTaskController.ModelState.AddModelError("DueDate", "Date must be in the future");

        var newDueDate = DateTime.Parse("2020-10-27T15:23:59.689Z");

        var getResult = await toDoTaskController.GetToDoTask(1);

        ToDoTask? oldTask = getResult.Value;

        oldTask.DueDate = newDueDate;

        var result = await toDoTaskController.PutToDoTask(1, oldTask);

        Assert.IsType<BadRequestResult>(result);
    }
}

