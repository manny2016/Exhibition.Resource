import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ResourceComponent } from './pages/resources/resource.component';
import { TerminalComponent } from './pages/terminal/terminal.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
const routes: Routes = [
  {path:'',redirectTo:'/dashboard',pathMatch:'full'},
  {path:'dashboard',component:DashboardComponent},
  {path:'resource/:type',component:ResourceComponent},
  {path:'terminal/:ip',component:TerminalComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
