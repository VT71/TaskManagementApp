import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ToDoTask } from '../interfaces/to-do-task';
import { environment } from '../../environments/environment';

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
