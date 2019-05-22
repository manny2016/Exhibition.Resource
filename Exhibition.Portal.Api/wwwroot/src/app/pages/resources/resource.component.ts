import { Component, OnInit, Injectable } from '@angular/core';
import { ActivatedRoute, Router} from '@angular/router';

@Component({
    selector: 'app-resource-page',
    templateUrl: './resource.component.html',    
    providers: []
  })
  @Injectable()
  export class ResourceComponent implements OnInit{
    private type:string;
    constructor(){     
    }
    ngOnInit():void{
      const that = this;
      //console.log(this.url.queryParams);
    }
  }