﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SaveDataElement : SaveCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public SaveDataElement() : base()
    {
        DataType = Enums.DataType.Save;
    }

    public string name;

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

    public IDataElement Clone()
    {
        var dataElement = new ChapterDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        SetOriginalValues();
    }
}