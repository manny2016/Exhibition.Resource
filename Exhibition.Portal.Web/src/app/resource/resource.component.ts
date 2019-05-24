import { Component, OnInit, Pipe } from '@angular/core';
import { HttpClient, HttpResponse ,HttpRequest, HttpEvent} from '@angular/common/http';
import{ManagementService} from '../services/ManagementService'
import { Resource } from 'app/models/Resource';
import { Subscription } from 'rxjs'
declare var $: any;
class DataTablesResponse {
  data: any[];
  draw: number;
  recordsFiltered: number;
  recordsTotal: number;
}

@Component({
  selector: 'app-resource-component',
  templateUrl: './resource.component.html',
  styleUrls: ['./resource.component.css']
}) 
export class ResourceComponent implements OnInit {
  public dtOptions: DataTables.Settings = {};
  public resources:Resource[];
  accept = '*'
  files:File[] = []
  progress:number
  url = 'https://evening-anchorage-3159.herokuapp.com/api/'
  hasBaseDropZoneOver:boolean = false
  httpEmitter:Subscription
  httpEvent:HttpEvent<{}>
  lastFileAt:Date

  sendableFormData:FormData//populated via ngfFormData directive

  dragFiles:any
  validComboDrag:any
  lastInvalids:any
  fileDropDisabled:any
  maxSize:any
  baseDropValid:any

  
  constructor(public HttpClient:HttpClient,public service:ManagementService) { }  
  ngOnInit() {
    const that = this;
    that.service.GetResources().toPromise().then(data=>{
      that.resources=data;
  })
  // .catch(error=>{
  // })
  //   that.dtOptions = {
  //       paging:false,        
  //       serverSide: true,
  //       processing: true,
  //       autoWidth:true,
  //       ajax: (dataTablesParameters: any, callback) => {
  //           console.log("dataTablesParameters",dataTablesParameters);
            
  //       },
  //       columns: [ { data: 'name' },{data:'fullName'}]
  //     };
  }
  public getIcoName(type:string){
    console.log(type);
    var ico :string = "description";
    type = type.toLowerCase();
    if(type=="images") return "folder_open";
    if(type=="video")return "movie";
    if(type=="h5") return "open_in_browser";    
    return ico;
  }
}
