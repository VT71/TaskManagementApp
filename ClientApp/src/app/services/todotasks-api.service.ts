import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable, of } from 'rxjs';
import { ToDoTask } from '../interfaces/to-do-task';
import { environment } from '../../environments/environment';
import { PagedResult } from '../interfaces/paged-result';

@Injectable({
    providedIn: 'root'
})
export class TodotasksApiService {
    private http = inject(HttpClient);

    getAllToDoTasks(): Observable<ToDoTask[]> {
        return this.http.get<ToDoTask[]>(`${environment.api.serverUrl}/ToDoTask`).pipe(
            map(tasks => tasks.map(task => ({
                ...task,
                dueDate: new Date(task.dueDate)
            })))
        );
    }

    getToDoTask(id: number): Observable<ToDoTask> {
        return this.http.get<ToDoTask>(`${environment.api.serverUrl}/ToDoTask/${id}`).pipe(
            map(task => ({
                ...task,
                dueDate: new Date(task.dueDate)
            })))
    }

    getFilteredToDoTasks(titleSearch: string | null, sortBy: string | null, sortDirection: string | null, page: string | null, pageSize: string | null): Observable<PagedResult<ToDoTask>> {
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

        return this.http.get<PagedResult<ToDoTask>>(`${environment.api.serverUrl}/ToDoTask/filter${queryString}`).pipe(
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

    getToDoTasksCount(): Observable<number> {
        return this.http.get<number>(`${environment.api.serverUrl}/ToDoTask/count`);
    }

    createToDoTask(toDoTask: ToDoTask) {
        return this.http.post<ToDoTask>(`${environment.api.serverUrl}/ToDoTask`, toDoTask);
    }

    updateToDoTask(id: number, toDoTask: ToDoTask) {
        return this.http.put<ToDoTask>(`${environment.api.serverUrl}/ToDoTask/${id}`, toDoTask);
    }

    markToDoTaskComplete(id: number, toDoTask: ToDoTask) {
        let toDoTaskToSend: ToDoTask = { ...toDoTask };
        toDoTaskToSend.completed = true;
        return this.http.put<ToDoTask>(`${environment.api.serverUrl}/ToDoTask/${id}`, toDoTaskToSend);
    }

    deleteToDoTask(id: number) {
        return this.http.delete<ToDoTask>(`${environment.api.serverUrl}/ToDoTask/${id}`);
    }
}
