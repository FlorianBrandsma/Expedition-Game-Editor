using UnityEngine;

public class SaveData : SaveBaseData
{
    public string ModelIconPath { get; set; }

    public string Name          { get; set; }
    public string LocationName  { get; set; }

    public string Time          { get; set; }

    public override void GetOriginalValues(SaveData originalData)
    {
        ModelIconPath   = originalData.ModelIconPath;

        Name            = originalData.Name;
        LocationName    = originalData.LocationName;

        Time            = originalData.Time;

        base.GetOriginalValues(originalData);
    }

    public SaveData Clone()
    {
        var data = new SaveData();
        
        data.ModelIconPath  = ModelIconPath;

        data.Name           = Name;
        data.LocationName   = LocationName;

        data.Time           = Time;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(SaveElementData elementData)
    {
        elementData.ModelIconPath  = ModelIconPath;

        elementData.Name            = Name;
        elementData.LocationName   = LocationName;

        elementData.Time           = Time;

        base.Clone(elementData);
    }
}
