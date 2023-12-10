import { Routes } from "@angular/router";
import { AuthComponent } from "./components/auth/auth.component";
import { CalendarComponent } from "./components/calendar/calendar.component";
import { HomeComponent } from "./components/home/home.component";
import { NotFoundComponent } from "./components/not-found/not-found.component";
import { PlacesComponent } from "./components/places/places.component";
import { ProfileComponent } from "./components/profile/profile.component";
import { TrainingsComponent } from "./components/trainings/trainings.component";
import { UsersComponent } from "./components/users/users.component";
import { AuthGuard } from "./services/auth.guard";
import { TournamentsComponent } from "./components/tournaments/tournaments.component";
import { TournamentInfoComponent } from "./components/tournament-info/tournament-info.component";

export const paths = {
 home: 'home',
 auth: 'auth',
 profile: 'profile',
 places: 'places',
 trainings: 'trainings',
 tournaments: 'tournaments',
 tournamentInfo: 'tournament-info',
 users: 'users',
 calendar: 'calendar'
}

export const routes: Routes = [
  { path: paths.home, title: 'Home', component: HomeComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: paths.auth, component: AuthComponent},
  { path: paths.profile, title: 'Profile', canActivate:[AuthGuard], component: ProfileComponent},
  { path: paths.places, title: 'Places', canActivate:[AuthGuard], component: PlacesComponent},
  { path: paths.trainings, title: 'Trainings', canActivate:[AuthGuard], component: TrainingsComponent},
  { path: paths.tournaments, title: 'Tournaments', canActivate:[AuthGuard], component: TournamentsComponent},
  { path: paths.tournamentInfo, title: 'Tournament Info', canActivate:[AuthGuard], component: TournamentInfoComponent},
  { path: paths.users, title: 'Users', canActivate:[AuthGuard], component: UsersComponent},
  { path: paths.calendar, title: 'Calendar', component: CalendarComponent},
  { path: '**', component: NotFoundComponent }
];
