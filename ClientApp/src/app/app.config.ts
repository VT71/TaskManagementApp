import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { authHttpInterceptorFn, provideAuth0 } from '@auth0/auth0-angular';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
    providers: [provideRouter(routes), provideAnimationsAsync(), provideHttpClient(),
    provideHttpClient(withInterceptors([authHttpInterceptorFn])),
    provideAuth0({
        domain: 'dev-1ehyyzsy67kkgexr.us.auth0.com',
        clientId: 'uDhsXQorgbV3316nAnTgsq2ptkxpwNqh',
        authorizationParams: {
            redirect_uri: window.location.origin
        },
        httpInterceptor: {
            allowedList: [`${environment.api.serverUrl}/ToDoTask`],
        },
    }),]
};
