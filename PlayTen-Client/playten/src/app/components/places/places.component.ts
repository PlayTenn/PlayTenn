import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { Place } from 'src/app/models/place';
import { Surface } from 'src/app/models/surface';
import { PlacesApiService } from 'src/app/services/places-api.service';
import { SurfacesApiService } from 'src/app/services/surfaces-api.service';
import { SessionService } from 'src/app/services/session.service';
import { AddNewPlaceDialogComponent } from '../add-new-place-dialog/add-new-place-dialog.component';
import { EditPlaceDialogComponent } from '../edit-place-dialog/edit-place-dialog.component';

@Component({
  selector: 'app-places',
  templateUrl: './places.component.html',
  styleUrls: ['./places.component.less']
})
export class PlacesComponent implements OnInit {
  public panelOpenState: boolean = false;
  public places!: Place[];
  public surfaces!: Surface[];
  public isAdmin: boolean = false;
  public showSpinner: boolean = false;
  public spinnerMode: ProgressSpinnerMode = "indeterminate";

  constructor(
    private readonly placesApi: PlacesApiService,
    private readonly surfacesApi: SurfacesApiService,
    private readonly session: SessionService,
    public readonly dialog: MatDialog,
    private readonly snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.showSpinner = true;
    this.getAllPlaces().subscribe({
      next: (places) => {
        this.places = places;
        this.isAdmin = this.session.isAdmin();
      },
      error: () => this.snackBar.open("Failed to get places", 'Close', {
        duration: 3000,
        panelClass: ['snackbar-color']
      }),
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

  public getAllPlaces(): Observable<Place[]> {
      return this.placesApi.getAllPlaces();
  }

  public getAllSurfaces(): Observable<Surface[]> {
    return this.surfacesApi.getAllSurfaces();
  }

  public getSurfaceType(surfaceId: number | undefined): string | undefined {
    var surface = this.surfaces.find(x=>x.id==surfaceId);
    return surface?.type;
  }

  public getImageUrl(place: Place): string {
    return place?.photoUrl;
  }

  public openAddNewPlaceDialog() {
    const dialogRef = this.dialog.open(AddNewPlaceDialogComponent, {
      height: '450px',
      width: '600px',
    });

    dialogRef.afterClosed().subscribe(() => {
      this.getAllPlaces().subscribe(places => {
        this.places = places;
      });
    });
  }

  public editPlace(place: Place) {
    const dialogRef = this.dialog.open(EditPlaceDialogComponent, {
      height: '400px',
      width: '600px',
    });

    dialogRef.afterClosed().subscribe(() => {
      this.getAllPlaces().subscribe(places => {
        this.places = places;
      });
    });
  }

  public deletePlace(place: Place) {
    this.showSpinner = true;
    this.placesApi.deletePlaceById(place.id).subscribe({
      next: () => {
        this.snackBar.open(`${place.name} place was successfully deleted`, 'Close', {
          duration: 3000,
          panelClass: ['snackbar-color']
        });
      },
      error: () => {
        this.snackBar.open(`Failed to delete ${place.name} place`, 'Close', {
          duration: 3000,
          panelClass: ['snackbar-color']
        });
        this.showSpinner = false;
      },
      complete: () => {
        this.getAllPlaces().subscribe(places => {
          this.places = places;
          this.showSpinner = false;
        });
      }
    });
  }

  public getPlacesCount() {
    return this.places.length;
  }
}
