import { AfterViewInit, ChangeDetectorRef, Component, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
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
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { SearchComponent } from '../search/search.component';
import { PaginatorComponent } from '../paginator/paginator.component';

@Component({
    selector: 'app-task-table',
    standalone: true,
    imports: [MatFormFieldModule, MatInputModule, MatTableModule, MatSortModule, MatPaginatorModule, MatIconModule, MatMenuModule, MatButtonModule, RouterLink, SearchComponent, PaginatorComponent],
    templateUrl: './task-table.component.html',
    styleUrl: './task-table.component.css'
})

export class TaskTableComponent implements AfterViewInit, OnInit, OnDestroy {
    // @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) matSort!: MatSort;

    private router = inject(Router);
    private route = inject(ActivatedRoute);

    private subscriptions: Subscription[] = [];

    private toDoTasksApiService = inject(TodotasksApiService);
    public toDoTasks: ToDoTask[] = [];
    public toDoTasksCount = 0;

    // Snackbar instance and configuration
    // for displaying messages
    private _snackBar = inject(MatSnackBar);
    horizontalPosition: MatSnackBarHorizontalPosition = 'end';
    verticalPosition: MatSnackBarVerticalPosition = 'top';

    public sortByParamValue = '';
    public sortDirectionParamValue = '';

    readonly dialog = inject(MatDialog);

    // ChangeDetectorRef instance for manually triggering change detection.
    private cdref = inject(ChangeDetectorRef);

    ngOnInit(): void {
        // Subscribe to query parameters to retrieve search, sorting,
        // and pagination options, and retrieve tasks based on these.
        this.subscriptions.push(this.route.queryParamMap.subscribe(params => {
            const titleSearchParam = params.get('search');
            const sortByParam = params.get('sortBy');
            const sortDirectionParam = params.get('sortDirection');
            const pageParam = params.get('page');
            const pageSizeParam = params.get('pageSize');

            if (sortDirectionParam && sortByParam) {
                this.sortByParamValue = sortByParam;
                this.sortDirectionParamValue = sortDirectionParam
            }

            this.getTasks(titleSearchParam, sortByParam, sortDirectionParam, pageParam, pageSizeParam)
        }));
    }

    ngAfterViewInit() {
        // Set initial sort state if provided by query parameters.
        if (this.sortDirectionParamValue && this.sortByParamValue) {
            this.matSort.active = this.sortByParamValue;
            this.matSort.direction = this.sortDirectionParamValue === 'desc' ? this.sortDirectionParamValue as 'desc' : 'asc';
            this.matSort.sortChange.emit({
                active: this.matSort.active,
                direction: this.matSort.direction,
            });
        }
        this.cdref.detectChanges();
    }

    ngOnDestroy(): void {
        // Unsubscribe from all active subscriptions
        this.subscriptions.forEach((subcription) => subcription.unsubscribe());
    }

    getTasks(titleSearch: string | null, sortBy: string | null, sortDirection: string | null, pageParam: string | null, pageSizeParam: string | null) {
        this.subscriptions.push(
            this.toDoTasksApiService.getToDoTasksByCriteria(titleSearch, sortBy, sortDirection, pageParam, pageSizeParam).subscribe({
                next: (pagedResult) => {
                    this.toDoTasks = pagedResult.items;
                    this.toDoTasksCount = pagedResult.totalCount;
                },
                error: () => {
                    this.openSnackBar("An error occurred while getting the tasks.")
                }
            })
        );
    }

    // Open a confirmation dialog for the specified action type and task.
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

    // Open a snackbar with the given message for a set duration.
    openSnackBar(message: string) {
        let durationInSeconds = 5;
        this._snackBar.open(message, '', {
            horizontalPosition: this.horizontalPosition,
            verticalPosition: this.verticalPosition,
            duration: durationInSeconds * 1000
        });
    }

    // Update query parameters based on the sort change and navigate.
    onSortChange(sort: Sort) {
        if (sort.active && sort.direction) {
            const queryParams = { ...this.route.snapshot.queryParams };
            queryParams['sortBy'] = sort.active;
            queryParams['sortDirection'] = sort.direction;
            this.router.navigate([], { queryParams });
        } else {
            this.router.navigate([], {
                queryParams: {
                    'sortBy': null,
                    'sortDirection': null
                },
                queryParamsHandling: 'merge'
            })
        }
        return;
    }

    convertDateToReadable(date: Date) {
        return date.toLocaleDateString('en-GB');
    }
}