import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Sidebar } from '../models/Sidebar';
import { Resource } from 'app/models/Resource';
import { environment } from '../../environments/environment.prod'
import { QueryFilter, SQLiteFilter } from 'app/models/queryFilter';
import { ResourceRequestContext } from "app/models/ResourceRequestContext";
import { ActionResponse } from 'app/models/QueryResponse';
import { getLocaleDateTimeFormat } from '@angular/common';
import { Terminal } from 'app/models/terminal';
import { Directive } from 'app/models/Directive';



@Injectable()

export class ManagementService {
    constructor(private https: HttpClient) { }
    /**
     * query resource
     */
    public GetResources(filter: QueryFilter): Observable<Resource[]> {
        const url = environment.api + "GetFileSystem?datetime=" + new Date();
        return this.https.post<Resource[]>(url, filter);
    }
    public QueryFileSystem(filter:QueryFilter):Observable<Resource[]>{
        const url = environment.api + "QueryFileSystem?datetime=" + new Date();
        return this.https.post<Resource[]>(url,filter);
    }
    /**
     * rename folder or file
     * @param context 
     */
    public Rename(context: ResourceRequestContext): Observable<Resource> {
        const url = environment.api + "Rename";
        return this.https.post<Resource>(url, context);
    }
    /**
     * create new folder
     * @param context 
     */
    public CreateDirectory(context: ResourceRequestContext): Observable<ActionResponse> {
        const url = environment.api + "CreateDirectory";
        return this.https.post<ActionResponse>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public DeleteResource(context: ResourceRequestContext): Observable<ActionResponse> {
        const url = environment.api + "DeleteResource";
        return this.https.post<ActionResponse>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public QueryTerminals(context: SQLiteFilter): Observable<ActionResponse> {
        const url = environment.api + "QueryTerminals";
        return this.https.post<ActionResponse>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public CreateOrUpdateTerminal(context: Terminal): Observable<ActionResponse> {
        const url = environment.api + "CreateTerminal";
        return this.https.post<ActionResponse>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public DeleteTerminal(context: Terminal): Observable<ActionResponse> {
        const url = environment.api + "DeleteTerminal";
        return this.https.post<ActionResponse>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public CreateOrUpdateDirective(context: Directive): Observable<ActionResponse> {
        const url = environment.api + "CreateOrUpdateDirective";
        return this.https.post<ActionResponse>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public QueryDirectives(context: SQLiteFilter): Observable<ActionResponse> {
        const url = environment.api + "QueryDirectives";
        return this.https.post<ActionResponse>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public DeleteDirective(context: Directive): Observable<ActionResponse> {
        const url = environment.api + "DeleteDirective";
        return this.https.post<ActionResponse>(url, context);
    }
}