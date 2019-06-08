import { Component, OnInit } from '@angular/core';

declare const $: any;
declare interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
}
export const ROUTES: RouteInfo[] = [
    // { path: '/dashboard', title: '仪表盘',  icon: 'dashboard', class: '' },
    { path: '/resource', title: '资源管理',  icon:'library_books', class: '' },
    { path: '/terminal', title: '设备管理',  icon:'content_paste', class: '' },    
    { path: '/directive', title: '指令配置',  icon:'bubble_chart', class: '' }
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  menuItems: any[];

  constructor() { }

  ngOnInit() {
    this.menuItems = ROUTES.filter(menuItem => menuItem);
  }
  isMobileMenu() {
      if ($(window).width() > 991) {
          return false;
      }
      return true;
  };
}
