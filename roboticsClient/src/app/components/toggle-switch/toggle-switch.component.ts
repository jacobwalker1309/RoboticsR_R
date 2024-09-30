import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonToggleModule } from '@angular/material/button-toggle';

@Component({
  selector: 'app-toggle-switch',
  templateUrl: './toggle-switch.component.html',
  styleUrls: ['./toggle-switch.component.css'],
  standalone: true,
  imports:[MatButtonToggleModule, FormsModule,CommonModule]
})
export class ToggleSwitchComponent {
  @Input() leftLabel: string = '';
  @Input() rightLabel: string = '';
  @Input() leftValue: any = true;
  @Input() rightValue: any = false;
  @Input() value: any;
  @Output() toggle: EventEmitter<any> = new EventEmitter<any>();

  isLeftActive: boolean = this.leftValue; // Initialize isLeftActive

  onToggleChange(event: any) {
    this.isLeftActive = event.value === this.leftValue;
    this.toggle.emit(event.value);
  }
}
