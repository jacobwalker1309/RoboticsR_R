import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../services/apiService';
import { ContainerEntry } from '../models/ContainerEntry';
import { ContainerEntryRequest } from '../models/dtos/containerEntryRequest';

@Injectable({
  providedIn: 'root',
})
export class ContainerEntryService {
  private endpoint = 'ContainerEntries'; // Endpoint specific to ContainerEntries

  constructor(private apiService: ApiService) {}

  // Get all entries
  getEntries(): Observable<ContainerEntry[]> {
    return this.apiService.get<ContainerEntry[]>(this.endpoint);
  }

  // Get a single entry by ID
  getEntryById(id: number): Observable<ContainerEntry> {
    return this.apiService.get<ContainerEntry>(`${this.endpoint}/${id}`);
  }

  // Create a new entry
  addEntry(entry: ContainerEntryRequest): Observable<ContainerEntryRequest> {
    return this.apiService.post<ContainerEntryRequest>(this.endpoint, entry);
  }

  // Update an entry
  updateEntry(id: number, entry: ContainerEntry): Observable<ContainerEntry> {
    return this.apiService.put<ContainerEntry>(`${this.endpoint}/${id}`, entry);
  }

  // Delete an entry
  deleteEntry(id: number): Observable<void> {
    return this.apiService.delete<void>(`${this.endpoint}/${id}`);
  }
}
