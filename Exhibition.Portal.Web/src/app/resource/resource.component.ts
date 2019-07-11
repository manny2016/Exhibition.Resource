import { Component, OnInit, Pipe } from '@angular/core';
import { HttpClient, HttpResponse, HttpRequest, HttpEvent } from '@angular/common/http';
import { ManagementService } from '../services/ManagementService'

import { Subscription } from 'rxjs'



import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { environment } from 'environments/environment';

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
  public resources:any;
  public parent: string;
  public edittingContext: string;
  public workspace: string;
  accept = '*' 
  afuConfig: any = null;
  constructor(public HttpClient: HttpClient, public service: ManagementService, private modalService: NgbModal) { }
  ngOnInit() {
    const that = this;
    that.service.GetResources({ current: that.workspace, search: null }).toPromise().then(res => {
      var source: any = res;
      that.resources = source.data;
      that.parent = source.parent;       
    })

    this.afuConfig = {
      multiple: false,
      formatsAllowed: ".jpg,.png,.jpeg,.bmp,.mp4,.avi,.mpeg,.link,.serial,.txt",
      maxSize: "1024",
      uploadAPI: {
        url: environment.api + "UploadFiles?workspace=",
        headers: {

        }
      },
      theme: "dragNDrop",
      hideProgressBar: false,
      hideResetBtn: true,
      hideSelectBtn: true,
      replaceTexts: {
        selectFileBtn: '浏览',
        resetBtn: '重置',
        uploadBtn: '开始上传',
        dragNDropBox: '将要上传的文件拖拽到这里',
        attachPinBtn: 'Attach Files...',
        afterUploadMsg_success: '上传成功!',
        afterUploadMsg_error: '上传失败!'
      }
    };

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

  public ClickResource(model: any) {
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
  public StartEditing(current: any) {
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
  public OnInputBoxKeyPress(event: any, resource: any) {
    const that = this;
    if (event.key == "Enter" && event.target.value != "") {
      console.log(resource.workspace);
      that.service.Rename({
        workspace: that.workspace,
        name: resource.name,
        newName: event.target.value
      }).toPromise().then(res => {
      
        resource.name = res.data.name;
        resource.editable = false
        resource.fullName = res.data.fullName;
        resource.workspace=res.data.workspace;
        console.log("rename",resource.fullName,res.data.fullName);
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
  /**
   * 
   * @param resource 
   */
  public Delete(resource: any) {
    const that = this;
    if (confirm("真的要删除吗?")) {
      that.service.DeleteResource({ workspace: that.workspace, name: resource.name, newName: "" })
        .toPromise().then(res => {
          that.service.GetResources({ current: that.workspace, search: null }).toPromise().then(res => {
            var source: any = res;
            that.resources = source.data;
            that.parent = source.parent;
            console.log(res);
          })
        });
    }
  }
  public OpenModal(content) {
    const that = this;
    if (that.workspace == "undefined") {
      that.workspace = null;
    }
    that.afuConfig.uploadAPI.url = environment.api + "UploadFiles?workspace=" + that.workspace;
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {      
    }, (reason) => {
    });
  }
  public DocUpload(event: any) {
    const that = this;
    that.service.GetResources({ current: that.workspace, search: null }).toPromise().then(res => {
      var source: any = res;
      that.resources = source.data;
      that.parent = source.parent;
      console.log("ref");
    }).catch(error => {
      console.log("fail", error);
    });
  }
}
