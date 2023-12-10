import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddNewTrainingDialogComponent } from './add-new-training-dialog.component';

describe('AddNewTrainingDialogComponent', () => {
  let component: AddNewTrainingDialogComponent;
  let fixture: ComponentFixture<AddNewTrainingDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddNewTrainingDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddNewTrainingDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
