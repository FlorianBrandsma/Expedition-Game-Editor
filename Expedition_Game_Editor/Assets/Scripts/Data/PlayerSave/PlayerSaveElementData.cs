using UnityEngine;
using System;

public class PlayerSaveElementData : PlayerSaveCore, IElementData
{
    public DataElement DataElement { get; set; }

    public PlayerSaveElementData() : base()
    {
        DataType = Enums.DataType.Interaction;
    }

    public int objectGraphicId;
    public string objectGraphicPath;

    public string objectGraphicIconPath;

    public float height;
    public float width;
    public float depth;

    public string interactableName;

    public DateTime saveTime;

    //Testing purposes
    public DateTime testTime;
    public TimeSpan passedTime { get { return  saveTime - testTime; } }
    public float passedSeconds { get { return passedTime.Seconds; } }
    //----------------

    public override void Update()
    {
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
        var elementData = new PlayerSaveElementData();

        CloneCore(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);
        
        SetOriginalValues();
    }
}
