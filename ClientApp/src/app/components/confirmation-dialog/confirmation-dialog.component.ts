import { Component, inject, model } from '@angular/core';
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

export interface DialogData {
    type: string
}

@Component({
    selector: 'app-confirmation-dialog',
    standalone: true,
    imports: [MatDialogTitle,
        MatDialogContent,
        MatDialogActions,
        MatDialogClose,],
    templateUrl: './confirmation-dialog.component.html',
    styleUrl: './confirmation-dialog.component.css'
})
export class ConfirmationDialogComponent {
    readonly dialogRef = inject(MatDialogRef<ConfirmationDialogComponent>);
    readonly data = inject<DialogData>(MAT_DIALOG_DATA);
    readonly animal = model(this.data.type);

    onNoClick(): void {
        this.dialogRef.close();
    }
}