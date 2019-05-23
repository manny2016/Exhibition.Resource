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
    constructor(private route:ActivatedRoute ,private router:Router){     
    }
    ngOnInit():void{
      const that = this;
      this.route.paramMap.subscribe(params => {
        console.log(params.get("type"));
      })
    }
  }