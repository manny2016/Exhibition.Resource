import { Component, OnInit, Optional, ChangeDetectorRef } from '@angular/core';
import { ManagementService } from 'app/services/ManagementService';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { DirectiveContent } from 'app/components/directive/directive.component';

import { config } from '../../../node_modules/rxjs';
@Component({
    selector: 'app-directive-component',
    templateUrl: './directive.component.html',
    styleUrls: ['./directive.component.css']
})
export class DirectiveComponent {
    
    constructor(public service: ManagementService, private modalService: NgbModal,private cdr: ChangeDetectorRef) { }
    public directives: any[];
    public terminals: any[];
    
    ngOnInit(): void {
        const that = this;
        this.service.QueryTerminals({search:""}).toPromise().then(res=>{
            that.terminals = res.data;
        });
        this.Refresh();
    }
    public OpenEdit(content) {                
        const that = this;
        const modalRef = that.modalService.open(DirectiveContent,{ size: 'lg' });
        modalRef.componentInstance.current = content;
        modalRef.componentInstance.terminals = that.terminals;
        modalRef.componentInstance.modalRef = modalRef;
        
        modalRef.result.then(ctx => {
            that.Refresh();
        });
        modalRef.dismiss = (reson => { });
    }
    public Execute(directive){
       
        this.service.Execute(directive).toPromise().then(res=>{
            console.log(res);
        });
    }
    public Refresh() {
        const that = this;
        that.service.QueryDirectives({ search: null }).toPromise().then(res => {
            that.directives = res.data;
        });
    }
    public DeleteDirective(directive:any){
        const that = this;
        if(confirm("真的要指令"+directive.name +"删除吗?")){
            that.service.DeleteDirective(directive).toPromise().then(res=>{
                if(res.success){
                    that.Refresh();
                }
            });
        }
    }
    ngAfterViewInit() {
        this.cdr.detectChanges();
        
      }
    public JointResourceName(resources:any){
        var text :string="";
        resources.forEach(element => {
            text +=element.name +";";
        });
        return text;
    }
    
}