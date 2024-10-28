import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ToDoTask } from '../../interfaces/to-do-task';

@Component({
    selector: 'app-task-table',
    standalone: true,
    imports: [MatFormFieldModule, MatInputModule, MatTableModule, MatSortModule, MatPaginatorModule],
    templateUrl: './task-table.component.html',
    styleUrl: './task-table.component.css'
})
export class TaskTableComponent implements AfterViewInit {
    displayedColumns: string[] = ['id', 'title', 'description', 'dueDate', 'completed'];
    dataSource: MatTableDataSource<ToDoTask>;

    @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) sort!: MatSort;

    constructor() {
        const tasks: ToDoTask[] = [{
            id: 1, title: "Task 1", description: null, dueDate: new Date("2025-10-27T15:23:59.689Z"), completed: false
        }]

        this.dataSource = new MatTableDataSource(tasks);
    }

    ngAfterViewInit() {
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();

        if (this.dataSource.paginator) {
            this.dataSource.paginator.firstPage();
        }
    }
}