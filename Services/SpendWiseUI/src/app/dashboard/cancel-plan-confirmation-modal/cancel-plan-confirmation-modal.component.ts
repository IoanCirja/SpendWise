import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-confirm-cancel-dialog',
  templateUrl: './cancel-plan-confirmation-modal.component.html',
  styleUrls: ['./cancel-plan-confirmation-modal.component.scss']
})
export class ConfirmCancelDialogComponent {
  constructor(public dialogRef: MatDialogRef<ConfirmCancelDialogComponent>) {}

  onConfirm(): void {
    this.dialogRef.close(true);
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
