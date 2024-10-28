import { Component } from '@angular/core';
import { TaskTableComponent } from '../task-table/task-table.component';

@Component({
  selector: 'app-task-list-page',
  standalone: true,
  imports: [TaskTableComponent],
  templateUrl: './task-list-page.component.html',
  styleUrl: './task-list-page.component.css'
})
export class TaskListPageComponent {

}
