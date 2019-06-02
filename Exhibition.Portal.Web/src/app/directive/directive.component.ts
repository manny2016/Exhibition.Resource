import { Component, OnInit } from '@angular/core';
import { ManagementService } from 'app/services/ManagementService';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Directive } from 'app/models/Directive';
import { DirectiveContent } from 'app/components/directive/directive.component';
import { Terminal } from 'app/models/terminal';
@Component({
    selector: 'app-directive-component',
    templateUrl: './directive.component.html',
    styleUrls: ['./directive.component.css']
})
export class DirectiveComponent {
    constructor(public service: ManagementService, private modalService: NgbModal) { }
    public directives: Directive[];
    public terminals: Terminal[];
    ngOnInit(): void {
        const that = this;
        this.service.QueryTerminals({ search: "" }).toPromise().then(res => {
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
    }
    public Refresh() {
        const that = this;
        that.service.QueryDirectives({ search: null }).toPromise().then(res => {
            that.directives = res.data;
        });
    }
}