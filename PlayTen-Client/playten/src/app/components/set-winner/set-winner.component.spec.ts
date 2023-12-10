import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetWinnerComponent } from './set-winner.component';

describe('SetWinnerComponent', () => {
  let component: SetWinnerComponent;
  let fixture: ComponentFixture<SetWinnerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SetWinnerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SetWinnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
