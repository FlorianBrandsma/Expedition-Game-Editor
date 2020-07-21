using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSaveElementData : InteractableSaveCore, IElementData
{
    public DataElement DataElement { get; set; }

    public InteractableSaveElementData() : base()
    {
        DataType = Enums.DataType.InteractableSave;
    }

    public int objectGraphicId;

    public string interactableName;

    public int health;
    public int hunger;
    public int thirst;

    public float weight;
    public float speed;
    public float stamina;

    public string objectGraphicPath;
    public string objectGraphicIconPath;

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
        var elementData = new ChapterSaveElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        SetOriginalValues();
    }
}
