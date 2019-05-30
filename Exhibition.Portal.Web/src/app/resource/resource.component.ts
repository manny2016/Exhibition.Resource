import { Component, OnInit, Pipe } from '@angular/core';
import { HttpClient, HttpResponse, HttpRequest, HttpEvent } from '@angular/common/http';
import { ManagementService } from '../services/ManagementService'
import { Resource } from 'app/models/Resource';
import { Subscription } from 'rxjs'
import { QueryFilter } from 'app/models/queryFilter';
import { ResourceRequestContext } from 'app/models/ResourceRequestContext';
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
  public resources: Resource[];
  public parent: string;
  public edittingContext: string;
  public workspace: string;
  accept = '*'
  files: File[] = []
  progress: number
  url = 'https://evening-anchorage-3159.herokuapp.com/api/'
  hasBaseDropZoneOver: boolean = false
  httpEmitter: Subscription
  httpEvent: HttpEvent<{}>
  lastFileAt: Date

  sendableFormData: FormData//populated via ngfFormData directive

  dragFiles: any
  validComboDrag: any
  lastInvalids: any
  fileDropDisabled: any
  maxSize: any
  baseDropValid: any


  constructor(public HttpClient: HttpClient, public service: ManagementService) { }
  ngOnInit() {
    const that = this;
    that.service.GetResources({ current: that.workspace, search: null }).toPromise().then(res => {
      var source: any = res;
      that.resources = source.data;
      that.parent = source.parent;
      console.log(res);
    })
  }
  public getIcoName(type: number) {

    var ico: string = "description";

    if (type == 1) return "folder_open";
    if (type == 2) return "movie";
    if (type == 3) return "image";
    if (type == 4) return "open_in_browser";
    if (type == 5) return "image";
    return ico;
  }
  public ClickResource(model: Resource) {
    const that = this;
    that.service.GetResources({ current: model.workspace, search: "" }).toPromise().then(res => {
      var source: any = res;
      that.resources = source.data;
      that.parent = source.parent;
      that.workspace = source.workspace;      
    });
  }
  public GoUp() {
    const that = this;
    that.service.GetResources({ current: that.parent, search: "" }).toPromise().then(res => {
      var source: any = res;
      that.resources = source.data;
      that.parent = source.parent;
      that.workspace = source.workspace;
      
    });
  }
  public onGoRoot() {
    console.log("ongoroot");
    const that = this;
    that.service.GetResources({ current: null, search: "" }).toPromise().then(res => {
      var source: any = res;
      that.resources = source.data;
      that.parent = source.parent;
      that.workspace = source.workspace;
    });
  }
  /**
   * 
   * @param current 
   */
  public StartEditing(current: Resource) {
    const that = this;
    this.edittingContext = current.name;
    current.editable = !current.editable;
    that.resources.forEach(element => {
      if (element.name != current.name) {
        element.editable = false;
      }
    });

  }
  /**
   * 
   * @param editable 
   */
  public getmatTooltip(editable: boolean) {
    return editable ? "保存" : "编辑";
  }
  /**
   * 
   * @param event 
   * @param resource 
   */
  public OnInputBoxKeyPress(event: any, resource: Resource) {
    const that = this;
    if (event.key == "Enter" && event.target.value != "") {
      console.log(resource.workspace);
      
      that.service.Rename({
        workspace: that.workspace,
        name: resource.name,
        newName: event.target.value
      }).toPromise().then(res => {
        var data: any = res;
        resource.name = data.data.name;
        resource.editable = false
      });
    }
  }
  /**
   * 
   */
  public CreateFolder() {
    const that = this;
    this.service.CreateDirectory({
      workspace: that.workspace, name: "新建文件夹", newName: ""
    }).toPromise().then(res => {
      
      that.service.GetResources({ current: that.workspace, search: null }).toPromise().then(res => {
        var source: any = res;
        that.resources = source.data;
        that.parent = source.parent;
        console.log(res);
      })
    });
  }
  public Delete(resource:Resource){
    const that = this;
    if (confirm("真的要删除吗?")) {
      that.service.Delete({workspace:that.workspace,name:resource.name,newName:""})
      .toPromise().then(res=>{
        that.service.GetResources({ current: that.workspace, search: null }).toPromise().then(res => {
          var source: any = res;
          that.resources = source.data;
          that.parent = source.parent;
          console.log(res);
        })
      });
    }
  }
}
