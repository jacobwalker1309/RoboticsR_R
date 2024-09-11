import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SearchComponent } from '../search/search.component';

import { CommonModule } from '@angular/common';
import { ToggleSwitchComponent } from '../toggle-switch/toggle-switch.component';
import { AddContainerComponent } from '..//add-container/add-container.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  standalone: true,
  imports: [
    FormsModule,
    SearchComponent,
    AddContainerComponent,
    CommonModule,
    ToggleSwitchComponent]
})
export class HomeComponent {
  isAddMode: boolean = true;  // Initialize the isAddMode to true to start in add mode

  toggleAddMode(isSingle: boolean) {
    this.isAddMode = isSingle;
  }
}
