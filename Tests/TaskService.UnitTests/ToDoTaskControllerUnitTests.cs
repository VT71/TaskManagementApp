using TaskService.Data;
using TaskService.Controllers;
using TaskService.Services;
using TaskService.Models;

using Moq;
using Moq.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace TaskService.UnitTests;

public class ToDoTaskControllerTests
{
    [Fact]
    public async Task GetEmptyArrayWhenNoTasksExist()
    {
        var dbContextMock = new Mock<ToDoTaskContext>(new DbContextOptions<ToDoTaskContext>());

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
        var dbContextMock = new Mock<ToDoTaskContext>(new DbContextOptions<ToDoTaskContext>());

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
        var dbContextMock = new Mock<ToDoTaskContext>(new DbContextOptions<ToDoTaskContext>());

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
}