import { Component, Input } from '@angular/core';
import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ManagementService } from 'app/services/ManagementService';


@Component({
  selector: 'app-serialport-editcontent',
  templateUrl: "./SerialPortEdit.Component.html",

  providers: [NgbActiveModal],
  entryComponents: []

})
export class SerialPortEdit {
  @Input() current: any;
  @Input() modalRef: NgbModalRef;
  constructor(public service: ManagementService,
    public activeModal: NgbActiveModal) { }
  /**
  * 
  * @param terminal 
  */
  public CreateOrUpdateTerminal(terminal: any) {
    const that = this;
    console.log(terminal);
    that.service.CreateOrUpdateSerialPortTerminal(terminal).toPromise().then(res => {      
      if (res.success) {
        that.activeModal.close("Close");
      }
      else {
        
      }
    })
      .catch(error => {

      });
  }
  public GetTerminalTypeName(type: number) {
    return this.service.GetTerminalTypeName(type);
  }
}