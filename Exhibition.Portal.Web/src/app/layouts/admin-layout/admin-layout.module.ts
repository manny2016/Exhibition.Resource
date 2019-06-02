import { NgModule, Pipe } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule ,ReactiveFormsModule} from '@angular/forms';
import { AdminLayoutRoutes } from './admin-layout.routing';
import { DashboardComponent } from '../../dashboard/dashboard.component';
import { UserProfileComponent } from '../../user-profile/user-profile.component';
import { TableListComponent } from '../../table-list/table-list.component';
import { TypographyComponent } from '../../typography/typography.component';
import { IconsComponent } from '../../icons/icons.component';
import { MapsComponent } from '../../maps/maps.component';
import { NotificationsComponent } from '../../notifications/notifications.component';
import { UpgradeComponent } from '../../upgrade/upgrade.component';
import { NgbModule, NgbModalModule, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { DataTablesModule } from 'angular-datatables';
import { AngularFileUploaderModule } from "angular-file-uploader";
import { TerminalComponent } from "app/terminal/terminal.component"
import { TerminalContent } from "app/components/terminaledit/TerminalContent.compnent"


import {
  MatButtonModule,
  MatInputModule,
  MatRippleModule,
  MatFormFieldModule,
  MatTooltipModule,
  MatSelectModule
} from '@angular/material';
import { ResourceComponent } from 'app/resource/resource.component';
import { from } from 'rxjs';
import { Terminal } from 'app/models/terminal';
import { WindowContent } from 'app/components/terminaledit/window.component';
import { DirectiveComponent } from 'app/directive/directive.component';
import { DirectiveContent } from 'app/components/directive/directive.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(AdminLayoutRoutes),
    FormsModule,
    MatButtonModule,
    MatRippleModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatTooltipModule,
    DataTablesModule,
    NgbModalModule, AngularFileUploaderModule, NgxMatSelectSearchModule

  ],
  declarations: [
    DashboardComponent,
    UserProfileComponent,
    TableListComponent,
    TypographyComponent,
    IconsComponent,
    MapsComponent,
    NotificationsComponent,
    UpgradeComponent,
    ResourceComponent,
    TerminalComponent, TerminalContent, WindowContent, DirectiveComponent, DirectiveContent
  ],
  entryComponents: [TerminalContent, WindowContent, DirectiveContent]
})

export class AdminLayoutModule { }
