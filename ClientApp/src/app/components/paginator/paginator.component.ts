import { Component, inject, Injectable, Input, OnDestroy, OnInit } from '@angular/core';
import { MatPaginatorIntl, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Subscription } from 'rxjs';

@Injectable()
export class MyCustomPaginatorIntl implements MatPaginatorIntl {
    changes = new Subject<void>();

    // For internationalization, the `$localize` function from
    // the `@angular/localize` package can be used.
    firstPageLabel = $localize`First page`;
    itemsPerPageLabel = $localize`Items per page:`;
    lastPageLabel = $localize`Last page`;

    // You can set labels to an arbitrary string too, or dynamically compute
    // it through other third-party internationalization libraries.
    nextPageLabel = 'Next page';
    previousPageLabel = 'Previous page';

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
    @Input() toDoTasksCount!: number;

    private subscriptions: Subscription[] = []

    private router = inject(Router);
    private route = inject(ActivatedRoute)

    public page: number = 0;
    public pageSize: number = 10;

    handlePageEvent(e: PageEvent) {
        const queryParams = { ...this.route.snapshot.queryParams };
        queryParams['page'] = e.pageIndex + 1;
        queryParams['pageSize'] = e.pageSize;
        this.router.navigate([], { queryParams });
    }

    ngOnInit(): void {
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
        this.subscriptions.forEach((subscription) => subscription.unsubscribe())
    }

}