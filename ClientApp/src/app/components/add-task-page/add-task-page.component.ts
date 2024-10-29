import { Component } from '@angular/core';
import { TaskManageFormComponent } from '../task-manage-form/task-manage-form.component';

@Component({
  selector: 'app-add-task-page',
  standalone: true,
  imports: [TaskManageFormComponent],
  templateUrl: './add-task-page.component.html',
  styleUrl: './add-task-page.component.css'
})
export class AddTaskPageComponent {

}
