import { Routes } from '@angular/router';

import { DashboardComponent } from '../../dashboard/dashboard.component';
import { UserProfileComponent } from '../../user-profile/user-profile.component';
import { TableListComponent } from '../../table-list/table-list.component';
import { TypographyComponent } from '../../typography/typography.component';
import { IconsComponent } from '../../icons/icons.component';
import { MapsComponent } from '../../maps/maps.component';
import { NotificationsComponent } from '../../notifications/notifications.component';
import { UpgradeComponent } from '../../upgrade/upgrade.component';
import { ResourceComponent } from '../../resource/resource.component';
import { TerminalComponent } from 'app/terminal/terminal.component';
import { DirectiveComponent } from 'app/directive/directive.component';

export const AdminLayoutRoutes: Routes = [  
    // { path: 'dashboard',      component: DashboardComponent },
    { path: 'resource',       component: ResourceComponent },
    { path: 'terminal',       component: TerminalComponent },
    { path: 'directive',      component: DirectiveComponent },
    { path: 'typography',     component: TypographyComponent },
    { path: 'icons',          component: IconsComponent },
    { path: 'maps',           component: MapsComponent },
    { path: 'notifications',  component: NotificationsComponent },
    { path: 'upgrade',        component: UpgradeComponent },
];
