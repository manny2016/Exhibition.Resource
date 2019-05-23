import { Component, OnInit, Injectable } from '@angular/core';
import { ActivatedRoute,Router } from '../../../../node_modules/@angular/router';
import { switchMap } from "rxjs/operators" // RxJS v6
@Component({
    selector: 'app-footer-component',
    templateUrl: './footer.component.html',    
    providers: []
  })
  @Injectable()
  export class FooterComponent implements OnInit{
    private ip:string;
    constructor(private route:ActivatedRoute ,private router:Router){     
    }
    ngOnInit():void{
      const that= this;
      this.route.paramMap.subscribe(params => {
        // that.ip = params.get("ip").split(':')[1];
        // console.log(that.ip);
      })
    }
  }