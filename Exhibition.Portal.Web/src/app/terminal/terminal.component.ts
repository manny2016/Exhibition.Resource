
import { Component, OnInit, Pipe, Input } from '@angular/core';

import { ManagementService } from 'app/services/ManagementService';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TerminalContent } from 'app/components/terminaledit/TerminalContent.compnent';
import { SerialPortEdit } from 'app/components/serialport-terminal/SerialPortEdit.Component';
//import { WindowContent } from 'app/components/terminaledit/window.component';
import { MediaPlayerEdit } from "app/components/media-terminal/MediaPlayerEdit.Component";
@Component({
    selector: 'app-terminal-component',
    templateUrl: './terminal.component.html',
    styleUrls: ['./terminal.component.css']
})

export class TerminalComponent implements OnInit {
    constructor(public service: ManagementService, private modalService: NgbModal) { }
    public dtOptions: DataTables.Settings = {};
    public terminals: any[];
    public current: any = null;
    /**
     * 
     */
    ngOnInit(): void {
        const that = this;
        this.Refresh();
    }
    /**
     * 
     * @param content 
     */
    public OpenTerminalEdit(content: any) {
        const that = this;

        if (content.type == 1) {

            const modalRef = that.modalService.open(MediaPlayerEdit, { size: 'lg' });
            modalRef.componentInstance.current = content;
            modalRef.componentInstance.modalRef = modalRef;
            modalRef.result.then(ctx => {
                that.Refresh();
            });
            modalRef.dismiss = (reson => { });
        }
        if (content.type == 2) {
            const modalRef = that.modalService.open(SerialPortEdit, { size: 'lg' });
            modalRef.componentInstance.current = content;
            modalRef.componentInstance.modalRef = modalRef;
            modalRef.result.then(ctx => {
                that.Refresh();
            });
            modalRef.dismiss = (reson => { });
        }
    }
    /**
     * 
     */
    public Refresh() {
        const that = this;
        that.service.QueryTerminals({ search: null }).toPromise().then(res => {
            that.terminals = res.data;
        });
    }


    public GetTerminalTypeName(type: number) {
        return this.service.GetTerminalTypeName(type);

    }
    public GenernateTerminalSettings(terminal: any) {
        return JSON.stringify(terminal.settings);
    }
}