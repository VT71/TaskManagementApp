import { Component } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-search',
    standalone: true,
    imports: [MatFormFieldModule, MatIconModule, MatInputModule, MatButtonModule],
    templateUrl: './search.component.html',
    styleUrl: './search.component.css'
})
export class SearchComponent {

}
