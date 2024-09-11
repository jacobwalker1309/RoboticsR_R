import { Component } from '@angular/core';
import { SingleAddContainerComponent } from './single-add-container/single-add-container.component';
import { MultipleAddContainerComponent } from './multiple-add-container/multiple-add-container.component';
import { CommonModule } from '@angular/common';
import { ToggleSwitchComponent } from '../../components/toggle-switch/toggle-switch.component';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-add-container',
  templateUrl: './add-container.component.html',
  styleUrls: ['./add-container.component.scss'],
  standalone: true,
  imports: [
    SingleAddContainerComponent,
    MultipleAddContainerComponent,
    CommonModule,
    ToggleSwitchComponent,
    MatCardModule]
})
export class AddContainerComponent {
  isAddSingleMode: boolean = true;

  toggleAddMode(isSingle: boolean) {
    this.isAddSingleMode = isSingle;
  }
}
