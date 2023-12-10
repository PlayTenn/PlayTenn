import { Component, OnInit } from '@angular/core';
import { AddNewTournamentComponent } from '../add-new-tournament/add-new-tournament.component';
import { MatDialog } from '@angular/material/dialog';
import { TournamentsApiService } from 'src/app/services/tournaments-api.service';
import { Tournament } from 'src/app/models/tournament';
import { SessionService } from 'src/app/services/session.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SurfacesApiService } from 'src/app/services/surfaces-api.service';
import { Surface } from 'src/app/models/surface';
import { Observable, forkJoin } from 'rxjs';
import { Router } from '@angular/router';
import { paths } from 'src/app/routes';
import { Participant } from 'src/app/models/participant';

@Component({
  selector: 'app-tournaments',
  templateUrl: './tournaments.component.html',
  styleUrls: ['./tournaments.component.less']
})
export class TournamentsComponent implements OnInit {
  public showSpinner: boolean = false;
  public tournaments: Tournament[] = [];
  public surfaces: Surface[] = [];
  public tournamentsCount: number = 0;
  public isAdmin: boolean = false;
  constructor(public readonly dialog: MatDialog,
    private readonly tournamentsApi: TournamentsApiService,
    private readonly surfacesApi: SurfacesApiService,
    private readonly session: SessionService,
    private readonly snackBar: MatSnackBar,
    private readonly router: Router) { }

  public ngOnInit(): void {
    this.showSpinner = true;
    this.getAllTournaments().subscribe({
      next: tournaments => {
        this.tournaments = tournaments;
        this.tournamentsCount = tournaments?.length;
        this.isAdmin = this.session.isAdmin();
      },
      error: () => {
        this.snackBar.open('Щось пішло не так', 'Закрити', {
          duration: 3000,
          panelClass: ['snackbar-color']
        });
        this.showSpinner = false;
      },
      complete: () => this.showSpinner = false
    });
    this.getAllSurfaces().subscribe({
      next: (surfaces) => {
        this.surfaces = surfaces;
      },
      error: () => this.snackBar.open("Failed to get surfaces", 'Close', {
        duration: 3000,
        panelClass: ['snackbar-color']
      }),
      complete: () => this.showSpinner = false
    })
  }

  public deleteTournament(id: number | undefined) {
    if (id) {
      this.tournamentsApi.deleteTournament(id).subscribe(() => {
        this.ngOnInit();
      });
    }
  }

  private getAllTournaments() {
    return this.tournamentsApi.getAllTournaments();
  }

  private getAllSurfaces(): Observable<Surface[]> {
    return this.surfacesApi.getAllSurfaces();
  }

  public getSurfaceType(surfaceId: number | undefined): string | undefined {
    var surface = this.surfaces.find(x => x.id == surfaceId);
    return surface?.type;
  }

  public openAddNewTournamentDialog() {
    const dialogRef = this.dialog.open(AddNewTournamentComponent, {
      height: 'fit-content',
      width: '600px',
    });

    dialogRef.afterClosed().subscribe(() => {
      this.getAllTournaments().subscribe(tournaments => {
        this.tournaments = tournaments;
      });
    });
  };

  public goToDetails(id: number) {
    this.router.navigate([paths.tournamentInfo], { queryParams: { id: id } });
  }

  public startTournament(id: number, numberOfParticipants: Participant[]) {
    if(numberOfParticipants.length <= 4) {
      this.snackBar.open("Недостатня кількість учасників", 'Close', {
        duration: 3000,
        panelClass: ['snackbar-color']
      });

      return;
    }
    const requests: Observable<any | any>[] = [];
    requests.push(this.tournamentsApi.generateDraw(id));
    requests.push(this.tournamentsApi.startTournament(id));

    forkJoin(requests).subscribe(() => {
      this.snackBar.open('Турнір почато', 'Закрити', {
        duration: 3000,
        panelClass: ['snackbar-color']
      });
      this.goToDetails(id);
    })
  }

  public finishTournament(id: number) {
    this.tournamentsApi.finishTournament(id).subscribe(() => {
      this.ngOnInit();
    });
  }
}
