import { Component, EventEmitter, inject, OnDestroy, OnInit, Output } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { NonNullableFormBuilder } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';
import { TodotasksApiService } from '../../services/todotasks-api.service';
import { ToDoTask } from '../../interfaces/to-do-task';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'app-search',
    standalone: true,
    imports: [MatFormFieldModule, MatIconModule, MatInputModule, MatButtonModule, ReactiveFormsModule],
    templateUrl: './search.component.html',
    styleUrl: './search.component.css'
})
export class SearchComponent implements OnDestroy, OnInit {
    private formBuilder = inject(NonNullableFormBuilder);
    private subscriptions: Subscription[] = [];
    private prevCriteria!: string;
    private router = inject(Router);
    private route = inject(ActivatedRoute);

    public searchForm = this.formBuilder.group({
        searchCriteria: [''],
    });

    ngOnInit(): void {
        this.subscriptions.push(this.route.queryParamMap.subscribe(params => {
            const searchParam = params.get('search');
            if (searchParam) {
                this.searchForm.get("searchCriteria")?.setValue(searchParam)
                this.prevCriteria = searchParam;
            }
        }));
    }

    onSubmit() {
        if (this.searchForm.valid) {
            console.log("Form valid")
            let criteriaToSend = this.searchForm.get("searchCriteria")?.value;
            if (criteriaToSend && criteriaToSend !== this.prevCriteria) {
                console.log("Form valid 1")
                this.prevCriteria = criteriaToSend;
                const queryParams = { ...this.route.snapshot.queryParams };
                queryParams['search'] = criteriaToSend;
                this.router.navigate([], { queryParams });
            }
        }
    }

    onClear(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        if (filterValue === '' && filterValue !== this.prevCriteria) {
            console.log("Form valid 2")
            this.prevCriteria = filterValue;
            this.router.navigate([], {
                queryParams: {
                    'search': null,
                },
                queryParamsHandling: 'merge'
            })
        }
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach((subscription) => subscription.unsubscribe())
    }
}

