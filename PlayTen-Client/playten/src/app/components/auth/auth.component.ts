import { Component } from '@angular/core';
import { FormControl, FormGroup, ValidatorFn, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { SignIn } from 'src/app/models/signIn';
import { SignUp } from 'src/app/models/signUp';
import { TennisLevel } from 'src/app/models/tennisLevel';
import { AuthApiService } from 'src/app/services/auth-api.service';
import { SessionService } from 'src/app/services/session.service';
import { TennisLevelApiService } from 'src/app/services/tennisLevel-api.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.less']
})
export class AuthComponent {
  public tennisLevels!: TennisLevel[];
  public hidePassword: boolean = true;
  public spinnerMode: ProgressSpinnerMode = "indeterminate";
  public showSignUpForm: boolean = false;
  public showSignInForm: boolean = true;
  public showSpinner: boolean = false;
  public isSignInButtonDisabled: boolean = true;
  public isSignUpButtonDisabled: boolean = true;
  public signIn: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('',
      [Validators.required, Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*.]).{8,}$/)])
  });

  public signUp: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    name: new FormControl('', [Validators.required]),
    surname: new FormControl('', [Validators.required]),
    tennisLevel: new FormControl('', [Validators.required]),
    howToConnect: new FormControl('', Validators.required),
    password: new FormControl('',
      [Validators.required, Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*.]).{8,}$/)]),
    confirmPassword: new FormControl('',
      [Validators.required, Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*.]).{8,}$/), this.passwordMatchValidator()])
  });


  constructor(private readonly authApi: AuthApiService,
    private readonly snackBar: MatSnackBar,
    private readonly router: Router,
    private readonly session: SessionService,
    private readonly tennisLevelApi: TennisLevelApiService) {
    this.signIn.valueChanges.subscribe(() => {
      this.isSignInButtonDisabled = !this.signIn.valid;
    });
    this.signUp.valueChanges.subscribe(() => {
      this.isSignUpButtonDisabled = !this.signUp.valid;
    });
  }

  public onClickHandle(button: string) {
    switch (button) {
      case "signin":
        this.showSignInForm = true;
        this.showSignUpForm = false;
        break;
      case "signup":
        this.showSignUpForm = false;
        this.showSignInForm = false;
        this.showSpinner = true;
        this.tennisLevelApi.getAllTennisLevels().subscribe({
          next: tennisLevels => this.tennisLevels = tennisLevels,
          error: () => {},
          complete: () => {
            this.showSpinner = false;
            this.showSignUpForm = true;
          }
        });
        break;
    }
  }

  public signInOnClick() {
    this.showSpinner = true;
    this.showSignInForm = false;
    var email = this.signIn.controls['email'].value;
    var password = this.signIn.controls['password'].value;
    var signInData: SignIn = {
      email: email,
      password: password
    };
    this.authApi.signin(signInData).subscribe(response => {
      this.session.setSession(response);
    }, error => {
      this.snackBar.open("Something went wrong", 'Close', {
        duration: 3000,
        panelClass: ['snackbar-color']
      });
      this.showSpinner = false;
      this.showSignInForm = true;
    }, () => {
      this.showSpinner = false;
      this.router.navigateByUrl("/home");
    });
  }

  public signUpOnClick() {
    this.showSpinner = true;
    this.showSignUpForm = false;
    var email = this.signUp.controls['email'].value;
    var name = this.signUp.controls['name'].value;
    var surname = this.signUp.controls['surname'].value;
    var tennisLevel = this.signUp.controls['tennisLevel'].value;
    var howToConnect = this.signUp.controls['howToConnect'].value;
    var confirmPassword = this.signUp.controls['confirmPassword'].value;
    var password = this.signUp.controls['password'].value;
    var signUpData: SignUp = {
      email: email,
      name: name,
      surname: surname,
      tennisLevelId: tennisLevel,
      howToConnect: howToConnect,
      password: password,
      confirmPassword: confirmPassword
    };
    this.authApi.signup(signUpData).subscribe(response => {
      this.session.setSession(response);
      this.showSignUpForm = false;
    }, error => {
      this.snackBar.open("Something went wrong", 'Close', {
        duration: 3000
      });
      this.showSpinner = false;
      this.showSignUpForm = true;
    }, () => {
      this.router.navigateByUrl("/home");
      this.showSpinner = false;
    });
  }

  private passwordMatchValidator(): ValidatorFn {
    return (control:AbstractControl) : ValidationErrors | null => {
      if(this.signUp) {
        return this.signUp.controls['password'].value === this.signUp.controls['confirmPassword'].value
        ? null : { 'mismatch': true };
      }

      return null;
    }

  }
}
