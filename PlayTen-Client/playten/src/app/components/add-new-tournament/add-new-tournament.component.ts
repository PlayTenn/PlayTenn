import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Place } from 'src/app/models/place';
import { TournamentInfo } from 'src/app/models/tournament';
import { PlacesApiService } from 'src/app/services/places-api.service';
import { SessionService } from 'src/app/services/session.service';
import { TournamentsApiService } from 'src/app/services/tournaments-api.service';

@Component({
  selector: 'app-add-new-tournament',
  templateUrl: './add-new-tournament.component.html',
  styleUrls: ['./add-new-tournament.component.less']
})
export class AddNewTournamentComponent implements OnInit {
  public showSpinner: boolean = false;
  public places: Place[] = [];
  public tournament: FormGroup = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.minLength(3)]),
    description: new FormControl('', Validators.maxLength(100)),
    dateStart: new FormControl(null, Validators.required),
    dateEnd: new FormControl(null, Validators.required),
    numberOfParticipants: new FormControl(0, Validators.required),
    placeId: new FormControl(null, Validators.required),
    price: new FormControl("", Validators.required),
  });

  constructor(
    private readonly placesApi: PlacesApiService,
    public dialogRef: MatDialogRef<AddNewTournamentComponent>,
    private readonly snackBar: MatSnackBar,
    private readonly session: SessionService,
    private readonly tournamentsApi: TournamentsApiService) { }

  public ngOnInit(): void {
    this.placesApi.getAllPlaces().subscribe(places => {
      this.places = places;
    });
  }

  public cancelClick() {
    this.dialogRef.close();
  }

  public createNewTournament() {
    if (!this.tournament.valid) {
      this.snackBar.open(`Форма заповненна неправильно`, 'Закрити', {
        duration: 3000
      });
      return;
    }

    const tournament: TournamentInfo = {
      name: this.tournament.controls['name'].value,
      description: this.tournament.controls['description'].value,
      dateStart: this.tournament.controls['dateStart'].value?.toLocaleString(),
      dateEnd: this.tournament.controls['dateEnd'].value?.toLocaleString(),
      numberOfParticipants: this.tournament.controls['numberOfParticipants'].value,
      ownerId: this.session.getUserFromSession().id,
      placeId: this.tournament.controls['placeId'].value,
      price: this.tournament.controls['price'].value
    }

    this.tournamentsApi.createNewTournament(tournament).subscribe({
      next: () => {
        this.dialogRef.close();
        this.snackBar.open(`Турнір ${tournament.name} створено`, 'Закрити', {
          duration: 3000
        });
      },
    })
  }
}
