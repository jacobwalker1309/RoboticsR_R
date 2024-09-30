import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { ContainerEntryService } from '../../../services/containerEntryService';
import { ContainerEntryRequest } from '../../../models/dtos/containerEntryRequest'; // Import the DTO

@Component({
  selector: 'app-single-add-container',
  templateUrl: './single-add-container.component.html',
  styleUrls: ['./single-add-container.component.css'],
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule
  ]
})
export class SingleAddContainerComponent {
  newData: ContainerEntryRequest = {
    longitude: null,  // Initialize as undefined
    latitude: null,   // Initialize as undefined
    containerID: null, // Initialize as undefined
    dateInserted: new Date()
  };

  constructor(private containerEntryService: ContainerEntryService) {}

  onSubmit() {
    console.log("should work here")
    // Ensure newData is correctly formatted for the request DTO
    const entry: ContainerEntryRequest = {
      ...this.newData,
      longitude: Number(this.newData.longitude),
      latitude: Number(this.newData.latitude),
      containerID: Number(this.newData.containerID),
      dateInserted: new Date() // Optionally set the date here
    };

    this.containerEntryService.addEntry(entry).subscribe({
      next: (response) => {
        console.log('Container entry added successfully:', response);
        this.resetForm();
      },
      error: (error) => {
        console.error('Error adding container entry:', error);
      }
    });
  }

  private resetForm() {
    this.newData = {
      longitude: 0,
      latitude: 0,
      containerID: 0,
      dateInserted: new Date()
    };
  }

  validateInput(event: Event) {
    const inputElement = event.target as HTMLInputElement;
    let value = inputElement.value;

    // Remove leading zeros
    value = value.replace(/^0+/, '');

    // If the value is '0' or empty, make the input blank
    if (value === '0' || value === '') {
      value = '';
    }

    inputElement.value = value;

    // Update the bound value in newData
    if (inputElement.id === 'containerId') {
      this.newData.containerID = value ? Number(value) : null;
    } else if (inputElement.id === 'longitude') {
      this.newData.longitude = value ? Number(value) : null;
    } else if (inputElement.id === 'latitude') {
      this.newData.latitude = value ? Number(value) : null;
    }
  }
}
