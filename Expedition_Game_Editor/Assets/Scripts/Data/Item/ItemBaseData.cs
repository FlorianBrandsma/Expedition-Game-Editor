using UnityEngine;

public class ItemBaseData
{
    public int Id           { get; set; }
    
    public int ProjectId    { get; set; }
    public int ModelId      { get; set; }

    public int Type         { get; set; }

    public int Index        { get; set; }

    public string Name      { get; set; }
 
    public virtual void GetOriginalValues(ItemData originalData)
    {
        Id          = originalData.Id;

        ProjectId   = originalData.ProjectId;
        ModelId     = originalData.ModelId;

        Type        = originalData.Type;

        Index       = originalData.Index;

        Name        = originalData.Name;
    }

    public virtual void Clone(ItemData data)
    {
        data.Id         = Id;

        data.ProjectId  = ProjectId;
        data.ModelId    = ModelId;

        data.Type       = Type;

        data.Index      = Index;

        data.Name       = Name;
    }
}
