using UnityEngine;
using System.Collections.Generic;

public class InteractionData : InteractionBaseData
{
    public string ModelIconPath     { get; set; }

    public string InteractableName  { get; set; }
    public string LocationName      { get; set; }

    public bool TimeConflict        { get; set; }
    public List<int> DefaultTimes   { get; set; } = new List<int>();

    public override void GetOriginalValues(InteractionData originalData)
    {
        ModelIconPath       = originalData.ModelIconPath;

        InteractableName    = originalData.InteractableName;
        LocationName        = originalData.LocationName;

        TimeConflict        = originalData.TimeConflict;
        DefaultTimes        = new List<int>(originalData.DefaultTimes);

        base.GetOriginalValues(originalData);
    }

    public InteractionData Clone()
    {
        var data = new InteractionData();
        
        data.ModelIconPath      = ModelIconPath;

        data.InteractableName   = InteractableName;
        data.LocationName       = LocationName;

        data.TimeConflict       = TimeConflict;
        data.DefaultTimes       = new List<int>(DefaultTimes);

        base.Clone(data);

        return data;
    }

    public virtual void Clone(InteractionElementData elementData)
    {
        elementData.ModelIconPath       = ModelIconPath;

        elementData.InteractableName    = InteractableName;
        elementData.LocationName        = LocationName;

        elementData.TimeConflict        = TimeConflict;
        elementData.DefaultTimes        = new List<int>(DefaultTimes);

        base.Clone(elementData);
    }
}
