import { Component } from '@angular/core';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-multiple-add-container',
  templateUrl: './multiple-add-container.component.html',
  styleUrls: ['./multiple-add-container.component.scss'],
  standalone: true,
  imports:[
    MatFormField,
    MatLabel,
    MatButtonModule,
    MatInputModule,
    MatCardModule]
})
export class MultipleAddContainerComponent {
  selectedFile: File | null = null;

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      // Handle file reading and processing here
      console.log('File selected:', file.name);
    }
  }

  onMultipleSubmit() {
    if (this.selectedFile) {
      console.log('File uploaded:', this.selectedFile);
      // Add logic to handle multiple entries from file upload
    }
  }

  triggerFileInput() {
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    fileInput.click();
  }
}
