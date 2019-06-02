
import { Component, OnInit, Pipe, Input } from '@angular/core';
import { Terminal, Window, Point, Size } from 'app/models/terminal';
import { ManagementService } from 'app/services/ManagementService';
import { NgbModal, NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import { TerminalContent } from 'app/components/terminaledit/TerminalContent.compnent';
import { WindowContent } from 'app/components/terminaledit/window.component';

@Component({
    selector: 'app-terminal-component',
    templateUrl: './terminal.component.html',
    styleUrls: ['./terminal.component.css']
})

export class TerminalComponent implements OnInit {
    constructor(public service: ManagementService, private modalService: NgbModal) { }
    public dtOptions: DataTables.Settings = {};
    public terminals: Terminal[];
    public current: Terminal = null;
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
    public OpenTerminalEdit(content:Terminal) {
        const that = this;
       // that.current = { ip: "", name: "", description: "", endpoint: "", schematic: "", editable:false, windows: [] };
        const modalRef= that.modalService.open(TerminalContent,{ size: 'lg' });
        modalRef.componentInstance.current = content;
        modalRef.componentInstance.modalRef = modalRef;
        modalRef.result.then(ctx=>{
            that.Refresh();
        });
    }
    /**
     * 
     */
    public Refresh(){
        const that = this;
        that.service.QueryTerminals({ search: null }).toPromise().then(res => {
            that.terminals = res.data;
        });
    }
    /**
     * 
     * @param context 
     */
    public DeleteTerminal(context:Terminal){
        const that = this;
        if (confirm("真的要删除吗?")) {
          that.service.DeleteTerminal(context)
            .toPromise().then(res => {
              this.Refresh();
            });
        }
    }
    /**
     * 
     * @param content 
     */
    public OpenWindowEdit(content:Terminal){
        const that = this;
        const modalRef= that.modalService.open(WindowContent);
        modalRef.componentInstance.current = content;
        modalRef.componentInstance.modalRef = modalRef;
        modalRef.result.then(ctx=>{
            that.Refresh();
        });
    }
}