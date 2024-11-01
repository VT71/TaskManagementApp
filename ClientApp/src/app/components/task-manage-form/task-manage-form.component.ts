import { Component, inject, Input, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormsModule, NonNullableFormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { TodotasksApiService } from '../../services/todotasks-api.service';
import { concatMap, map, Observable, of, Subscription } from 'rxjs';
import { ToDoTask } from '../../interfaces/to-do-task';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import {
    MatSnackBar, MatSnackBarHorizontalPosition,
    MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';


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
export class TaskManageFormComponent implements OnDestroy, OnInit {
    @Input() type!: string;

    private toDoTasksApiService = inject(TodotasksApiService)
    private formBuilder = inject(NonNullableFormBuilder);

    private subscriptions: Subscription[] = [];

    private router = inject(Router);
    private route = inject(ActivatedRoute);

    private _snackBar = inject(MatSnackBar);
    horizontalPosition: MatSnackBarHorizontalPosition = 'end';
    verticalPosition: MatSnackBarVerticalPosition = 'top';

    public toDoTask!: ToDoTask;

    public taskManageForm = this.formBuilder.group({
        id: [0],
        title: ['', [Validators.required, Validators.maxLength(100)]],
        description: [''],
        dueDate: [new Date(), [Validators.required]],
        completed: [false, [Validators.required]]
    },
        { validators: this.futureDateValidator, })

    ngOnInit(): void {
        if (this.type === 'edit') {
            this.subscriptions.push(
                this.route.paramMap.pipe(
                    concatMap((paramMap) => {
                        let taskId = Number(paramMap.get("id"));
                        if (taskId) {
                            return this.toDoTasksApiService.getToDoTask(taskId);
                        } else {
                            return of();
                        }
                    })
                ).subscribe(
                    {
                        next: (toDoTask) => {
                            this.populateForm(toDoTask)
                            this.toDoTask = toDoTask;
                        },
                        error: (e) => console.log("Error occurred when getting task data.")
                    })
            )
        }
    }

    private populateForm(toDoTask: ToDoTask) {
        this.taskManageForm.markAsPristine()
        this.taskManageForm.get("id")?.setValue(toDoTask.id);
        this.taskManageForm.get("title")?.setValue(toDoTask.title);
        this.taskManageForm.get("description")?.setValue(toDoTask.description ?? '');
        this.taskManageForm.get("dueDate")?.setValue(toDoTask.dueDate);
        this.taskManageForm.get("completed")?.setValue(toDoTask.completed);
    }

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
        if (this.type === 'add') {
            if (this.taskManageForm.valid) {
                this.subscriptions.push(
                    this.toDoTasksApiService.createToDoTask(this.taskManageForm.getRawValue()).subscribe({
                        next: (newToDoTask) => {
                            this.router.navigateByUrl(`tasks/edit-task/${newToDoTask.id}`);
                            this.openSnackBar("Task created successfully")
                        },
                        error: () => console.log("Error")
                    })
                )
            }
        } else if (this.type === 'edit') {
            if (this.taskManageForm.valid) {
                this.subscriptions.push(
                    this.toDoTasksApiService.updateToDoTask(this.toDoTask.id, this.taskManageForm.getRawValue()).subscribe({
                        next: () => console.log("Success"),
                        error: () => console.log("Error")
                    })
                )
            }
        }
    }

    openSnackBar(message: string) {
        let durationInSeconds = 5;
        this._snackBar.open(message, '', {
            horizontalPosition: this.horizontalPosition,
            verticalPosition: this.verticalPosition,
            duration: durationInSeconds * 1000
        });
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach((subscription) => subscription.unsubscribe());
    }
}
