// import { Component, Input, OnInit } from '@angular/core';
// import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

// import { ManagementService } from 'app/services/ManagementService';


// @Component({
//     selector: 'app-window-content',
//     templateUrl: "window.component.html",
//     providers: [NgbActiveModal]
// })
// export class WindowContent implements OnInit {
//     @Input() current: any;
//     @Input() modalRef: NgbModalRef;
//     public newly: any;

//     ngOnInit(): void {
//         this.newly = {
//             id: this.current.windows.length + 1,
//             location: { x: 0, y: 0 },
//             size: { width: 1024, height: 768 },
//             monitor: 1
//         };
//     }


//     constructor(public service: ManagementService,
//         public activeModal: NgbActiveModal) { }
//     /**
//     * 
//     * @param terminal 
//     */
//     public CreateWindow(window: any) {
//         const that = this;
//         var id: number = 1;
//         that.current.windows.forEach(ctx => {
//             if (ctx.id >= id) {
//                 id += 1;
//             }
//         });
//         var temp :any={id:id,size:window.size,location:window.location,monitor:window.monitor};
//         that.current.windows.push(temp);
//         that.CreateOrUpdateTerminal(that.current);
//         that.modalRef.close("Close");
//     }
//     public DeleteWindow(window: any) {
//         const that = this;        
//         var windows: any[] = [];
//         that.current.windows.forEach(ctx => {
//             if (ctx.id != window.id) {
//                 windows.push(ctx);
//             }
//         });
//         that.current.windows = windows;
//     }
//     public CreateOrUpdateTerminal(terminal: any) {
//         const that = this;
        
//         that.service.CreateOrUpdateTerminal(terminal).toPromise().then(res=>{
//             if(res.success){
//               that.modalRef.close("Close");
//             }
//             else{
//                 alert("Ip地址不允许为空或重复,请检查信息是否正确!");                
//             }            
//         })
//         .catch(error=>{
  
//         });
//     }
// }