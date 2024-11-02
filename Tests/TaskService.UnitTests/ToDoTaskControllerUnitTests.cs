using TaskService.Data;
using TaskService.Controllers;
using TaskService.Services;
using TaskService.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TaskService.Attributes;
using NuGet.ContentModel;

namespace TaskService.UnitTests;

public class ToDoTaskControllerTests
{
    private int pageSize = 10;
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

    private async Task RemoveData(ToDoTaskContext toDoTaskContext)
    {
        List<ToDoTask> allTasks = await toDoTaskContext.ToDoTasks.ToListAsync();

        for (int i = 0; i < allTasks.Count; i++)
        {
            toDoTaskContext.Remove(allTasks[i]);
        }

        await toDoTaskContext.SaveChangesAsync();
    }

    [Fact]
    public async Task GetEmptyArrayWhenNoTasksExist()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);
        await RemoveData(dbContext);

        var allToDoTasks = await toDoTaskController.GetToDoTasks();

        var result = Assert.IsType<OkObjectResult>(allToDoTasks.Result);
        var tasks = Assert.IsAssignableFrom<List<ToDoTask>>(result.Value);
        Assert.Empty(tasks);
    }

    [Fact]
    public async Task GetAllTasksReturnsAll()
    {
        var dbContext = DatabaseContext();

        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var allToDoTasksResult = await toDoTaskController.GetToDoTasks();

        var result = Assert.IsType<OkObjectResult>(allToDoTasksResult.Result);
        var tasks = Assert.IsAssignableFrom<List<ToDoTask>>(result.Value);
        Assert.True(allToDoTasks.SequenceEqual(tasks));
    }

    [Fact]
    public async Task GetCorrectTaskBasedOnId()
    {
        var dbContext = DatabaseContext();

        long idToCheck = 1;
        ToDoTask? taskFromDbSet = await dbContext.ToDoTasks.FindAsync(idToCheck);
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var toDoTaskResult = await toDoTaskController.GetToDoTask(idToCheck);

        Assert.IsType<ToDoTask>(taskFromDbSet);
        Assert.IsType<ToDoTask>(toDoTaskResult.Value);
        Assert.Equal(taskFromDbSet.Id, toDoTaskResult.Value.Id);
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
    public async Task GetAllTasksWhenFilteringByNull()
    {
        var dbContext = DatabaseContext();

        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();
        pageSize = allToDoTasks.Count;
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: null, sortDirection: null, page: 1, pageSize);

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(allToDoTasks.Count, pagedUnit.TotalCount);
        Assert.True(pagedUnit.Items.SequenceEqual(allToDoTasks));
    }

    [Fact]
    public async Task GetExpectedTasksWhenFilteringByTitle()
    {
        var dbContext = DatabaseContext();
        var searchCriteria = "task a";
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.Where(t => t.Title.ToLower().Contains(searchCriteria)).ToListAsync();
        pageSize = tasksFromDbSet.Count;
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: searchCriteria, sortBy: null, sortDirection: null, page: 1, pageSize);

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var filteredToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.True(filteredToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    [Fact]
    public async Task GetEmptyListWhenFilteringByNonExistingTitle()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: "task xyz", sortBy: null, sortDirection: null);

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var filteredToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Empty(filteredToDoTasks);
    }

    [Fact]
    public async Task GetAllTasksWhenSortingByNull()
    {
        var dbContext = DatabaseContext();

        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();
        pageSize = allToDoTasks.Count;
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: null, sortDirection: null, page: 1, pageSize);

        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(allToDoTasks.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(allToDoTasks));
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByTitleAsc()
    {
        var dbContext = DatabaseContext();
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.OrderBy(task => task.Title).ToListAsync();
        pageSize = tasksFromDbSet.Count;
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: "title", sortDirection: "asc", page: 1, pageSize);

        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(tasksFromDbSet.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByTitleDesc()
    {
        var dbContext = DatabaseContext();
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.OrderByDescending(task => task.Title).ToListAsync();
        pageSize = tasksFromDbSet.Count;
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: "title", sortDirection: "desc", page: 1, pageSize);

        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(tasksFromDbSet.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByDueDateAsc()
    {
        var dbContext = DatabaseContext();
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.OrderBy(task => task.DueDate).ToListAsync();
        pageSize = tasksFromDbSet.Count;
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: "duedate", sortDirection: "asc", page: 1, pageSize);

        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(tasksFromDbSet.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByDueDateDesc()
    {
        var dbContext = DatabaseContext();
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.OrderByDescending(task => task.DueDate).ToListAsync();
        pageSize = tasksFromDbSet.Count;
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: "duedate", sortDirection: "desc", page: 1, pageSize);

        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(tasksFromDbSet.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByCompletionStatusAsc()
    {
        var dbContext = DatabaseContext();
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.OrderBy(task => task.Completed).ToListAsync();
        pageSize = tasksFromDbSet.Count;
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: "completed", sortDirection: "asc", page: 1, pageSize);

        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(tasksFromDbSet.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByCompletionStatusDesc()
    {
        var dbContext = DatabaseContext();
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.OrderByDescending(task => task.Completed).ToListAsync();
        pageSize = tasksFromDbSet.Count;
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: "completed", sortDirection: "desc", page: 1, pageSize);

        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(tasksFromDbSet.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    [Fact]
    public async Task GetTasksFromFirstPageWithDefaultPageSizeWhenNoneProvided()
    {
        var dbContext = DatabaseContext();

        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var toDoTasksDefaultResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: null, sortDirection: null);
        var defaultResult = Assert.IsType<OkObjectResult>(toDoTasksDefaultResult.Result);
        var defaultPagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(defaultResult.Value);
        var defaultToDoTasks = defaultPagedUnit.Items;

        Assert.IsAssignableFrom<List<ToDoTask>>(defaultToDoTasks);
        Assert.Equal(defaultPagedUnit.TotalCount, allToDoTasks.Count);
        Assert.True(defaultToDoTasks.SequenceEqual(allToDoTasks.Take(pageSize)));
    }

    [Fact]
    public async Task GetExpectedTasksWhenValidPageAndPageSizeProvided()
    {
        var dbContext = DatabaseContext();

        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var paginationToDoRasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: null, sortDirection: null, page: 1, pageSize);

        var result = Assert.IsType<OkObjectResult>(paginationToDoRasksResult.Result);
        var paginationUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var paginationToDoTasks = paginationUnit.Items;

        Assert.IsAssignableFrom<List<ToDoTask>>(paginationToDoTasks);
        Assert.Equal(paginationUnit.TotalCount, allToDoTasks.Count);
        Assert.True(paginationToDoTasks.SequenceEqual(allToDoTasks.Take(pageSize)));
    }

    [Fact]
    public async Task GetEmptyListWhenALargePageNumberProvided()
    {
        var dbContext = DatabaseContext();

        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var paginationToDoRasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: null, sortDirection: null, page: 999, pageSize);

        var result = Assert.IsType<OkObjectResult>(paginationToDoRasksResult.Result);
        var paginationUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        Assert.IsAssignableFrom<List<ToDoTask>>(paginationUnit.Items);
        Assert.Equal(paginationUnit.TotalCount, allToDoTasks.Count);
        Assert.Empty(paginationUnit.Items);
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

        if (oldTask != null)
        {

            oldTask.Title = newTitle;
            oldTask.Description = newDescription;
            oldTask.DueDate = newDueDate;
            oldTask.Completed = newCompletedStatus;


            var result = await toDoTaskController.PutToDoTask(1, oldTask);

            var newGetResult = await toDoTaskController.GetToDoTask(1);
            ToDoTask? newTask = newGetResult.Value;

            if (newTask != null)
            {
                Assert.IsType<NoContentResult>(result);
                Assert.Equal(newTask.Title, newTitle);
                Assert.Equal(newTask.Description, newDescription);
                Assert.Equal(newTask.DueDate, newDueDate);
                Assert.Equal(newTask.Completed, newCompletedStatus);
            }
            else
            {
                Assert.Fail();
            }
        }
        else
        {
            Assert.Fail();
        }
    }

    [Fact]
    public async Task GetBadRequestWhenUpdatingAndIdInvalid()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var getResult = await toDoTaskController.GetToDoTask(1);

        ToDoTask? existingTask = getResult.Value;

        if (existingTask != null)
        {
            var result = await toDoTaskController.PutToDoTask(999, existingTask);
            Assert.IsType<BadRequestResult>(result);
        }
        else
        {
            Assert.Fail();
        }
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

        if (oldTask != null)
        {
            oldTask.DueDate = newDueDate;

            var result = await toDoTaskController.PutToDoTask(1, oldTask);

            Assert.IsType<BadRequestResult>(result);
        }
        else
        {
            Assert.Fail();
        }
    }

    [Fact]
    public async Task GetNotFoundWhenUpdatingNonExistentTask()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        ToDoTask nonExistentTask = new ToDoTask
        {
            Id = 999,
            Title = "Task",
            Description = "A new description",
            DueDate = DateTime.Parse("2025-10-27T15:23:59.689Z"),
            Completed = false
        };

        var result = await toDoTaskController.PutToDoTask(999, nonExistentTask);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetNoContentWhenDeletingTaskWithValidId()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var deleteResult = await toDoTaskController.DeleteToDoTask(1);
        var getResult = await toDoTaskController.GetToDoTask(1);

        Assert.IsType<NoContentResult>(deleteResult);
        Assert.IsType<NotFoundResult>(getResult.Result);
    }

    [Fact]
    public async Task GetNotFoundWhenDeletingTaskWithInvalidId()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var deleteResult = await toDoTaskController.DeleteToDoTask(999);

        Assert.IsType<NotFoundResult>(deleteResult);
    }
}

