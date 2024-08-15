import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { ExpiringSessionCountdownComponent } from './components/modals/expiring-session-countdown/expiring-session-countdown.component';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { NotificationComponent } from './components/modals/notification/notification.component';
import { UserHasRoleDirective } from './directives/user-has-role.directive';
import { ValidationMessagesComponent } from './components/errors/validation-messages/validation-messages.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ModalModule } from 'ngx-bootstrap/modal';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
@NgModule({
  declarations: [
    NotFoundComponent,
    ValidationMessagesComponent,
    NotificationComponent,
    UserHasRoleDirective,
    ExpiringSessionCountdownComponent,
  ],
  imports: [
    FormsModule,
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatCheckboxModule,
    MatSelectModule,
    ModalModule.forRoot(),
  ],
  exports: [
    FormsModule,
    RouterModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    MatDatepickerModule,
    MatCheckboxModule,
    MatSelectModule,
    ValidationMessagesComponent,
    UserHasRoleDirective,
  ],
})
export class SharedModule {}
