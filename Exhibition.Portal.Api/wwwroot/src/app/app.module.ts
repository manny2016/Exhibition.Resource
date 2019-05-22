import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {ResourceComponent} from './pages/resources/resource.component';
import {TerminalComponent} from './pages/terminal/terminal.component';
import{ NavigationComponent} from './pages/navigation/navigation.component';
import{ ManagementService } from './services/ManagementService'
import { DashboardComponent } from './pages/dashboard/dashboard.component';

@NgModule({
  declarations: [
    AppComponent,DashboardComponent,ResourceComponent,TerminalComponent,NavigationComponent
  ],
  imports: [
    BrowserModule,    AppRoutingModule,HttpClientModule
  ],
  providers: [ManagementService],
  bootstrap: [AppComponent]
})
export class AppModule { }
