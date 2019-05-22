export class MenuItem{
    public Id:string ;        
    public Text:string;
    public Args:string;
    public Uri:string;
    public ClassName:string;
    public isExpanded:boolean;
    public Items:MenuItem[];
}

export class Sidebar{
    public Id:string;
    public Name:string;
    public Tips:string;
    public isExpanded:boolean;
    public Items:MenuItem[];
}