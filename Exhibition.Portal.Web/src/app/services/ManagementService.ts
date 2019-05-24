import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Sidebar } from '../models/Sidebar';
import { Resource } from 'app/models/Resource';

@Injectable()
export class ManagementService{
    constructor(private https: HttpClient) {}
    public GetResources(): Observable<Resource[]> {
        return this.https.get<Resource[]>("../data/resource.json");        
    } 
}