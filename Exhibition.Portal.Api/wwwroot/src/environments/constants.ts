import { Injectable } from '@angular/core';
import { Type } from '@angular/compiler';
import {Sidebar,MenuItem} from '../app/models/Navigation';
export const Constants : any ={
    Sidebar:[
        {Name:"资源管理",Items:[
            {Id:"01",Text:"资源管理",Uri:"javascript:void()",ClassName:"",isExpanded:false,Items:[
                {Id:"01-01",Text:"视频文件",Type:"",Uri:"/resource?type=video"},
                {Id:"01-02",Text:"图片集合",Type:"",Uri:"/resource?type=image"},
                {Id:"01-02",Text:"H5网页",Type:"",Uri:"/resource?type=webpage"},
            ]},
            {Id:"02",Text:"资源管理",Uri:"javascript:void()",ClassName:"",isExpanded:false,Items:[
                {Id:"02-01",Text:"视频文件",Type:"",Uri:"/resource?type=video"},
                {Id:"02-02",Text:"图片集合",Type:"",Uri:"/resource?type=image"},
                {Id:"02-02",Text:"H5网页",Type:"",Uri:"/resource?type=webpage"},
            ]}
        ]},
        {Name:"指令配置",Items:[
            {Id:"01",Text:"资源管理",Uri:"javascript:void()",ClassName:"",isExpanded:false,Items:[
    
            ]}
        ]}
    ]
};
// = {
    
//     // Navigation: [
//     //     { Id:"01",Text:"资源管理",Type:"",Uri:"/",Items:[
//     //         {Id:"01-01",Text:"视频文件",Type:"",Uri:"/resource?type=video"},
//     //         {Id:"01-02",Text:"图片集合",Type:"",Uri:"/resource?type=image"},
//     //         {Id:"01-05",Text:"H5网页",Type:"",Uri:"/resource?type=image"},
//     //     ] },
//     //     {
//     //         Id:"02",Text:"播放终端",type:"",Uri:"",Items:[
//     //             {Id:"02-01",Text:"三连屏(192.168.0.12)",Type:"",Uri:"/Terminal?TerminalId=1"},
//     //             {Id:"02-02",Text:"三连屏(192.168.0.13)",Type:"",Uri:"/Terminal?TerminalId=2"}
//     //         ]
//     //     }        
//     // ]
// }