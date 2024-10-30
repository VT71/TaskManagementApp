import { Component, inject, model, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
    MAT_DIALOG_DATA,
    MatDialog,
    MatDialogActions,
    MatDialogClose,
    MatDialogContent,
    MatDialogRef,
    MatDialogTitle,
} from '@angular/material/dialog';
import { ToDoTask } from '../../interfaces/to-do-task';
import { Subscription } from 'rxjs';
import { TodotasksApiService } from '../../services/todotasks-api.service';

export interface DialogData {
    type: string;
    toDoTask: ToDoTask;
}

@Component({
    selector: 'app-confirmation-dialog',
    standalone: true,
    imports: [MatButtonModule, MatDialogTitle,
        MatDialogContent,
        MatDialogActions,
        MatDialogClose,],
    templateUrl: './confirmation-dialog.component.html',
    styleUrl: './confirmation-dialog.component.css'
})
export class ConfirmationDialogComponent implements OnInit, OnDestroy {
    private subscriptions: Subscription[] = []
    private toDoTasksApiService = inject(TodotasksApiService);

    readonly dialogRef = inject(MatDialogRef<ConfirmationDialogComponent>);
    readonly data = inject<DialogData>(MAT_DIALOG_DATA);
    readonly type = model(this.data.type);
    readonly toDoTask = model(this.data.toDoTask);

    public title: string = '';

    ngOnInit(): void {
        if (this.type() === 'mark-as-complete') {
            this.title = 'Mark this task as Complete ?';
        } else if (this.type() === 'delete') {
            this.title = 'Delete this task ?'
        }
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach((subscription) => subscription.unsubscribe())
    }

    onConfirm() {
        if (this.type() === 'mark-as-complete') {
            this.subscriptions.push(
                this.toDoTasksApiService.updateToDoTask(this.toDoTask().id, this.toDoTask()).subscribe(
                    {
                        next: () => console.log("Success"),
                        error: () => console.log("Error")
                    }
                )
            )
        } else if (this.type() === 'delete') {
        }
    }

    onNoClick(): void {
        this.dialogRef.close();
    }
}