import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { ManagementService } from 'app/services/ManagementService';

import { environment } from 'environments/environment';
import { FormControl } from '@angular/forms';
import { MatSelect } from '@angular/material';
import { Bank, BANKS, BANKGROUPS } from "./demo-data";
import { ReplaySubject, Subject } from 'rxjs';
import { take, takeUntil } from 'rxjs/operators';
import { DirectiveType } from '@angular/core/src/render3';

import { debounceTime, delay, tap, filter, map } from 'rxjs/operators';
import { ChangeDetectorRef } from '@angular/core';

import { Type } from '../../../../node_modules/@angular/compiler';
import { element } from 'protractor';

@Component({
  selector: 'app-directive-content',
  templateUrl: "directive.component.html",
  styleUrls: ['./directive.component.css'],
  providers: [NgbActiveModal]
})
export class DirectiveContent implements OnInit {
  @Input() current: any;
  @Input() terminals: any[];
  private currentTerminal: any;
  public resources: any[];
  public windows: any[];
  /**Begin select of terminal  */


  /** control for the selected bank */
  public terminalCtrl: FormControl = new FormControl();
  /**End select of directive type */

  /**Begin select of window  */

  /** control for the selected bank */
  public windowCtrl: FormControl = new FormControl();
  /**End select of directive type */

  /** list of bank groups */
  protected bankGroups: any[] = BANKGROUPS;

  /** control for the selected bank for option groups */
  public bankGroupsCtrl: FormControl = new FormControl();

  /** control for the MatSelect filter keyword for option groups */
  public bankGroupsFilterCtrl: FormControl = new FormControl();
  
  /** list of bank groups filtered by search keyword for option groups */
  public filteredBankGroups: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  
  @ViewChild('multiSelect') multiSelect: MatSelect;
  @ViewChild('singleSelect') singleSelect: MatSelect;
  

  public searching: boolean = false;
  public onlyShowFolder:boolean =true;
  /** Subject that emits when the component has been destroyed. */
  protected _onDestroy = new Subject<void>();

  constructor(private service: ManagementService, public activeModal: NgbActiveModal, private cdRef: ChangeDetectorRef) { }


  ngOnInit() {
    const that = this;
    that.service.QueryResourceforChoosing({}).toPromise().then(res => {
      // that.banks = res.data;
      var temp: any = res.data;
      that.bankGroups = temp;
      that.filteredBankGroups.next(that.copyBankGroups(that.bankGroups));
      that.bankGroupsFilterCtrl.valueChanges
        .pipe(takeUntil(that._onDestroy))
        .subscribe(() => {
          that.filterBankGroups();
        });
    })

    //console.log(that.current.terminal);
    that.terminalCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(element => {
        that.windows = element.settings.windows;
        if (element.settings.windows) {
          element.settings.windows.forEach(window => {
            console.log(that.current.DefaultWindow, "->", window);
            if (that.current.DefaultWindow != null && that.current.DefaultWindow.id != 0) {
              if (that.current.DefaultWindow.id == window.id) {
                that.windowCtrl.setValue(window);
              }
            }
          });
        }
      });
    
  }


  ngAfterViewInit() {
    const that = this;
    //this.terminalCtrl.setValue(this.current.terminal);
    that.terminals.forEach((element, index) => {

      if (element.name == that.current.terminal.name) {
        that.terminalCtrl.setValue(that.terminals[index])
        that.windows = element.settings.windows;
        if (element.settings.windows) {
          element.settings.windows.forEach(window => {            
            if (that.current.DefaultWindow != null && that.current.DefaultWindow.id != 0) {
              if (that.current.DefaultWindow.id == window.id) {
                that.windowCtrl.setValue(window);
              }
            }
          });
        }
      }
    });
   
    that.cdRef.detectChanges();
  }

  ngOnDestroy() {
    this._onDestroy.next();
    this._onDestroy.complete();
  }
  public CreateOrUpdateDirective(context: any) {
    context.terminal = this.terminalCtrl.value;
    context.DefaultWindow = this.windowCtrl.value;
    console.log(context.DefaultWindow);
    this.service.CreateOrUpdateDirective(context).toPromise()
      .then(res => {
        if (res.success) {
          this.activeModal.close("Close");
        }
        else {
          alert("Ip地址不允许为空或重复,请检查信息是否正确!");
        }
      });
  }


  ngAfterViewChecked() {

  }

  protected filterBankGroups() {
    if (!this.bankGroups) {
      return;
    }
    // get the search keyword
    let search = this.bankGroupsFilterCtrl.value;
    const bankGroupsCopy = this.copyBankGroups(this.bankGroups);
    if (!search) {
      this.filteredBankGroups.next(bankGroupsCopy);
      return;
    } else {
      search = search.toLowerCase();
    }
    // filter the banks
    this.filteredBankGroups.next(
      bankGroupsCopy.filter(bankGroup => {
        const showBankGroup = bankGroup.name.toLowerCase().indexOf(search) > -1;
        if (!showBankGroup) {
          bankGroup.items = bankGroup.items.filter(bank => bank.name.toLowerCase().indexOf(search) > -1);
        }
        return bankGroup.items.length > 0;
      })
    );
  }
  protected setInitialValue() {
    this.filteredBankGroups
      .pipe(take(1), takeUntil(this._onDestroy))
      .subscribe(() => {
        // setting the compareWith property to a comparison function
        // triggers initializing the selection according to the initial value of
        // the form control (i.e. _initializeSelection())
        // this needs to be done after the filteredBanks are loaded initially
        // and after the mat-option elements are available
        this.multiSelect.compareWith = (a: Bank, b: Bank) => a && b && a.id === b.id;
      });
  }

  protected copyBankGroups(bankGroups: any[]) {
    const bankGroupsCopy = [];
    bankGroups.forEach(bankGroup => {
      bankGroupsCopy.push({
        name: bankGroup.name,
        items: bankGroup.items.slice()
      });
    });
    return bankGroupsCopy;
  }
  protected CheckedOnlyShowFolder(value:boolean){
    const that = this;
    that.service.QueryResourceforChoosing({onlyShowFolder:value}).toPromise().then(res => {
      // that.banks = res.data;
      var temp: any = res.data;
      that.bankGroups = temp;
      that.filteredBankGroups.next(that.copyBankGroups(that.bankGroups));
      that.bankGroupsFilterCtrl.valueChanges
        .pipe(takeUntil(that._onDestroy))
        .subscribe(() => {
          that.filterBankGroups();
        });
    })
  }
}