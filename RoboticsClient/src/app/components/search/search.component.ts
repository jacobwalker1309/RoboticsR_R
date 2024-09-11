import { ContainerEntry } from './../../models/ContainerEntry';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCard } from '@angular/material/card';
import { MatCell, MatHeaderCell, MatHeaderRow, MatRow, MatTable } from '@angular/material/table';
import { SearchResultsComponent } from '../search-results/search-results.component';
import { ContainerEntryService } from '../../services/containerEntryService';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatCard,
    MatTable,
    MatCell,
    MatHeaderCell,
    MatHeaderRow,
    MatRow,
    SearchResultsComponent,
  ],
  providers:[]
})
export class SearchComponent {
  searchQuery: number | null = null;
  searchResults: ContainerEntry[] = [];

  displayedColumns: string[] = ['id', 'longitude', 'latitude', 'containerID', 'dateInserted'];

  constructor(private containerEntryService: ContainerEntryService) {}

  onSearch() {
    if (this.searchQuery !== null) {
      this.fetchEntryById(this.searchQuery);
    }
    else
    {
      this.fetchAllEntries();
    }

    console.log("SearchResults: " + this.searchResults);
  }

  fetchAllEntries() {
    // Fetch and log all container entries from the API using observer pattern
    this.containerEntryService.getEntries().pipe(
      catchError(this.handleError.bind(this, 'Error fetching all container entries')) // Catch and handle errors
    ).subscribe({
      next: (data: ContainerEntry[]) => {
        console.log('All container entries:', data);
        this.searchResults = data; // Update searchResults with fetched data
      },
      error: (error) => {
        this.handleError('Error fetching all container entries', error);
      },
      complete: () => {
        console.log('Fetching all container entries completed.');
      }
    });
  }

  fetchEntryById(id: number) {
    console.log(id);
    // Fetch and log a specific container entry by ID using observer pattern
    this.containerEntryService.getEntryById(id).pipe(
      catchError(this.handleError.bind(this, 'Error fetching container entry by ID')) // Catch and handle errors
    ).subscribe({
      next: (entry: ContainerEntry) => {
        console.log(`Container entry with ID ${id}:`, entry);
        this.searchResults = [entry];
        // You can perform additional operations with the entry if needed
      },
      error: (error) => {
        this.handleError('Error fetching container entry by ID', error);
      },
      complete: () => {
        console.log(`Fetching container entry with ID ${id} completed.`);
      }
    });
  }

  private handleError(errorMessage: string, error: any) {
    console.error(errorMessage, error);
    return throwError(() => new Error(errorMessage));
  }
}
