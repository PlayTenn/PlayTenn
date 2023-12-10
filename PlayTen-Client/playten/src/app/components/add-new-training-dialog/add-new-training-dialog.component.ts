import { Component, OnInit, ViewChild } from '@angular/core';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TrainingApiService } from 'src/app/services/training-api.service';
import { Training, TrainingInfo } from 'src/app/models/training';
import { SessionService } from 'src/app/services/session.service';
import { PlacesApiService } from 'src/app/services/places-api.service';
import { Place } from 'src/app/models/place';
import { ThemePalette } from '@angular/material/core';

@Component({
  selector: 'app-add-new-training-dialog',
  templateUrl: './add-new-training-dialog.component.html',
  styleUrls: ['./add-new-training-dialog.component.less']
})
export class AddNewTrainingDialogComponent implements OnInit {
  @ViewChild('picker1') picker1: any;
  @ViewChild('picker2') picker2: any;
  public stepHours = [1, 2, 3, 4, 5];
  public stepMinutes = [1, 5, 10, 15, 20, 25];
  public stepSeconds = [1, 5, 10, 15, 20, 25];
  public stepHour = 1;
  public stepMinute = 1;
  public color: ThemePalette = 'primary';
  public spinnerMode: ProgressSpinnerMode = "indeterminate";
  public showSpinner: boolean = false;
  public places: Place[] = [];
  public training: FormGroup = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.maxLength(50), Validators.minLength(3)]),
    description: new FormControl('', Validators.maxLength(100)),
    dateStart: new FormControl(null, Validators.required),
    dateEnd: new FormControl(null, Validators.required),
    hasBalls: new FormControl(false, Validators.required),
    numberOfParticipants: new FormControl(0, Validators.required),
    placeId: new FormControl(null, Validators.required)
  });

  constructor(public dialogRef: MatDialogRef<AddNewTrainingDialogComponent>,
    private readonly snackBar: MatSnackBar,
    private readonly trainingApi: TrainingApiService,
    private readonly session: SessionService,
    private readonly placesApi: PlacesApiService) { }

  public ngOnInit() {
    this.training.valueChanges.subscribe(() => {
      console.log(this.training.controls['placeId'].value);
    });
    this.placesApi.getAllPlaces().subscribe(places => {
      this.places = places;
    });
  }

  public cancelClick() {
    this.dialogRef.close();
  }

  public createNewTraining() {
    if (!this.training.valid) {
      this.snackBar.open(`Форма заповненна неправильно`, 'Закрити', {
        duration: 3000
      });
      return;
    }
    
    const training: TrainingInfo = {
      name: this.training.controls['name'].value,
      description: this.training.controls['description'].value,
      dateStart: this.training.controls['dateStart'].value?.toLocaleString(),
      dateEnd: this.training.controls['dateEnd'].value?.toLocaleString(),
      hasBalls: this.training.controls['hasBalls'].value,
      numberOfParticipants: this.training.controls['numberOfParticipants'].value,
      ownerId: this.session.getUserFromSession().id,
      placeId: this.training.controls['placeId'].value
    }

    this.trainingApi.createNewTraining(training).subscribe( {
      next: () => {
        this.dialogRef.close();
        this.snackBar.open(`Тренування ${training.name} створено`, 'Закрити', {
          duration: 3000
        });
      },
    })
  }
}
