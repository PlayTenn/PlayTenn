import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Place } from 'src/app/models/place';
import { PlacesApiService } from 'src/app/services/places-api.service';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { Surface } from 'src/app/models/surface';
import { SurfacesApiService } from 'src/app/services/surfaces-api.service';

@Component({
  selector: 'app-add-new-place-dialog',
  templateUrl: './add-new-place-dialog.component.html',
  styleUrls: ['./add-new-place-dialog.component.less']
})
export class AddNewPlaceDialogComponent {
  public spinnerMode: ProgressSpinnerMode = "indeterminate";
  public showSpinner: boolean = false;
  public surfaces: Surface[] = [];

  public place: FormGroup = new FormGroup({
    name: new FormControl('', [Validators.required]),
    street: new FormControl('', [Validators.required]),
    surfaceId: new FormControl(null, Validators.required)
  });

  constructor(public dialogRef: MatDialogRef<AddNewPlaceDialogComponent>,
    private readonly placeApi: PlacesApiService,
    private readonly snackBar: MatSnackBar,
    private readonly surfacesApi: SurfacesApiService) { }

  public ngOnInit() {
    this.place.valueChanges.subscribe(() => {
      console.log(this.place.controls['surfaceId'].value);
    });
    this.surfacesApi.getAllSurfaces().subscribe(surfaces => {
      this.surfaces = surfaces;
    });
  }

  public cancelClick() {
    this.dialogRef.close();
  }

  public createNewPlace() {
    if(this.place.valid) {
      this.showSpinner = true;
      const newPlace = {
        name: this.place.controls['name'].value,
        streetAddress: this.place.controls['street'].value,
        surfaceId: this.place.controls['surfaceId'].value
      } as Place
      this.placeApi.createNewPlace(newPlace).subscribe(() => {
        this.snackBar.open(`${newPlace.name} was successfully created`, 'Close', {
          duration: 3000
        });
        this.dialogRef.close();
      }, error => {
        this.snackBar.open(`${error}`, 'Close', {
          duration: 3000
        });
        this.showSpinner = false;
      }, () => {
        this.showSpinner = false;
      });
    } else {
      this.snackBar.open(`All fields are required`, 'Close', {
        duration: 3000
      });
    }
  }
}
