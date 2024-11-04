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
    // Inject necessary services
    private formBuilder = inject(NonNullableFormBuilder);
    private router = inject(Router);
    private route = inject(ActivatedRoute);

    private subscriptions: Subscription[] = [];

    private prevCriteria!: string; // Previous search term

    // Form group for managing search criteria
    public searchForm = this.formBuilder.group({
        searchCriteria: [''],
    });

    ngOnInit(): void {
        // Subscribe to query parameters and update the form if 'search' parameter exists
        this.subscriptions.push(this.route.queryParamMap.subscribe(params => {
            const searchParam = params.get('search');
            if (searchParam) {
                this.searchForm.get("searchCriteria")?.setValue(searchParam) // Set search criteria in the form
                this.prevCriteria = searchParam;
            }
        }));
    }

    // Handle form submission
    onSubmit() {
        if (this.searchForm.valid) {
            let criteriaToSend = this.searchForm.get("searchCriteria")?.value; // Get current search criteria
            if (criteriaToSend && criteriaToSend !== this.prevCriteria) {
                this.prevCriteria = criteriaToSend;
                const queryParams = { ...this.route.snapshot.queryParams }; // Preserve existing query parameters
                queryParams['search'] = criteriaToSend;
                queryParams['page'] = '1';
                this.router.navigate([], { queryParams }); // Navigate with updated query parameters
            }
        }
    }

    // Handle clearing the search
    onClear(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        if (filterValue === '' && filterValue !== this.prevCriteria) {
            this.prevCriteria = filterValue;
            this.router.navigate([], {
                queryParams: {
                    'search': null, // Clear search parameter
                    'page': '1' // Reset to first page
                },
                queryParamsHandling: 'merge' // Merge with existing parameters
            })
        }
    }

    ngOnDestroy(): void {
        // Unsubscribe from all active subscriptions
        this.subscriptions.forEach((subscription) => subscription.unsubscribe())
    }
}

