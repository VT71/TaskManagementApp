import { Component, inject, Injectable, Input, OnDestroy, OnInit } from '@angular/core';
import { MatPaginatorIntl, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Subscription } from 'rxjs';

// Custom implementation of Material MatPaginatorIntl for internationalization
@Injectable()
export class MyCustomPaginatorIntl implements MatPaginatorIntl {
    changes = new Subject<void>();

    firstPageLabel = $localize`First page`;
    itemsPerPageLabel = $localize`Items per page:`;
    lastPageLabel = $localize`Last page`;

    nextPageLabel = 'Next page'; // Label for the next page button
    previousPageLabel = 'Previous page'; // Label for the previous page button

    // Generates the range label for the paginator
    getRangeLabel(page: number, pageSize: number, length: number): string {
        if (length === 0) {
            return $localize`Page 1 of 1`;
        }
        const amountPages = Math.ceil(length / pageSize);
        return $localize`Page ${page + 1} of ${amountPages}`;
    }
}

@Component({
    selector: 'app-paginator',
    standalone: true,
    imports: [MatPaginatorModule],
    providers: [{ provide: MatPaginatorIntl, useClass: MyCustomPaginatorIntl }],
    templateUrl: './paginator.component.html',
    styleUrl: './paginator.component.css'
})
export class PaginatorComponent implements OnInit, OnDestroy {
    @Input() toDoTasksCount!: number; // Input property for the total count of tasks

    // Inject necessary services
    private router = inject(Router);
    private route = inject(ActivatedRoute)

    private subscriptions: Subscription[] = []

    public page: number = 0; // Current page index (0-based)
    public pageSize: number = 10; // Default number of items per page

    // Handles page change events from the paginator
    handlePageEvent(e: PageEvent) {
        const queryParams = { ...this.route.snapshot.queryParams }; // Get current query params
        queryParams['page'] = e.pageIndex + 1;
        queryParams['pageSize'] = e.pageSize;
        this.router.navigate([], { queryParams }); // Navigate with updated query params
    }

    ngOnInit(): void {
        // Subscribe to query params to set page and pageSize when component initializes
        this.subscriptions.push(this.route.queryParamMap.subscribe(params => {
            const pageParam = params.get('page');
            const pageSizeParam = params.get('pageSize');
            if (Number(pageParam) && Number(pageSizeParam)) {
                this.page = Number(pageParam) - 1
                this.pageSize = Number(pageSizeParam)
            }
        }));
    }

    ngOnDestroy(): void {
        // Unsubscribe from all active subscriptions
        this.subscriptions.forEach((subscription) => subscription.unsubscribe())
    }

}