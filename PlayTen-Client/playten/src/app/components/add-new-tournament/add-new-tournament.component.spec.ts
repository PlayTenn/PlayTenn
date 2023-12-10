import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddNewTournamentComponent } from './add-new-tournament.component';

describe('AddNewTournamentComponent', () => {
  let component: AddNewTournamentComponent;
  let fixture: ComponentFixture<AddNewTournamentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddNewTournamentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddNewTournamentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
