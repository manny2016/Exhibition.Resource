import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Terminal, Window } from 'app/models/terminal';
import { ManagementService } from 'app/services/ManagementService';
import { Directive, DirectiveforEditing } from 'app/models/Directive';
import { environment } from 'environments/environment';
import { FormControl } from '@angular/forms';
import { MatSelect } from '@angular/material';
import { Bank, BANKS } from "./demo-data";
import { ReplaySubject, Subject } from 'rxjs';
import { take, takeUntil } from 'rxjs/operators';
import { DirectiveType } from '@angular/core/src/render3';
import { Resource } from 'app/models/Resource';
import { debounceTime, delay, tap, filter, map } from 'rxjs/operators';
import { ChangeDetectorRef } from '@angular/core';
import { OptionModel } from 'app/models/Directive';
import { Type } from '../../../../node_modules/@angular/compiler';

@Component({
  selector: 'app-directive-content',
  templateUrl: "directive.component.html",
  styleUrls: ['./directive.component.css'],
  providers: [NgbActiveModal]
})
export class DirectiveContent implements OnInit {
  @Input() current: DirectiveforEditing;
  @Input() action: string;
  @Input() terminals: Terminal[];
  @Input() resources: Resource[];
  @Input() types: any[];

  /**Begin select of directive type */
  public omDirectiveTypes: any[] = environment.types;
  public omTerminals: OptionModel[];
  public omResources: OptionModel[];
  /** control for the selected bank */
  public dtypeCtrl: FormControl = new FormControl();
  /**End select of directive type */


  /**Begin select of terminal  */




  /** control for the selected bank */
  public terminalCtrl: FormControl = new FormControl();
  /**End select of directive type */

  /**Begin select of window  */
  protected omWindows: OptionModel[];
  /** control for the selected bank */
  public windowCtrl: FormControl = new FormControl();
  /**End select of directive type */
  /** Start select of resource */
  // protected resources: Resource[];
  /** Start select of resource */
  /** control for the selected bank for server side filtering */
  public resourceServerSideCtrl: FormControl = new FormControl();
  /** control for filter for server side. */
  public resourceServerSideFilteringCtrl: FormControl = new FormControl();
  /** indicate search operation is in progress */
  public searching: boolean = false;

  /** list of banks filtered after simulating server side search */
  public filteredServerSideResource: ReplaySubject<OptionModel[]> = new ReplaySubject<OptionModel[]>(1);
  /** Subject that emits when the component has been destroyed. */
  protected _onDestroy = new Subject<void>();

  constructor(private service: ManagementService, public activeModal: NgbActiveModal, private cdRef: ChangeDetectorRef) { }


  ngOnInit() {
    const that = this;

    that.dtypeCtrl.setValue(that.current.type);
    that.service.QueryResourceforChoosing({ current: "", search: "" }).toPromise().then(res => {
      that.omResources = res.data;
      console.log("that.omResources",that.omResources);
      that.resourceServerSideCtrl.setValue(that.current.resourceFullName);
    });
    that.service.QueryTerminalforChoosing({ search: "" }).toPromise().then(res => {
      that.omTerminals = res.data;
      that.terminalCtrl.setValue(that.current.terminalIp);
    });
    that.terminalCtrl.setValue(that.current.terminalIp);
    that.resourceServerSideCtrl.setValue(this.current.resourceFullName);
    that.terminalCtrl.valueChanges.pipe()
    that.terminalCtrl.valueChanges
      .pipe(takeUntil(that._onDestroy))
      .subscribe(() => {
        if (that.terminalCtrl.value) {
          that.terminals.forEach(element => {
            if (element.ip == that.terminalCtrl.value) {
              that.omWindows = [];
              element.windows.forEach(window => {
                var text = window.id + " | " + window.location.x + " * " + window.location.y + " (x * y) |" +
                  window.size.width + " * " + window.size.height + "(宽 * 高)";
                that.omWindows.push({ key: window.id.toString(), text: text });
              });
            }
            that.windowCtrl.setValue(this.current.windowId);
          });
        }
      });


    that.resourceServerSideFilteringCtrl.valueChanges
      .pipe(
        filter(search => !!search),
        tap(() => that.searching = true),
        takeUntil(that._onDestroy),
        debounceTime(2000),
        map(search => {
          // simulate server fetching and filtering data
          if (search == null||search=="") {
            return that.omResources;
          }
          return that.omResources.filter(resource => resource.text.toLowerCase().indexOf(search) > -1);
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
  // public getIndexofTypes(key: number) {
  //   for (let index = 0; index < this.directiveTypes.length; index++)
  //     if (this.directiveTypes[index].key == key) {
  //       return index;
  //     }
  //   return -1;
  // }

  ngAfterViewInit() {
    const that = this;


    //that.windowCtrl.setValue(that.current.window.id);
  }

  ngOnDestroy() {
    this._onDestroy.next();
    this._onDestroy.complete();
  }
  public CreateOrUpdateDirective(context: DirectiveforEditing) {
    let directive:Directive = {
      name : context.name,
      terminal : this.GetChoosedTerminal(context.terminalIp),
      window: this.windowCtrl.value,
      description: context.description,
      resource :this.GetChoosedResource(context.resourceFullName),
      type :this.dtypeCtrl.value
    };
    console
    this.service.CreateOrUpdateDirective(directive).toPromise()
      .then(res => {
        if (res.success) {
          this.activeModal.close("Close");
        }
        else {
          alert("Ip地址不允许为空或重复,请检查信息是否正确!");
        }
      });
  }
  protected GetChoosedTerminal(ip:string):Terminal{
    this.terminals.forEach(element => {
      if(ip==element.ip) return element;
    });
    return null;
  }
  protected GetChoosedResource(fullName:string):Resource{
    this.resources.forEach(element => {
      if(fullName==element.fullName) return element;
    });
    return null;
  }
  ngAfterViewChecked() {
    

  }
}