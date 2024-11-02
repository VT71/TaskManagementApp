import { Component, Inject, inject } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AsyncPipe } from '@angular/common';
import { DOCUMENT } from '@angular/common';

@Component({
    selector: 'app-auth-button',
    standalone: true,
    imports: [MatIconModule, MatButtonModule, AsyncPipe],
    templateUrl: './auth-button.component.html',
    styleUrl: './auth-button.component.css'
})
export class AuthButtonComponent {
    constructor(@Inject(DOCUMENT) public document: Document, public auth: AuthService) { }
}
