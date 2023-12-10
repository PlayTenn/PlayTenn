import { Component, OnInit } from '@angular/core';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user';
import { UserApiService } from 'src/app/services/user-api.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.less']
})
export class UsersComponent implements OnInit {
  public panelOpenState: boolean = false;
  public users!: User[];
  public showSpinner: boolean = false;
  public spinnerMode: ProgressSpinnerMode = "indeterminate";
  public isNoUsers: boolean = false;

  constructor(private readonly userApi: UserApiService,
    private readonly snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.showSpinner = true;
    this.getAllUsers().subscribe({
      next: (users) => {
        if(users.length) {
          this.users = users;
        } else {
          this.isNoUsers = true;
        }
      },
      error: () => {
        this.snackBar.open(`Failed to get users`, 'Close', {
          duration: 3000,
          panelClass: ['snackbar-color']
        })
      },
      complete: () => this.showSpinner = false
    });
  }

  public getAllUsers(): Observable<User[]> {
    return this.userApi.getAllUsers();
  }

  public deleteUser(user: User) {
    this.showSpinner = true;
    this.userApi.deleteUserById(user.id).subscribe({
      next: () => {
        this.snackBar.open(`User ${user.name} was successfully deleted`, 'Close', {
          duration: 3000,
          panelClass: ['snackbar-color']
        })
      },
      error: () => {
        this.snackBar.open(`Failed to delete ${user.name}`, 'Close', {
          duration: 3000,
          panelClass: ['snackbar-color']
        })
      },
      complete: () => this.showSpinner = false
    });
  }
}
