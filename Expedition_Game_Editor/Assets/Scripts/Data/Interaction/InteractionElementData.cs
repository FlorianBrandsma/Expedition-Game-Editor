﻿using System.Collections.Generic;
using System.Linq;

public class InteractionElementData : InteractionCore, IElementData
{
    public DataElement DataElement { get; set; }

    public InteractionElementData() : base()
    {
        DataType = Enums.DataType.Interaction;
    }
    
    public bool timeConflict;  
    public List<int> defaultTimes;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        ClearChanges();
    }

    public new void GetOriginalValues() { }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IElementData Clone()
    {
        var elementData = new InteractionElementData();

        elementData.timeConflict = timeConflict;
        elementData.defaultTimes = defaultTimes.ToList();

        CloneCore(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var interactionDataSource = (InteractionElementData)dataSource;

        timeConflict = interactionDataSource.timeConflict;
        defaultTimes = interactionDataSource.defaultTimes.ToList();

        SetOriginalValues();
    }
}
