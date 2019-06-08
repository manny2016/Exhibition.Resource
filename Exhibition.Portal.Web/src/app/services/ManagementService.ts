import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment.prod'
// import { Sidebar } from '../models/Sidebar';
// import { Resource } from 'app/models/Resource'; 
// import { QueryFilter, SQLiteFilter } from 'app/models/queryFilter';
// import { ResourceRequestContext } from "app/models/ResourceRequestContext";
// import { ActionResponse } from 'app/models/QueryResponse';
// import { getLocaleDateTimeFormat } from '@angular/common';
// import { Terminal, Window } from 'app/models/terminal';
// import { Directive, OptionModel } from 'app/models/Directive';
import { text } from '../../../node_modules/@angular/core/src/render3';



@Injectable()

export class ManagementService {
    constructor(private https: HttpClient) { }
    /**
     * query resource
     */
    public GetResources(filter: any): Observable<any[]> {
        const url = environment.api + "GetFileSystem?datetime=" + new Date();
        return this.https.post<any[]>(url, filter);
    }
    public QueryFileSystem(filter: any): Observable<any[]> {
        const url = environment.api + "QueryFileSystem?datetime=" + new Date();
        return this.https.post<any[]>(url, filter);
    }
    /**
     * rename folder or file
     * @param context 
     */
    public Rename(context: any): Observable<any> {
        const url = environment.api + "Rename";
        return this.https.post<any>(url, context);
    }
    /**
     * create new folder
     * @param context 
     */
    public CreateDirectory(context: any): Observable<any> {
        const url = environment.api + "CreateDirectory";
        return this.https.post<any>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public DeleteResource(context: any): Observable<any> {
        const url = environment.api + "DeleteResource";
        return this.https.post<any>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public QueryTerminals(context: any): Observable<any> {
        const url = environment.api + "QueryTerminals";
        return this.https.post<any>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public CreateOrUpdateSerialPortTerminal(context: any): Observable<any> {
        const url = environment.api + "CreateOrUpdateSerialPortTerminal";
        return this.https.post<any>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public CreateOrUpdateMediaPlayerTerminal(context:any):Observable<any>{
        const url = environment.api + "CreateOrUpdateMediaPlayerTerminal";
        return this.https.post<any>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public DeleteTerminal(context: any): Observable<any> {
        const url = environment.api + "DeleteTerminal";
        return this.https.post<any>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public CreateOrUpdateDirective(context: any): Observable<any> {
        const url = environment.api + "CreateOrUpdateDirective";
        return this.https.post<any>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public QueryDirectives(context: any): Observable<any> {
        const url = environment.api + "QueryDirectives";
        return this.https.post<any>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public DeleteDirective(context: any): Observable<any> {
        const url = environment.api + "DeleteDirective";
        return this.https.post<any>(url, context);
    }

    /**
     * 
     * @param filter 
     */
    public QueryResourceforChoosing(filter: any): Observable<any> {
        const url = environment.api + "QueryFileSystemforChoosing";
        return this.https.post<any>(url, filter);
    }
    /**
     * 
     * @param filter 
     */
    public QueryTerminalforChoosing(filter:any):Observable<any>{
        const url = environment.api+"QueryTerminalforChoosing";
        return this.https.post<any>(url, filter);
    }
    /**
     * 
     * @param type 
     */
    public GetTerminalTypeName(type: number) {
        if (type == -1) { return "不支持的设备"; }
        if (type == 1) { return "媒体播放机"; }
        if (type == 2) { return "PLC中控设备"; }
    }
}