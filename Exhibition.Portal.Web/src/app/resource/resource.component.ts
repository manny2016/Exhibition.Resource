import { Component, OnInit, Pipe } from '@angular/core';
import { HttpClient, HttpResponse, HttpRequest, HttpEvent } from '@angular/common/http';
import { ManagementService } from '../services/ManagementService'
import { Resource } from 'app/models/Resource';
import { Subscription } from 'rxjs'
import { QueryFilter } from 'app/models/queryFilter';
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
    that.service.GetResources({ current: null, search: null }).toPromise().then(res => {
      var source: any = res;
      that.resources = source.data;
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
  public onClickResource(model: Resource) {
    const that = this;
    that.service.GetResources({current:model.workspace,search:""}).toPromise().then(res => {
      var source: any = res;
      that.resources = source.data;
    });
  }
}
