import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { ContainerEntry } from '../../models/ContainerEntry';

@Component({
  selector: 'app-search-results',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatCardModule],
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent {
  @Input() searchResults: ContainerEntry[] = [];
  displayedColumns: string[] = ['id', 'longitude', 'latitude', 'containerID', 'dateInserted'];
}
