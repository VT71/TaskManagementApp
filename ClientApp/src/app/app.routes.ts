import { Routes } from '@angular/router';
import { TaskListPageComponent } from './components/task-list-page/task-list-page.component';
import { AddTaskPageComponent } from './components/add-task-page/add-task-page.component';

export const routes: Routes = [
    { path: '', redirectTo: '/tasks', pathMatch: "full" },
    { path: 'tasks', component: TaskListPageComponent, },
    { path: 'tasks/add-task', component: AddTaskPageComponent }
];
