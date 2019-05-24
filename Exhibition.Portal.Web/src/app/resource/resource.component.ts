import { Component, OnInit, Pipe } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import{ManagementService} from '../services/ManagementService'
import { Resource } from 'app/models/Resource';
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
  constructor(private service:ManagementService) { }  
  ngOnInit() {
    const that = this;
    that.dtOptions = {
        pagingType: 'simple',
        pageLength: 2,
        serverSide: true,
        processing: true,
        ajax: (dataTablesParameters: any, callback) => {
            console.log("dataTablesParameters",dataTablesParameters);
            that.service.GetResources().toPromise().then(data=>{
                that.resources=data;
            })
            .catch(error=>{

            })
        //   that.http
        //     .post<DataTablesResponse>(
        //       'https://angular-datatables-demo-server.herokuapp.com/',
        //       dataTablesParameters, {}
        //     ).subscribe(resp => {
        //       that.persons = resp.data;
  
        //       callback({
        //         recordsTotal: resp.recordsTotal,
        //         recordsFiltered: resp.recordsFiltered,
        //         data: []
        //       });
        //     });
        },
        columns: [{ data: 'id' }, { data: 'type' }, { data: 'name' },{data:'fullName'}]
      };
  }

}
