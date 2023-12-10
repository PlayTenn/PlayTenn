import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MAT_BOTTOM_SHEET_DATA, MatBottomSheet } from '@angular/material/bottom-sheet';
import { Training } from 'src/app/models/training';
import { TrainingApiService } from 'src/app/services/training-api.service';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { UserApiService } from 'src/app/services/user-api.service';
import { SessionService } from 'src/app/services/session.service';

@Component({
  selector: 'app-training-actions',
  templateUrl: './training-actions.component.html',
  styleUrls: ['./training-actions.component.less']
})
export class TrainingActionsComponent implements OnInit {
  private trainingId: number = 0;
  public type: string = "";
  public training: Training = new Training;
  public dataSource: any;
  public displayedColumns: string[] = ['fullName', 'level', 'status', 'changeStatus'];
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private readonly trainingApi: TrainingApiService,
    private readonly sessionService: SessionService,
    private readonly bottomSheet: MatBottomSheet,
    @Inject(MAT_BOTTOM_SHEET_DATA) public data: { id: number, type: string }) {
    this.trainingId = data.id;
    this.type = data.type;
  }

  public ngOnInit() {
    this.trainingApi.getTrainingById(this.trainingId).subscribe(training => {
      if (training) {
        this.dataSource = training.training.participants;
        this.training = training;
        this.dataSource.paginator = this.paginator;
      }

    })
  }

  public subscribeOnTraining() {
    this.trainingApi.subscribeOnTraining(this.trainingId).subscribe(() => {

    })
  }

  public onCloseClick() {
    this.bottomSheet.dismiss();
  }
}
