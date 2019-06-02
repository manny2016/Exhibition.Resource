import { Terminal } from "./terminal";
import { Resource } from "./Resource";

export class Directive{
    public type:number;
    public terminal:Terminal;
    public window:number;
    public name:string;
    public resources:Resource[];
    public description:string;
}