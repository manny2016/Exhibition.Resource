import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Sidebar } from '../models/Sidebar';
import { Resource } from 'app/models/Resource';
import { environment } from '../../environments/environment.prod'
import { QueryFilter } from 'app/models/queryFilter';
@Injectable()
export class ManagementService {
    constructor(private https: HttpClient) { }
    public GetResources(filter:QueryFilter): Observable<Resource[]> {
        const url = environment.api + "GetFileSystem";
        return this.https.post<Resource[]>(url,filter);
    }
}