import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable, of } from 'rxjs';
import { ToDoTask } from '../interfaces/to-do-task';
import { environment } from '../../environments/environment';
import { PagedUnit } from '../interfaces/paged-unit';

@Injectable({
    providedIn: 'root'
})
export class TodotasksApiService {
    private http = inject(HttpClient);

    /**
     * Fetches all ToDoTasks from the server.
     * Converts each task's dueDate property from a string to a Date object.
     */
    getAllToDoTasks(): Observable<ToDoTask[]> {
        return this.http.get<ToDoTask[]>(`${environment.api.serverUrl}/ToDoTask`).pipe(
            map(tasks => tasks.map(task => ({
                ...task,
                dueDate: new Date(task.dueDate)
            })))
        );
    }

    /**
    * Fetches a single ToDoTask by ID.
    * Converts the task's dueDate property from a string to a Date object.
    */
    getToDoTask(id: number): Observable<ToDoTask> {
        return this.http.get<ToDoTask>(`${environment.api.serverUrl}/ToDoTask/${id}`).pipe(
            map(task => ({
                ...task,
                dueDate: new Date(task.dueDate)
            })))
    }

    /**
     * Retrieves ToDoTasks based on various filtering, sorting, and pagination criteria.
     * Converts each task's dueDate from a string to a Date object.
     */
    getToDoTasksByCriteria(titleSearch: string | null, sortBy: string | null, sortDirection: string | null, page: string | null, pageSize: string | null): Observable<PagedUnit<ToDoTask>> {
        let queryParams = [];

        if (titleSearch) {
            queryParams.push(`titleSearch=${titleSearch}`);
        }

        if (sortBy && sortDirection) {
            queryParams.push(`sortBy=${sortBy}`, `sortDirection=${sortDirection}`);
        }

        if (page && pageSize) {
            queryParams.push(`page=${page}`, `pageSize=${pageSize}`);
        }

        const queryString = queryParams.length ? `?${queryParams.join('&')}` : '';

        return this.http.get<PagedUnit<ToDoTask>>(`${environment.api.serverUrl}/ToDoTask/criteria${queryString}`).pipe(
            map(result => {
                return {
                    ...result, items: result.items.map(task => ({
                        ...task,
                        dueDate: new Date(task.dueDate)
                    }))
                }
            })
        );

    }

    /**
     * Creates a new ToDoTask on the server.
     */
    createToDoTask(toDoTask: ToDoTask) {
        return this.http.post<ToDoTask>(`${environment.api.serverUrl}/ToDoTask`, toDoTask);
    }

    /**
     * Updates an existing ToDoTask by ID.
     */
    updateToDoTask(id: number, toDoTask: ToDoTask) {
        return this.http.put<ToDoTask>(`${environment.api.serverUrl}/ToDoTask/${id}`, toDoTask);
    }

    /**
     * Marks a specific ToDoTask as completed by updating its completed property to true.
     */
    markToDoTaskComplete(id: number, toDoTask: ToDoTask) {
        let toDoTaskToSend: ToDoTask = { ...toDoTask };
        toDoTaskToSend.completed = true;
        return this.http.put<ToDoTask>(`${environment.api.serverUrl}/ToDoTask/${id}`, toDoTaskToSend);
    }

    /**
     * Deletes a ToDoTask by ID.
     */
    deleteToDoTask(id: number) {
        return this.http.delete<ToDoTask>(`${environment.api.serverUrl}/ToDoTask/${id}`);
    }
}
