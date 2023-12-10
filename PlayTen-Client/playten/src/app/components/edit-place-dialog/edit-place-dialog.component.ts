import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-edit-place-dialog',
  templateUrl: './edit-place-dialog.component.html',
  styleUrls: ['./edit-place-dialog.component.less']
})
export class EditPlaceDialogComponent {
  public place: FormGroup = new FormGroup({
    name: new FormControl('', [Validators.required]),
    street: new FormControl('', [Validators.required])
  });

  constructor(public dialogRef: MatDialogRef<EditPlaceDialogComponent>) { }

  public cancelClick() {
    this.dialogRef.close();
  }
}
