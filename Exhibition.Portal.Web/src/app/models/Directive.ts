import { Terminal,Window } from "./terminal";
import { Resource } from "./Resource";

export class Directive{
    public type:number;
    public terminal:Terminal;
    public window:Window;    
    public name:string;
    public resource:Resource;
    public description:string;
}

export class OptionModel{
    public key:string;
    public text:string;
}

export class DirectiveforEditing{
    public type:number;
    public terminalIp:string;
    public windowId:number;    
    public name:string;
    public resourceFullName:string;
    public description:string;
}