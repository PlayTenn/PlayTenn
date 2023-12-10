import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Training, TrainingInfo } from 'src/app/models/training';
import { SessionService } from 'src/app/services/session.service';
import { TrainingApiService } from 'src/app/services/training-api.service';
import { AddNewTrainingDialogComponent } from '../add-new-training-dialog/add-new-training-dialog.component';
import { SurfacesApiService } from 'src/app/services/surfaces-api.service';
import { Surface } from 'src/app/models/surface';
import { Observable } from 'rxjs';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { TrainingActionsComponent } from './training-actions/training-actions.component';

@Component({
  selector: 'app-trainings',
  templateUrl: './trainings.component.html',
  styleUrls: ['./trainings.component.less']
})
export class TrainingsComponent implements OnInit {
  public isAdmin: boolean = false;
  public panelOpenState: boolean = false;
  public trainings!: Training[];
  public surfaces!: Surface[];
  public showSpinner: boolean = false;
  public trainingsCount: number = 0;
  public spinnerMode: ProgressSpinnerMode = "indeterminate";

  constructor(private readonly trainingApi: TrainingApiService,
    private readonly surfacesApi: SurfacesApiService,
    private readonly snackBar: MatSnackBar,
    public readonly dialog: MatDialog,
    private readonly bottomSheet: MatBottomSheet,
    private readonly session: SessionService) {
  }

  public ngOnInit(): void {
    this.showSpinner = true;
    this.getAllTrainings().subscribe({
      next: trainings => {
        this.trainings = trainings;
        this.trainingsCount = trainings?.length;
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

  public deleteTraining(id: number | undefined) {
    if (id) {
      this.trainingApi.deleteTraining(id).subscribe(() => {
        this.ngOnInit();
      });
    }
  }

  private getAllTrainings() {
    return this.trainingApi.getAllTrainings();
  }

  private getAllSurfaces(): Observable<Surface[]> {
    return this.surfacesApi.getAllSurfaces();
  }

  public getSurfaceType(surfaceId: number | undefined): string | undefined {
    var surface = this.surfaces.find(x => x.id == surfaceId);
    return surface?.type;
  }

  public openAddNewTrainingDialog() {
    const dialogRef = this.dialog.open(AddNewTrainingDialogComponent, {
      height: '700px',
      width: '600px',
    });

    dialogRef.afterClosed().subscribe(() => {
      this.getAllTrainings().subscribe(trainings => {
        this.trainings = trainings;
      });
    });
  }

  public openSubscribeDialog(id: number | undefined): void {
    if (id) {
      this.bottomSheet.open(TrainingActionsComponent, { data: { id: id, type: "subscribe" } });
    }
  }

  public openUnsubscribeDialog(id: number | undefined): void {

  }

  public openDetailsDialog(id: number | undefined): void {
    if (id) {
      this.bottomSheet.open(TrainingActionsComponent, { data: { id: id, type: "details" } });
    }
  }
}
