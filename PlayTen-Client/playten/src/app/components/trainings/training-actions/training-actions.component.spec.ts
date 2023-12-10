import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingActionsComponent } from './training-actions.component';

describe('TrainingActionsComponent', () => {
  let component: TrainingActionsComponent;
  let fixture: ComponentFixture<TrainingActionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TrainingActionsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrainingActionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
