import { Component, OnInit, ViewChild } from '@angular/core';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { map, of } from 'rxjs';
import { Match } from 'src/app/models/match';
import { Tournament } from 'src/app/models/tournament';
import { User } from 'src/app/models/user';
import { paths } from 'src/app/routes';
import { TournamentsApiService } from 'src/app/services/tournaments-api.service';
import { UserApiService } from 'src/app/services/user-api.service';
import { SetWinnerComponent } from '../set-winner/set-winner.component';

@Component({
  selector: 'app-tournament-info',
  templateUrl: './tournament-info.component.html',
  styleUrls: ['./tournament-info.component.less']
})
export class TournamentInfoComponent implements OnInit {
  public tournament: Tournament = new Tournament();
  public showSpinner: boolean = false;
  public dataSource: any;
  public matches: Match[] = [];
  public displayedColumns: string[] = ['fullName', 'level', 'status'];
  public amountOfRounds: number = 0;
  public rounds: any[] = [];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly tournamentApi: TournamentsApiService,
    private readonly snackBar: MatSnackBar,
    private readonly userApi: UserApiService,
    private bottomSheet: MatBottomSheet) {
  }

  public ngOnInit(): void {
    this.showSpinner = true;
    const idFromRoute = this.route.snapshot.queryParams["id"]

    if (!idFromRoute) {
      this.tournament = null;
      this.snackBar.open('Щось пішло не так', 'Закрити', {
        duration: 3000,
        panelClass: ['snackbar-color']
      });
      this.showSpinner = false;
      this.router.navigate([paths.tournaments]);

      return;
    }

    this.tournamentApi.getTournamentById(+idFromRoute).subscribe((tournament: Tournament) => {
      this.tournament = tournament;
      this.amountOfRounds = tournament.tournament.amountOfRounds;
      this.dataSource = tournament.tournament.participants;
      if (this.tournament.tournament.hasStarted) {
        this.tournamentApi.getAllMatches(tournament.tournament.tournamentId).subscribe((matches: Match[]) => {
          this.matches = matches;
          const numberOfParticipants = tournament.tournament.participants.length;
          // Find the next power of 2
          const nextPower = this.nextPowerOf2(numberOfParticipants);

          // Adjust the number of participants
          const adjustedParticipants = nextPower !== numberOfParticipants ? nextPower : numberOfParticipants;
          this.amountOfRounds = Math.log2(adjustedParticipants);

          // Calculate and assign rounds to matches
          let matchIndex = 0;
          for (let round = 1; round <= this.amountOfRounds; round++) {
            const matchesInRoundCount = adjustedParticipants / Math.pow(2, round);

            let matchesInRound = [] as any;
            for (let i = 0; i < matchesInRoundCount; i++) {
              matchesInRound.push(matches[matchIndex]);
              matchIndex++;
            }
            this.rounds.push(matchesInRound);
          }
          console.log(this.rounds);
        });
      }
      this.showSpinner = false;
    });
  }

  public getParticipant(id: string) {
    if (!id) {
      return of(null);
    }
    return this.userApi.getUserById(id).pipe(map(user => {
      return user;
    })).subscribe();
  }

  public nextPowerOf2(n: number): number {
    let power = 1;
    while (power < n) {
      power *= 2;
    }
    return power;
  }

  public openSetWinnerSheet(matches: Match[], nextMatch: Match, player: number) {
    const bottomSheetRef = this.bottomSheet.open(SetWinnerComponent, {
      data: { matches: matches, nextMatch: nextMatch, player: player },
    });

    bottomSheetRef.afterDismissed().subscribe(() => {
      this.ngOnInit();
    })
  }
}
