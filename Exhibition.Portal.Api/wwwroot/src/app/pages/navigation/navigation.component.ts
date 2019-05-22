import { Component, OnInit, Injectable } from '@angular/core';
import { Sidebar, MenuItem } from 'src/app/models/Navigation';
import { ManagementService } from 'src/app/services/ManagementService';
import { serializePath } from '@angular/router/src/url_tree';
@Component({
    selector: 'app-navigation-sidebar',
    templateUrl: './navigation.component.html',
    styleUrls: [],  
    providers: []
})
@Injectable()
export class NavigationComponent implements OnInit{
    public Navigation:Sidebar[] = [];
    constructor(private service:ManagementService) { }
    ngOnInit(): void {
        const that = this;
        that.service.GetSidebars().subscribe(source=>{
            that.Navigation = source;
            console.log(that.Navigation);
        });
    }
    collapse(sidebar:Sidebar){
        sidebar.isExpanded=!sidebar.isExpanded;
    }
}