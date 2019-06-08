import { Component, Input } from '@angular/core';
import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ManagementService } from 'app/services/ManagementService';


@Component({
  selector: 'app-mediaplayer-editcontent',
  templateUrl: "MediaPlayerEdit.Component.html",

  providers: [NgbActiveModal],
  entryComponents: []

})
export class MediaPlayerEdit {
  @Input() current: any;
  @Input() modalRef: NgbModalRef;
  //newlyWindow: any = { id: 0, location: { x: 0, y: 0 }, size: { width: 1920, height: 768 }, monitor: 1 }
  constructor(public service: ManagementService,
    public activeModal: NgbActiveModal) { }
  /**
  * 
  * @param terminal 
  */
  public CreateOrUpdateTerminal(terminal: any) {
    const that = this;
    console.log(terminal);
    that.service.CreateOrUpdateMediaPlayerTerminal(terminal).toPromise().then(res => {
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