using TaskService.Data;
using TaskService.Controllers;
using TaskService.Services;
using TaskService.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TaskService.Attributes;

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
    public async Task GetAllTasksReturnsCorrectAmount()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var allToDoTasks = await toDoTaskController.GetToDoTasks();

        var result = Assert.IsType<OkObjectResult>(allToDoTasks.Result);
        var tasks = Assert.IsAssignableFrom<List<ToDoTask>>(result.Value);
        Assert.Equal(26, tasks.Count);
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
    public async Task GetAllTasksWhenFilteringByNull()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: null, sortBy: null, sortDirection: null);

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(26, pagedUnit.TotalCount);
        Assert.Equal(pageSize, pagedUnit.Items.Count);
    }

    [Fact]
    public async Task GetExpectedTaskWhenFilteringByTitle()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: "task a", sortBy: null, sortDirection: null);

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var filteredToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Single(filteredToDoTasks);
        Assert.IsType<ToDoTask>(filteredToDoTasks[0]);
        Assert.Equal("Task A", filteredToDoTasks[0].Title);
    }

    [Fact]
    public async Task GetEmptyListWhenFilteringByNonExistingTitle()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: "task xyz", sortBy: null, sortDirection: null);

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var filteredToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Empty(filteredToDoTasks);
    }

    [Fact]
    public async Task GetMultipleTasksWhenUsingBroadFiltering()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: "task", sortBy: null, sortDirection: null);

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(26, pagedUnit.TotalCount);
    }

    [Fact]
    public async Task GetAllTasksWhenSortingByNull()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: null, sortBy: null, sortDirection: null);

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        Assert.Equal(26, pagedUnit.TotalCount);
        Assert.Equal(pageSize, pagedUnit.Items.Count);
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByTitleAsc()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: null, sortBy: "title", sortDirection: "asc");

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var filteredToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(26, pagedUnit.TotalCount);
        Assert.Equal("Task A", filteredToDoTasks[0].Title);
        Assert.Equal("Task B", filteredToDoTasks[1].Title);
        Assert.Equal("Task C", filteredToDoTasks[2].Title);
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByTitleDesc()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: null, sortBy: "title", sortDirection: "desc");

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var filteredToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(26, pagedUnit.TotalCount);
        Assert.Equal("Task Z", filteredToDoTasks[0].Title);
        Assert.Equal("Task Y", filteredToDoTasks[1].Title);
        Assert.Equal("Task X", filteredToDoTasks[2].Title);
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByDueDateAsc()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: null, sortBy: "duedate", sortDirection: "asc");

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var filteredToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(26, pagedUnit.TotalCount);
        Assert.Equal(DateTime.Parse("2025-01-01T00:00:00.000Z"), filteredToDoTasks[0].DueDate);
        Assert.Equal(DateTime.Parse("2025-01-02T00:00:00.000Z"), filteredToDoTasks[1].DueDate);
        Assert.Equal(DateTime.Parse("2025-01-03T00:00:00.000Z"), filteredToDoTasks[2].DueDate);
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByDueDateDesc()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: null, sortBy: "duedate", sortDirection: "desc");

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var filteredToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(26, pagedUnit.TotalCount);
        Assert.Equal(DateTime.Parse("2025-01-26T00:00:00.000Z"), filteredToDoTasks[0].DueDate);
        Assert.Equal(DateTime.Parse("2025-01-25T00:00:00.000Z"), filteredToDoTasks[1].DueDate);
        Assert.Equal(DateTime.Parse("2025-01-24T00:00:00.000Z"), filteredToDoTasks[2].DueDate);
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByCompletionStatusAsc()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: null, sortBy: "completed", sortDirection: "asc");

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var filteredToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(26, pagedUnit.TotalCount);
        Assert.False(filteredToDoTasks[0].Completed);
        Assert.False(filteredToDoTasks[1].Completed);
        Assert.False(filteredToDoTasks[2].Completed);
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByCompletionStatusDesc()
    {
        var dbContext = DatabaseContext();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var filteredToDoTasksResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: null, sortBy: "completed", sortDirection: "desc");

        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var filteredToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(26, pagedUnit.TotalCount);
        Assert.True(filteredToDoTasks[0].Completed);
        Assert.True(filteredToDoTasks[1].Completed);
        Assert.True(filteredToDoTasks[2].Completed);
    }

    [Fact]
    public async Task GetTasksFromFirstPageWithDefaultPageSizeWhenNoneProvided()
    {
        var dbContext = DatabaseContext();

        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var toDoTasksDefaultResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: null, sortBy: null, sortDirection: null);
        var toDoTasksFirstPageResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: null, sortBy: null, sortDirection: null, page: 1, pageSize);

        var defaultResult = Assert.IsType<OkObjectResult>(toDoTasksDefaultResult.Result);
        var firstPageResult = Assert.IsType<OkObjectResult>(toDoTasksFirstPageResult.Result);

        var defaultPagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(defaultResult.Value);
        var firstPagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(firstPageResult.Value);

        var defaultToDoTasks = defaultPagedUnit.Items;
        var firstPageToDoTasks = firstPagedUnit.Items;

        Assert.IsAssignableFrom<List<ToDoTask>>(defaultToDoTasks);
        Assert.IsAssignableFrom<List<ToDoTask>>(firstPageToDoTasks);

        Assert.Equal(defaultPagedUnit.TotalCount, allToDoTasks.Count);
        Assert.Equal(defaultToDoTasks, firstPageToDoTasks);
    }

    [Fact]
    public async Task GetExpectedTasksWhenValidPageAndPageSizeProvided()
    {
        var dbContext = DatabaseContext();

        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var paginationToDoRasksResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: null, sortBy: null, sortDirection: null, page: 1, pageSize);

        var result = Assert.IsType<OkObjectResult>(paginationToDoRasksResult.Result);
        var paginationUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var paginationToDoTasks = paginationUnit.Items;

        Assert.IsAssignableFrom<List<ToDoTask>>(paginationToDoTasks);
        Assert.Equal(paginationUnit.TotalCount, allToDoTasks.Count);
        Assert.Equal(paginationToDoTasks, allToDoTasks.Take(pageSize).ToList());
    }

    [Fact]
    public async Task GetEmptyListWhenALargePageNumberProvided()
    {
        var dbContext = DatabaseContext();

        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();

        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        var paginationToDoRasksResult = await toDoTaskController.GetFilteredToDoTasks(titleSearch: null, sortBy: null, sortDirection: null, page: 999, pageSize);

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

