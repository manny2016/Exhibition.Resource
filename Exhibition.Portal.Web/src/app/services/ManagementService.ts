import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Sidebar } from '../models/Sidebar';
import { Resource } from 'app/models/Resource';
import { environment } from '../../environments/environment.prod'
import { QueryFilter } from 'app/models/queryFilter';
import { ResourceRequestContext } from 'app/models/ResourceRequestContext';
import { ResourceActionResponse } from 'app/models/ResourceActionResponse';


@Injectable()

export class ManagementService {
    constructor(private https: HttpClient) { }
    /**
     * query resource
     */
    public GetResources(filter: QueryFilter): Observable<Resource[]> {
        const url = environment.api + "GetFileSystem";
        return this.https.post<Resource[]>(url, filter);
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
    public CreateDirectory(context: ResourceRequestContext): Observable<ResourceActionResponse> {
        const url = environment.api + "CreateDirectory";
        return this.https.post<ResourceActionResponse>(url, context);
    }
    /**
     * 
     * @param context 
     */
    public Delete(context: ResourceRequestContext): Observable<ResourceActionResponse> {
        const url = environment.api + "DeleteResource";
        return this.https.post<ResourceActionResponse>(url, context);
    }
}