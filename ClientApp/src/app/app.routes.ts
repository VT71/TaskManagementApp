import { Routes } from '@angular/router';
import { TaskListPageComponent } from './components/task-list-page/task-list-page.component';
import { AddTaskPageComponent } from './components/add-task-page/add-task-page.component';
import { EditTaskPageComponent } from './components/edit-task-page/edit-task-page.component';
import { AuthGuard } from '@auth0/auth0-angular';

// Application routes
export const routes: Routes = [
    { path: '', redirectTo: '/tasks', pathMatch: "full" },

    // Protected routes by AuthGuard for authenticated users
    { path: 'tasks', component: TaskListPageComponent, canActivate: [AuthGuard] },
    { path: 'tasks/add-task', component: AddTaskPageComponent, canActivate: [AuthGuard] },
    { path: 'tasks/edit-task/:id', component: EditTaskPageComponent, canActivate: [AuthGuard] }
];
