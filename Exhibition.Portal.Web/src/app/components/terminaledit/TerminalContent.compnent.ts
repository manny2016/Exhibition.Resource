import { Component, Input } from '@angular/core';
import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Terminal } from 'app/models/terminal';
import { ManagementService } from 'app/services/ManagementService';


@Component({
  selector: 'app-terminal-content',
  templateUrl:"TerminalContent.component.html",
  providers:[NgbActiveModal],
  entryComponents:[]
  
})
export class TerminalContent {
  @Input() current:Terminal;
  @Input() modalRef:NgbModalRef;  
  constructor(public service: ManagementService,
    public activeModal: NgbActiveModal) {}
     /**
     * 
     * @param terminal 
     */
    public CreateOrUpdateTerminal(terminal: Terminal) {
      const that = this;
      console.log(terminal);
      that.service.CreateOrUpdateTerminal(terminal).toPromise().then(res=>{
          if(res.success){
            that.activeModal.close("Close");
          }
          else{
              alert("Ip地址不允许为空或重复,请检查信息是否正确!");
              
          }            
      })
      .catch(error=>{

      });
  }
}