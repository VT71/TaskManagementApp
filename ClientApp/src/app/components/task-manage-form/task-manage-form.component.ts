import { Component, inject } from '@angular/core';
import { FormBuilder, FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-task-manage-form',
    standalone: true,
    providers: [
        { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
        provideNativeDateAdapter(),
    ],
    imports: [MatFormFieldModule, MatInputModule, MatDatepickerModule, MatSelectModule, MatButtonModule, ReactiveFormsModule],
    templateUrl: './task-manage-form.component.html',
    styleUrl: './task-manage-form.component.css'
})
export class TaskManageFormComponent {
    private formBuilder = inject(FormBuilder);

    public formGroup = this.formBuilder.group({
        title: ['', [Validators.required, Validators.maxLength(100)]],
        description: [''],
        dueDate: [new Date(), [Validators.required]]
    })

}
