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
    // Default page size for testing
    private int pageSize = 10;

    // Sets up a new in-memory database for testing.
    // This ensures the database is fresh for each test.
    private ToDoTaskContext DatabaseContext()
    {
        var options = new DbContextOptionsBuilder<ToDoTaskContext>()
            .UseSqlite("Data Source=testdatabase.dat")
            .Options;

        var databaseContext = new ToDoTaskContext(options);

        // Make sure the database is cleared and recreated each time.
        databaseContext.Database.EnsureDeleted();
        databaseContext.Database.EnsureCreated();

        return databaseContext;
    }

    // Deletes all ToDoTasks from the database.
    private async Task RemoveData(ToDoTaskContext toDoTaskContext)
    {
        List<ToDoTask> allTasks = await toDoTaskContext.ToDoTasks.ToListAsync();

        for (int i = 0; i < allTasks.Count; i++)
        {
            toDoTaskContext.Remove(allTasks[i]);
        }

        await toDoTaskContext.SaveChangesAsync();
    }

    // GET Tests

    [Fact]
    public async Task GetEmptyArrayWhenNoTasksExist()
    {
        // Arrange: Set up the database, service, and controller.
        var dbContext = DatabaseContext();
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Remove all ToDoTasks
        await RemoveData(dbContext);

        // Act: Call the GetToDoTasks method.
        var allToDoTasks = await toDoTaskController.GetToDoTasks();

        // Assert: Check if the response is OK and has an empty list.
        var result = Assert.IsType<OkObjectResult>(allToDoTasks.Result);
        var tasks = Assert.IsAssignableFrom<List<ToDoTask>>(result.Value);
        Assert.Empty(tasks);
    }

    [Fact]
    public async Task GetAllTasksReturnsAll()
    {
        // Arrange: Set up the database context.
        var dbContext = DatabaseContext();

        // Create a list of ToDoTasks retrieved directly from the database.
        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();

        // Initialize the service and controller
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the GetToDoTasks method
        var allToDoTasksResult = await toDoTaskController.GetToDoTasks();

        // Assert: Verify that the response is OK and contains all tasks from the db set.
        var result = Assert.IsType<OkObjectResult>(allToDoTasksResult.Result);
        var tasks = Assert.IsAssignableFrom<List<ToDoTask>>(result.Value);
        Assert.True(allToDoTasks.SequenceEqual(tasks));
    }

    [Fact]
    public async Task GetCorrectTaskBasedOnId()
    {
        // Arrange: Set up the database context and the ID to check.
        var dbContext = DatabaseContext();
        long idToCheck = 1;

        // Retrieve the task directly from the database, based on the ID.
        ToDoTask? taskFromDbSet = await dbContext.ToDoTasks.FindAsync(idToCheck);

        // Initialize the service and controller
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the GetToDoTask method with the specified ID.
        var toDoTaskResult = await toDoTaskController.GetToDoTask(idToCheck);

        // Assert: Verify that the task is of the correct type and has the expected ID.
        Assert.IsType<ToDoTask>(taskFromDbSet);
        Assert.IsType<ToDoTask>(toDoTaskResult.Value);
        Assert.Equal(taskFromDbSet.Id, toDoTaskResult.Value.Id);
    }

    [Fact]
    public async Task GetNotFoundWhenNoTaskExists()
    {
        // Arrange: Set up the database context and the service/controller.
        var dbContext = DatabaseContext();
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the GetToDoTask method with a non-existent ID.
        var toDoTaskResult = await toDoTaskController.GetToDoTask(999);

        // Assert: Verify that the result is a NotFoundResult.
        Assert.IsType<NotFoundResult>(toDoTaskResult.Result);
    }

    // Filtering Tests

    [Fact]
    public async Task GetAllTasksWhenFilteringByNull()
    {
        // Arrange: Create the database context and fetch all tasks directly.
        var dbContext = DatabaseContext();
        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();

        // Set page size to the total number of tasks, to receive all in the PagedUnit.
        pageSize = allToDoTasks.Count;

        // Initialize the service and controller with the database context.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the method to get tasks with null filtering criteria.
        var filteredToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: null, sortDirection: null, page: 1, pageSize);

        // Assert: Check that the result is OK and contains the expected PagedUnit with all tasks.
        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(allToDoTasks.Count, pagedUnit.TotalCount);
        Assert.True(pagedUnit.Items.SequenceEqual(allToDoTasks));
    }

    [Fact]
    public async Task GetExpectedTasksWhenFilteringByTitle()
    {
        // Arrange: Create the database context and define the search criteria.
        var dbContext = DatabaseContext();
        var searchCriteria = "task a";

        // Fetch the expected tasks directly based on the search criteria.
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.Where(t => t.Title.ToLower().Contains(searchCriteria)).ToListAsync();

        // Set page size to the total number of tasks, to receive all in the PagedUnit.
        pageSize = tasksFromDbSet.Count;

        // Initialize the service and controller with the database context.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the method to get tasks filtered by the specified title.
        var filteredToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: searchCriteria, sortBy: null, sortDirection: null, page: 1, pageSize);

        // Assert: Verify that the result is OK and contains the expected filtered tasks.
        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var filteredToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.True(filteredToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    [Fact]
    public async Task GetEmptyListWhenFilteringByNonExistingTitle()
    {
        // Arrange: Create the database context and initialize the service/controller.
        var dbContext = DatabaseContext();
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the method to filter tasks by a title that does not exist.
        var filteredToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: "task xyz", sortBy: null, sortDirection: null);

        // Assert: Verify that the result is OK and contains an empty list of tasks.
        var result = Assert.IsType<OkObjectResult>(filteredToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var filteredToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Empty(filteredToDoTasks);
    }

    // Sorting Tests

    [Fact]
    public async Task GetAllTasksWhenSortingByNull()
    {
        // Arrange: Create the database context and fetch all tasks directly.
        var dbContext = DatabaseContext();
        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();

        // Set page size to the total number of tasks, to receive all in the PagedUnit.
        pageSize = allToDoTasks.Count;

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the method to get all tasks without any sorting criteria.
        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: null, sortDirection: null, page: 1, pageSize);

        // Assert: Verify that the result is OK and contains the expected tasks.
        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);

        // Check that the returned list of tasks matches the original list without sorting.
        Assert.Equal(allToDoTasks.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(allToDoTasks));
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByTitleAsc()
    {
        // Arrange: Create the database context and fetch the expected sorted tasks directly.
        var dbContext = DatabaseContext();
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.OrderBy(task => task.Title).ToListAsync();

        // Set page size to the total number of tasks, to receive all in the PagedUnit.
        pageSize = tasksFromDbSet.Count;

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the method to get tasks sorted by title in ascending order.
        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: "title", sortDirection: "asc", page: 1, pageSize);

        // Assert: Verify that the result is OK and contains the expected sorted tasks.
        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(tasksFromDbSet.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByTitleDesc()
    {
        // Arrange: Create the database context and fetch the expected sorted tasks directly.
        var dbContext = DatabaseContext();
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.OrderByDescending(task => task.Title).ToListAsync();

        // Set page size to the total number of tasks, to receive all in the PagedUnit.
        pageSize = tasksFromDbSet.Count;

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the method to get tasks sorted by title in descending order.
        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: "title", sortDirection: "desc", page: 1, pageSize);

        // Assert: Verify that the result is OK and contains the expected sorted tasks.
        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(tasksFromDbSet.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByDueDateAsc()
    {
        // Arrange: Create the database context and fetch the expected sorted tasks directly.
        var dbContext = DatabaseContext();
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.OrderBy(task => task.DueDate).ToListAsync();

        // Set page size to the total number of tasks, to receive all in the PagedUnit.
        pageSize = tasksFromDbSet.Count;

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the method to get tasks sorted by due date in ascending order.
        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: "duedate", sortDirection: "asc", page: 1, pageSize);

        // Assert: Verify that the result is OK and contains the expected sorted tasks.
        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(tasksFromDbSet.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByDueDateDesc()
    {
        // Arrange: Create the database context and fetch the expected sorted tasks directly.
        var dbContext = DatabaseContext();
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.OrderByDescending(task => task.DueDate).ToListAsync();

        // Set page size to the total number of tasks, to receive all in the PagedUnit.
        pageSize = tasksFromDbSet.Count;

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the method to get tasks sorted by due date in descending order.
        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: "duedate", sortDirection: "desc", page: 1, pageSize);

        // Assert: Verify that the result is OK and contains the expected sorted tasks.
        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(tasksFromDbSet.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByCompletionStatusAsc()
    {
        // Arrange: Create the database context and fetch the expected sorted tasks directly.
        var dbContext = DatabaseContext();
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.OrderBy(task => task.Completed).ToListAsync();

        // Set page size to the total number of tasks, to receive all in the PagedUnit.
        pageSize = tasksFromDbSet.Count;

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the method to get tasks sorted by completion status in ascending order.
        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: "completed", sortDirection: "asc", page: 1, pageSize);

        // Assert: Verify that the result is OK and contains the expected sorted tasks.
        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(tasksFromDbSet.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    [Fact]
    public async Task GetExpectedTasksWhenSortingByCompletionStatusDesc()
    {
        // Arrange: Create the database context and fetch the expected sorted tasks directly.
        var dbContext = DatabaseContext();
        List<ToDoTask> tasksFromDbSet = await dbContext.ToDoTasks.OrderByDescending(task => task.Completed).ToListAsync();

        // Set page size to the total number of tasks, to receive all in the PagedUnit.
        pageSize = tasksFromDbSet.Count;

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the method to get tasks sorted by completion status in descending order.
        var sortedToDoTasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: "completed", sortDirection: "desc", page: 1, pageSize);

        // Assert: Verify that the result is OK and contains the expected sorted tasks.
        var result = Assert.IsType<OkObjectResult>(sortedToDoTasksResult.Result);
        var pagedUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        var sortedToDoTasks = Assert.IsAssignableFrom<List<ToDoTask>>(pagedUnit.Items);
        Assert.Equal(tasksFromDbSet.Count, pagedUnit.TotalCount);
        Assert.True(sortedToDoTasks.SequenceEqual(tasksFromDbSet));
    }

    // Pagination Tests

    [Fact]
    public async Task GetTasksFromFirstPageWithDefaultPageSizeWhenNoneProvided()
    {
        // Arrange: Create the database context and fetch all tasks directly.
        var dbContext = DatabaseContext();
        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the method to get tasks without page number or page size provided.
        var toDoTasksDefaultResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: null, sortDirection: null);

        // Assert: Verify that the result is OK and contains the expected tasks, based on default page size.
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
        // Arrange: Create the database context and fetch all tasks directly.
        var dbContext = DatabaseContext();
        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the method to get tasks with specific pagination parameters.
        var paginationToDoRasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: null, sortDirection: null, page: 1, pageSize);

        // Assert: Verify that the result is OK and contains the expected tasks based on the requested page size.
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
        // Arrange: Create the database context and fetch all tasks directly.
        var dbContext = DatabaseContext();
        List<ToDoTask> allToDoTasks = await dbContext.ToDoTasks.ToListAsync();

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Call the method to get tasks with a very large page number.
        var paginationToDoRasksResult = await toDoTaskController.GetToDoTasksByCriteria(titleSearch: null, sortBy: null, sortDirection: null, page: 999, pageSize);

        // Assert: Verify that the result is OK and the PagedUnit contains an empty list.
        var result = Assert.IsType<OkObjectResult>(paginationToDoRasksResult.Result);
        var paginationUnit = Assert.IsAssignableFrom<PagedUnit<ToDoTask>>(result.Value);
        Assert.IsAssignableFrom<List<ToDoTask>>(paginationUnit.Items);
        Assert.Equal(paginationUnit.TotalCount, allToDoTasks.Count);
        Assert.Empty(paginationUnit.Items);
    }

    // POST Tests

    [Fact]
    public async Task GetNewTaskWhenValidTaskCreated()
    {
        // Arrange: Create the database context.
        var dbContext = DatabaseContext();

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Create a new ToDoTask object with valid properties.
        ToDoTask newToDoTask = new ToDoTask
        {
            Title = "A new task",
            Description = "A new description",
            DueDate = DateTime.Parse("2025-10-27T15:23:59.689Z"),
            Completed = false
        };

        // Act: Call the controller method to create a new task.
        var result = await toDoTaskController.PostToDoTask(newToDoTask);

        // Check that the created task has the same properties as the new task.
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdToDoTask = Assert.IsType<ToDoTask>(actionResult.Value);
        Assert.Equal(newToDoTask.Title, createdToDoTask.Title);
        Assert.Equal(newToDoTask.Description, createdToDoTask.Description);
        Assert.Equal(newToDoTask.DueDate, createdToDoTask.DueDate);
        Assert.Equal(newToDoTask.Completed, createdToDoTask.Completed);
    }

    [Fact]
    public async Task GetBadRequestWhenCreatingInvalidTask()
    {
        // Arrange: Create an instance of the database context.
        var dbContext = DatabaseContext();

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Create a new ToDoTask with an invalid due date (in the past).
        ToDoTask newToDoTask = new ToDoTask
        {
            Title = "A new task",
            Description = "A new description",
            DueDate = DateTime.Parse("2020-10-27T15:23:59.689Z"),
            Completed = false
        };

        // Simulate a model state error for the DueDate field.
        toDoTaskController.ModelState.AddModelError("DueDate", "Date must be in the future");

        // Act: Attempt to create the task with invalid data.
        var result = await toDoTaskController.PostToDoTask(newToDoTask);

        // Assert: Verify that the result is a BadRequest response.
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    // Due Date Validator Tests

    [Fact]
    public void DueDatesInThePastNotValidated()
    {
        // Arrange: Create an instance of the FutureDateAttribute validator.
        FutureDateAttribute futureDateAttribute = new FutureDateAttribute();

        // Create a new ToDoTask with a due date set in the past.
        ToDoTask newToDoTask = new ToDoTask
        {
            Title = "A new task",
            Description = "A new description",
            DueDate = DateTime.Parse("2022-10-27T15:23:59.689Z"),
            Completed = false
        };

        // Act: Validate the due date using the FutureDateAttribute.
        bool dueDateInTheFuture = futureDateAttribute.IsValid(newToDoTask.DueDate);

        // Assert
        Assert.False(dueDateInTheFuture);
    }

    // PUT Tests

    [Fact]
    public async Task GetNoContentWhenUpdatingValidTask()
    {
        // Arrange: Create an instance of the database context.
        var dbContext = DatabaseContext();

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Set the new values for the task's properties.
        var newTitle = "Task 1 Updated";
        var newDescription = "Description updated";
        var newDueDate = DateTime.Parse("2026-10-27T15:23:59.689Z");
        var newCompletedStatus = true;

        // Act: Retrieve the existing task with ID 1.
        var getResult = await toDoTaskController.GetToDoTask(1);
        ToDoTask? oldTask = getResult.Value;

        if (oldTask != null)
        {

            oldTask.Title = newTitle;
            oldTask.Description = newDescription;
            oldTask.DueDate = newDueDate;
            oldTask.Completed = newCompletedStatus;

            // Act: Call the method to update the task.
            var result = await toDoTaskController.PutToDoTask(1, oldTask);

            // Retrieve the updated task to verify changes.
            var newGetResult = await toDoTaskController.GetToDoTask(1);
            ToDoTask? newTask = newGetResult.Value;

            if (newTask != null)
            {
                // Assert: Check that the result indicates no content was returned (successful update).
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
        // Arrange: Create an instance of the database context.
        var dbContext = DatabaseContext();

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Attempt to retrieve an existing task with ID 1.
        var getResult = await toDoTaskController.GetToDoTask(1);
        ToDoTask? existingTask = getResult.Value;

        if (existingTask != null)
        {
            // Act: Try to update the task with an invalid ID (999).
            var result = await toDoTaskController.PutToDoTask(999, existingTask);

            // Assert: Verify that the result is a BadRequest.
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
        // Arrange: Create an instance of the database context.
        var dbContext = DatabaseContext();

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Simulate model state error for an invalid DueDate (in the past).
        toDoTaskController.ModelState.AddModelError("DueDate", "Date must be in the future");

        var newDueDate = DateTime.Parse("2020-10-27T15:23:59.689Z");

        // Act: Attempt to retrieve an existing task with ID 1.
        var getResult = await toDoTaskController.GetToDoTask(1);
        ToDoTask? oldTask = getResult.Value;

        if (oldTask != null)
        {
            oldTask.DueDate = newDueDate;

            // Act: Try to update the task with the invalid due date.
            var result = await toDoTaskController.PutToDoTask(1, oldTask);

            // Assert: Verify that the result is a BadRequest.
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
        // Arrange: Create an instance of the database context.
        var dbContext = DatabaseContext();

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Create a task object representing a non-existent task.
        ToDoTask nonExistentTask = new ToDoTask
        {
            Id = 999,
            Title = "Task",
            Description = "A new description",
            DueDate = DateTime.Parse("2025-10-27T15:23:59.689Z"),
            Completed = false
        };
        // Act: Attempt to update a non-existent task.
        var result = await toDoTaskController.PutToDoTask(999, nonExistentTask);

        // Assert: Verify that the result is a NotFound response.
        Assert.IsType<NotFoundResult>(result);
    }

    // DELETE Tests

    [Fact]
    public async Task GetNoContentWhenDeletingTaskWithValidId()
    {
        // Arrange: Create an instance of the database context.
        var dbContext = DatabaseContext();

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Attempt to delete a task with a valid ID.
        var deleteResult = await toDoTaskController.DeleteToDoTask(1);

        // Try to get the task again to check if it has been deleted.
        var getResult = await toDoTaskController.GetToDoTask(1);

        // Assert: Verify that the result of the delete operation is NoContent.
        Assert.IsType<NoContentResult>(deleteResult);

        // Assert: Verify that trying to get the deleted task returns NotFound.
        Assert.IsType<NotFoundResult>(getResult.Result);
    }

    [Fact]
    public async Task GetNotFoundWhenDeletingTaskWithInvalidId()
    {
        // Arrange: Create an instance of the database context.
        var dbContext = DatabaseContext();

        // Initialize the service and controller.
        ToDoTaskService toDoTaskService = new ToDoTaskService(dbContext);
        ToDoTaskController toDoTaskController = new ToDoTaskController(toDoTaskService);

        // Act: Attempt to delete a task with an invalid ID (999).
        var deleteResult = await toDoTaskController.DeleteToDoTask(999);

        // Assert: Verify that the result of the delete operation returns NotFound.
        Assert.IsType<NotFoundResult>(deleteResult);
    }
}

