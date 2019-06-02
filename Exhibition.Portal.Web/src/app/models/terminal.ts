export class Terminal {
    public ip: string;
    public name: string;
    public description: string;
    public schematic: string;
    public endpoint: string;
    public editable:boolean;
    public windows: Window[];
}
export class Window {
    public id: number
    public location: Point;
    public size:Size;
    public monitor:number;
}
export class Point {
    public x: number;
    public y: number;
}
export class Size {
    public width: number;
    public height: number;
}