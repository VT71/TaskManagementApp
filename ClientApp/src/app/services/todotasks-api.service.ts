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

    getAllTasks(): Observable<ToDoTask[]> {
        return this.http.get<ToDoTask[]>(`${environment.api.serverUrl}/ToDoTask`).pipe(
            map(tasks => tasks.map(task => ({
                ...task,
                dueDate: new Date(task.dueDate)
            })))
        );
    }

    createTask(toDoTask: ToDoTask) {
        return this.http.post<ToDoTask>(`${environment.api.serverUrl}/ToDoTask`, toDoTask);
    }
}
