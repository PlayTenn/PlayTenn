import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule, MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatBadgeModule } from '@angular/material/badge';
import { MatTabsModule } from '@angular/material/tabs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatListModule } from '@angular/material/list';
import { MatBottomSheetModule, MAT_BOTTOM_SHEET_DEFAULT_OPTIONS } from '@angular/material/bottom-sheet';
import { MatDialogModule } from '@angular/material/dialog';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { HomeComponent } from './components/home/home.component';
import { AuthComponent } from './components/auth/auth.component';
import { MatMenuModule } from '@angular/material/menu';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { ProfileComponent } from './components/profile/profile.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { Http401Interceptor } from './interceptors/http401.interceptor';
import { PlacesComponent } from './components/places/places.component';
import { AddNewPlaceDialogComponent } from './components/add-new-place-dialog/add-new-place-dialog.component';
import { UsersComponent } from './components/users/users.component';
import { SessionExpiredComponent } from './components/session-expired/session-expired.component';
import { EditPlaceDialogComponent } from './components/edit-place-dialog/edit-place-dialog.component';
import { CalendarCommonModule, CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { CalendarComponent } from './components/calendar/calendar.component';
import { TrainingsComponent } from './components/trainings/trainings.component';
import { AddNewTrainingDialogComponent } from './components/add-new-training-dialog/add-new-training-dialog.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import {MatRadioModule} from '@angular/material/radio';
import {
  NgxMatDatetimePickerModule,
  NgxMatNativeDateModule,
  NgxMatTimepickerModule
} from '@angular-material-components/datetime-picker';
import { AvatarComponent } from './components/avatar/avatar.component';
import { TrainingActionsComponent } from './components/trainings/training-actions/training-actions.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { TournamentsComponent } from './components/tournaments/tournaments.component';
import { AddNewTournamentComponent } from './components/add-new-tournament/add-new-tournament.component';
import { TournamentInfoComponent } from './components/tournament-info/tournament-info.component';
import { SetWinnerComponent } from './components/set-winner/set-winner.component';

const MatModules = [
  MatButtonModule,
  MatToolbarModule,
  MatIconModule,
  MatFormFieldModule,
  MatRadioModule,
  MatInputModule,
  MatSnackBarModule,
  MatProgressSpinnerModule,
  MatTooltipModule,
  MatSelectModule,
  MatCardModule,
  MatListModule,
  MatExpansionModule,
  MatCheckboxModule,
  MatDialogModule,
  MatBottomSheetModule,
  MatMenuModule,
  MatTabsModule,
  MatTableModule,
  MatPaginatorModule,
  MatChipsModule,
  MatBadgeModule,
  MatDatepickerModule,
  MatNativeDateModule,
  NgxMatDatetimePickerModule,
  NgxMatTimepickerModule,
  NgxMatNativeDateModule
]

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AuthComponent,
    NotFoundComponent,
    ProfileComponent,
    PlacesComponent,
    AddNewPlaceDialogComponent,
    UsersComponent,
    SessionExpiredComponent,
    EditPlaceDialogComponent,
    CalendarComponent,
    TrainingsComponent,
    AddNewTrainingDialogComponent,
    AvatarComponent,
    TrainingActionsComponent,
    TournamentsComponent,
    AddNewTournamentComponent,
    TournamentInfoComponent,
    SetWinnerComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ...MatModules,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory,
    }),
    CalendarCommonModule
  ],
  providers: [
    { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: { appearance: 'outline' } },
    { provide: MAT_BOTTOM_SHEET_DEFAULT_OPTIONS, useValue: { hasBackdrop: false } },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: Http401Interceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
