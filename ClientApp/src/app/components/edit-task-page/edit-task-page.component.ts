import { Component } from '@angular/core';
import { TaskManageFormComponent } from '../task-manage-form/task-manage-form.component';

@Component({
    selector: 'app-edit-task-page',
    standalone: true,
    imports: [TaskManageFormComponent],
    templateUrl: './edit-task-page.component.html',
    styleUrl: './edit-task-page.component.css'
})
export class EditTaskPageComponent {

}
