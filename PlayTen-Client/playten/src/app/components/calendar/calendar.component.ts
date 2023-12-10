import { Component, OnInit } from '@angular/core';
import { CalendarView, CalendarEvent } from 'angular-calendar';
import { Subject } from 'rxjs';
import {
  startOfDay,
  subDays,
  addDays,
  endOfMonth,
  addHours,
} from 'date-fns';
import { EventColor } from 'calendar-utils';
import { TrainingApiService } from 'src/app/services/training-api.service';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.less']
})
export class CalendarComponent implements OnInit {
  public events: CalendarEvent[] = [];
  public refresh = new Subject<void>();

  constructor(private readonly trainingsApi: TrainingApiService) { }

  public ngOnInit(): void {
    this.trainingsApi.getAllTrainings().subscribe(trainings => {
      trainings.forEach(training => {
        this.events.push({
          start: subDays(new Date(training.training.dateStart), 1),
          end: subDays(new Date(training.training.dateEnd), 1),
          title: training.training.name,
          color: this.colors['red'],
          allDay: true
        });
      });
      this.refresh.next();
    });
  }
  public viewDate: Date = new Date();
  public view: CalendarView = CalendarView.Month;
  public activeDayIsOpen: boolean = true;

  CalendarView = CalendarView;

  public closeOpenMonthViewDay() {
    this.activeDayIsOpen = false;
  }

  public setView(view: CalendarView) {
    this.view = view;
  }

  public colors: Record<string, EventColor> = {
    red: {
      primary: '#ad2121',
      secondary: '#FAE3E3',
    },
    blue: {
      primary: '#1e90ff',
      secondary: '#D1E8FF',
    },
    yellow: {
      primary: '#e3bc08',
      secondary: '#FDF1BA',
    },
  };

  // events: CalendarEvent[] = [
  //   {
  //     start: subDays(startOfDay(new Date()), 1),
  //     end: addDays(new Date(), 1),
  //     title: 'A 3 day event',
  //     color: { ...this.colors['red'] },
  //     allDay: true,
  //     resizable: {
  //       beforeStart: true,
  //       afterEnd: true,
  //     },
  //     draggable: true,
  //   },
  //   {
  //     start: startOfDay(new Date()),
  //     title: 'An event with no end date',
  //     color: { ...this.colors['yellow'] },
  //   },
  //   {
  //     start: subDays(endOfMonth(new Date()), 3),
  //     end: addDays(endOfMonth(new Date()), 3),
  //     title: 'A long event that spans 2 months',
  //     color: { ...this.colors['blue'] },
  //     allDay: true,
  //   },
  //   {
  //     start: addHours(startOfDay(new Date()), 2),
  //     end: addHours(new Date(), 2),
  //     title: 'A draggable and resizable event',
  //     color: { ...this.colors['yellow'] },
  //     resizable: {
  //       beforeStart: true,
  //       afterEnd: true,
  //     },
  //     draggable: true,
  //   },
  // ];
}
