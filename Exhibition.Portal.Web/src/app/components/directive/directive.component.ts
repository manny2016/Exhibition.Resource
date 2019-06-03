import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Terminal } from 'app/models/terminal';
import { ManagementService } from 'app/services/ManagementService';
import { Directive } from 'app/models/Directive';
import { environment } from 'environments/environment';
import { FormControl } from '@angular/forms';
import { MatSelect } from '@angular/material';
import { Bank, BANKS } from "./demo-data";
import { ReplaySubject, Subject } from 'rxjs';
import { take, takeUntil } from 'rxjs/operators';
import { DirectiveType } from '@angular/core/src/render3';
import { Resource } from 'app/models/Resource';
import {debounceTime, delay, tap, filter, map } from 'rxjs/operators';
@Component({
  selector: 'app-directive-content',
  templateUrl: "directive.component.html",
  styleUrls: ['./directive.component.css'],
  providers: [NgbActiveModal]
})
export class DirectiveContent implements OnInit {

  @Input() current: Directive;
  @Input() modalRef: NgbModalRef;
  @Input() action:string;
  // @ViewChild('dtypeSelect') dtypes: MatSelect;
  // @ViewChild('resourceSelect') resourceSelect: MatSelect;
  // @ViewChild('windowSelect') windowSelect: MatSelect;
  // @ViewChild('targetIpSelect') targetIpSelect: MatSelect;
  
  /**Begin select of directive type */
  public directiveTypes: any[] = environment.types;
  /** control for the selected bank */
  public dtypeCtrl: FormControl = new FormControl();
  /**End select of directive type */


  /**Begin select of terminal  */
  @Input() terminals: Terminal[];
  /** control for the selected bank */
  public terminalCtrl: FormControl = new FormControl();
  /**End select of directive type */

  /**Begin select of window  */
  protected windows: Window[];
  /** control for the selected bank */
  public windowCtrl: FormControl = new FormControl();
  /**End select of directive type */
  /** Start select of resource */
  // protected resources: Resource[];
  /** Start select of resource */
  /** control for the selected bank for server side filtering */
  public resourceServerSideCtrl:FormControl = new FormControl();
  /** control for filter for server side. */
  public resourceServerSideFilteringCtrl: FormControl = new FormControl();
  /** indicate search operation is in progress */
  public searching: boolean = false;
  public resources:Resource[];
  /** list of banks filtered after simulating server side search */
  public  filteredServerSideResource: ReplaySubject<Resource[]> = new ReplaySubject<Resource[]>(1);
  /** Subject that emits when the component has been destroyed. */
  protected _onDestroy = new Subject<void>();

  constructor(private service: ManagementService, public activeModal: NgbActiveModal) { }


  ngOnInit() {
    const that = this;
    let index = this.getIndexofTypes(that.current.type);    
    that.dtypeCtrl.setValue(that.directiveTypes[index]);
    
    
    that.terminalCtrl.valueChanges
      .pipe(takeUntil(that._onDestroy))
      .subscribe(() => {
        if (that.terminalCtrl.value) {
          that.windows = that.terminalCtrl.value.windows;
        }
        else {
          that.windows = [];
        }
      });

    that.service.QueryFileSystem({current:"",search:""}).toPromise().then(res=>{
      var source: any = res;
      that.resources = source.data;
      console.log("resource",source.data);
      that.resourceServerSideCtrl.setValue(that.current.resource);
    })
    that.resourceServerSideFilteringCtrl.valueChanges
    .pipe(
      filter(search => !!search),
      tap(() => that.searching = true),
      takeUntil(that._onDestroy),
      debounceTime(2000),
      map(search => {
        // simulate server fetching and filtering data
        return that.resources.filter(resource => resource.name.toLowerCase().indexOf(search) > -1);
      }),
      delay(500)
    )
    .subscribe(filteredBanks => {
      this.searching = false;
      this.filteredServerSideResource.next(filteredBanks);
    },
      error => {
        // no errors in our simulated example
        this.searching = false;
        // handle error...
      });

  }
  public getIndexofTypes(key: number) {
    for (let index = 0; index < this.directiveTypes.length; index++)
      if (this.directiveTypes[index].key == key) {
        return index;
      }
    return -1;
  }

  ngAfterViewInit() {
    const that = this;
    
    that.terminalCtrl.setValue(that.current.terminal);
    that.windowCtrl.setValue(that.current.window);
    
  }

  ngOnDestroy() {
    this._onDestroy.next();
    this._onDestroy.complete();
  }
  public CreateOrUpdateDirective(directive:Directive){
    this.service.CreateOrUpdateDirective(directive).toPromise()
    .then(res=>{
      if(res.success){
        this.activeModal.close("Close");
      }
      else{
          alert("Ip地址不允许为空或重复,请检查信息是否正确!");
          
      }            
    });
  }



}