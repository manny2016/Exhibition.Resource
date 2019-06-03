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