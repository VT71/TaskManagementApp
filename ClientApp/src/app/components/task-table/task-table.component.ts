import { AfterViewInit, Component, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { ToDoTask } from '../../interfaces/to-do-task';
import { TodotasksApiService } from '../../services/todotasks-api.service';
import { Subscription } from 'rxjs';
import {
    MatSnackBar, MatSnackBarHorizontalPosition,
    MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterLink } from '@angular/router';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
    selector: 'app-task-table',
    standalone: true,
    imports: [MatFormFieldModule, MatInputModule, MatTableModule, MatSortModule, MatPaginatorModule, MatIconModule, MatMenuModule, MatButtonModule, RouterLink],
    templateUrl: './task-table.component.html',
    styleUrl: './task-table.component.css'
})

export class TaskTableComponent implements AfterViewInit, OnInit, OnDestroy {
    displayedColumns: string[] = ['id', 'title', 'description', 'dueDate', 'completed', 'options-menu'];
    dataSource: MatTableDataSource<ToDoTask> = new MatTableDataSource();

    @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) sort!: MatSort;

    private router = inject(Router);

    private subscriptions: Subscription[] = [];

    private toDoTasksApiService = inject(TodotasksApiService);
    public toDoTasks: ToDoTask[] = [];

    private _snackBar = inject(MatSnackBar);
    horizontalPosition: MatSnackBarHorizontalPosition = 'end';
    verticalPosition: MatSnackBarVerticalPosition = 'top';

    readonly dialog = inject(MatDialog);

    ngOnInit(): void {
        this.getTasks();
    }

    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach((subcription) => subcription.unsubscribe);
    }

    getTasks() {
        console.log("Getting tasks")
        this.subscriptions.push(
            this.toDoTasksApiService.getAllToDoTasks().subscribe(
                {
                    next: (toDoTasks) => {
                        this.toDoTasks = toDoTasks;
                        console.log(toDoTasks)
                        // if (this.dataSource) {
                        //     console.log("UPDATING DATA")
                        //     this.dataSource.data = toDoTasks;
                        // } else {
                        //     this.dataSource = new MatTableDataSource();
                        //     this.dataSource.data = toDoTasks;
                        // }
                        this.dataSource = new MatTableDataSource(toDoTasks);
                    },
                    error: (e) => this.openSnackBar("Error occurred when getting Tasks Data")
                })
        )
    }

    openDialog(type: string, toDoTask: ToDoTask): void {
        const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
            data: { type, toDoTask },
            width: '300px'
        });
    }

    onMarkAsComplete(toDoTask: ToDoTask) {
        this.openDialog('mark-as-complete', toDoTask)
    }

    onDeleteClick(toDoTask: ToDoTask) {
        this.openDialog('delete', toDoTask)
    }

    onEditClick(id: number) {
        if (id) {
            this.router.navigateByUrl(`tasks/edit-task/${id}`);
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

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();

        if (this.dataSource.paginator) {
            this.dataSource.paginator.firstPage();
        }
    }

    convertDateToReadable(date: Date) {
        return date.toLocaleDateString('en-GB');
    }
}