import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Sidebar } from '../models/Navigation';

@Injectable()
export class ManagementService{
    constructor(private https: HttpClient) {}

    public GetSidebars(): Observable<Sidebar[]> {
        return this.https.get<Sidebar[]>("../data/Sidebar.json");
        
    }
}