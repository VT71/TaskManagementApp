import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormsModule, NonNullableFormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { TodotasksApiService } from '../../services/todotasks-api.service';
import { Subscription } from 'rxjs';
import { ToDoTask } from '../../interfaces/to-do-task';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-task-manage-form',
    standalone: true,
    providers: [
        { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
        provideNativeDateAdapter(),
    ],
    imports: [MatFormFieldModule, MatInputModule, MatDatepickerModule, MatSelectModule, MatButtonModule, ReactiveFormsModule, RouterLink],
    templateUrl: './task-manage-form.component.html',
    styleUrl: './task-manage-form.component.css'
})
export class TaskManageFormComponent implements OnDestroy {
    private toDoTasksApiService = inject(TodotasksApiService)
    private formBuilder = inject(NonNullableFormBuilder);
    private subscriptions: Subscription[] = [];

    public taskManageForm = this.formBuilder.group({
        id: [0],
        title: ['', [Validators.required, Validators.maxLength(100)]],
        description: [''],
        dueDate: [new Date(), [Validators.required]],
        completed: [false, [Validators.required]]
    },
        { validators: this.futureDateValidator, })

    private futureDateValidator(control: AbstractControl): ValidationErrors | null {
        const dueDateInput = control.get('dueDate');
        const dueDateTimeObject = new Date(dueDateInput?.value);

        const todayDateTime = new Date();

        if (dueDateTimeObject <= todayDateTime) {
            control.get('dueDate')?.setErrors({ invalidDate: true });
        }

        return dueDateTimeObject <= todayDateTime
            ? { invalidDueDate: { value: control.value } }
            : null;
    }

    onSubmit() {
        if (this.taskManageForm.valid) {
            this.subscriptions.push(
                this.toDoTasksApiService.createToDoTask(this.taskManageForm.getRawValue()).subscribe({
                    next: () => console.log("Success"),
                    error: () => console.log("Error")
                })
            )
        }
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach((subscription) => subscription.unsubscribe);
    }
}
