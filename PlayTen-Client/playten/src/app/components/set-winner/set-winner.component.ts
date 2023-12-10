import { Component, Inject } from '@angular/core';
import { MAT_BOTTOM_SHEET_DATA, MatBottomSheetRef } from '@angular/material/bottom-sheet';
import { Match } from 'src/app/models/match';
import { User } from 'src/app/models/user';
import { TournamentsApiService } from 'src/app/services/tournaments-api.service';

@Component({
  selector: 'app-set-winner',
  templateUrl: './set-winner.component.html',
  styleUrls: ['./set-winner.component.less']
})
export class SetWinnerComponent {
  public matches: Match[] = [];
  selectedValue: number;
  selectedPlayerId: string;
  score: string = "";
  constructor(@Inject(MAT_BOTTOM_SHEET_DATA) public data: { matches: Match[], nextMatch: Match, player: number }, private readonly tournamentApi: TournamentsApiService, private _bottomSheetRef: MatBottomSheetRef<SetWinnerComponent>) {
    this.matches = data.matches;
  }

  public getPlayer() {
    return this.matches.find(m => +m.id === this.selectedValue);
  }

  public save() {
    const matchToUpdate = this.matches.find(m => +m.id === this.selectedValue);

    matchToUpdate.winnerId = this.selectedPlayerId;
    matchToUpdate.score = this.score;

    this.tournamentApi.updateMatch(matchToUpdate).subscribe(() => {
      const test = () => this.matches.find(m => m.id === this.data.nextMatch.id);
      if (this.matches.findIndex(test) % 2 !== 0)
        this.data.nextMatch.player1Id = this.selectedPlayerId;
      else
        this.data.nextMatch.player2Id = this.selectedPlayerId;

      this.tournamentApi.updateMatch(this.data.nextMatch).subscribe(() => {
        window.location.reload();
      })
    });
  }
}
