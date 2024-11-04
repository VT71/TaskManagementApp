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
import { Router } from '@angular/router';

// Interface for data passed to the dialog
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

    private toDoTasksApiService = inject(TodotasksApiService); // Inject the service for API calls

    // Inject the dialog reference and the data passed to the dialog
    readonly dialogRef = inject(MatDialogRef<ConfirmationDialogComponent>);
    readonly data = inject<DialogData>(MAT_DIALOG_DATA);
    readonly type = this.data.type;
    readonly toDoTask = this.data.toDoTask;

    public title: string = '';

    ngOnInit(): void {
        // Set the dialog title based on the action type
        if (this.type === 'mark-as-complete') {
            this.title = 'Mark this task as Complete ?';
        } else if (this.type === 'delete') {
            this.title = 'Delete this task ?'
        }
    }

    ngOnDestroy(): void {
        // Unsubscribe from all active subscriptions
        this.subscriptions.forEach((subscription) => subscription.unsubscribe())
    }

    // Method to handle confirmation action
    onConfirm() {
        // Check action type and perform corresponding API call
        if (this.type === 'mark-as-complete') {
            this.subscriptions.push(
                this.toDoTasksApiService.markToDoTaskComplete(this.toDoTask.id, this.toDoTask).subscribe(
                    {
                        next: () => {
                            console.log("Success");
                            this.toDoTask.completed = true;
                            this.dialogRef.close(); // Close the dialog
                        },
                        error: () => console.log("Error")
                    }
                )
            )
        } else if (this.type === 'delete') {
            this.subscriptions.push(
                this.toDoTasksApiService.deleteToDoTask(this.toDoTask.id).subscribe(
                    {
                        next: () => {
                            console.log("Success");
                            this.dialogRef.close(); // Close the dialog
                            window.location.reload(); // Reload the page to reflect changes
                        },
                        error: () => console.log("Error")
                    }
                )
            )
        }
    }

    onNoClick(): void {
        this.dialogRef.close(); // Close the dialog
    }
}