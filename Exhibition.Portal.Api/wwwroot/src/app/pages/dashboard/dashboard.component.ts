import { Component, OnInit, Injectable } from '@angular/core';
import { ManagementService } from 'src/app/services/ManagementService';
import { serializePath } from '@angular/router/src/url_tree';
@Component({
    selector: 'app-dashboard-page',
    templateUrl: './dashboard.component.html',
    styleUrls: [],  
    providers: []
})
@Injectable()
export class DashboardComponent implements OnInit{
    
    constructor(private service:ManagementService) { }
    ngOnInit(): void {
        const that = this;
     
    }
    
}