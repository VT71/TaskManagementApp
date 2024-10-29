import { Component } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
    selector: 'app-task-manage-form',
    standalone: true,
    imports: [MatFormFieldModule, MatInputModule],
    templateUrl: './task-manage-form.component.html',
    styleUrl: './task-manage-form.component.css'
})
export class TaskManageFormComponent {

}
