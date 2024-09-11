import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from '../app/components/login/login.component';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from '../app/components/home/home.component';
import { SearchComponent } from '../app/components/search/search.component';
import { ToggleSwitchComponent } from './components/toggle-switch/toggle-switch.component';
import { SingleAddContainerComponent } from '../app/components/add-container/single-add-container/single-add-container.component';
import { SearchResultsComponent } from '../app/components/search-results/search-results.component';
import { ContainerEntryService } from './services/containerEntryService';
import { ApiService } from './services/apiService';
import { provideHttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    LoginComponent,
    FormsModule,
    HomeComponent,
    SearchComponent,
    ToggleSwitchComponent,
    SingleAddContainerComponent,
    SearchResultsComponent
  ],
  providers:[
    ContainerEntryService,
    ApiService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'roboticsClient';
}
