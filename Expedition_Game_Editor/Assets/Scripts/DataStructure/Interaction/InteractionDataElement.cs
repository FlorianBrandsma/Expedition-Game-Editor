﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class InteractionDataElement : InteractionCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public InteractionDataElement() : base() { }

    public int questId;
    public int objectiveId;

    public int objectGraphicId;
    public string objectGraphicPath;

    public string regionName;
    public string objectGraphicIconPath;

    public float height;
    public float width;
    public float depth;

    public Vector2 startPosition;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        if (!Changed) return;

        base.SetOriginalValues();

        ClearChanges();
    }

    public new void GetOriginalValues() { }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }

    public IDataElement Copy()
    {
        var dataElement = new InteractionDataElement();
        
        dataElement.SelectionElement = SelectionElement;

        dataElement.questId = questId;
        dataElement.objectiveId = objectiveId;

        dataElement.objectGraphicId = objectGraphicId;
        dataElement.objectGraphicPath = objectGraphicPath;

        dataElement.regionName = regionName;
        dataElement.objectGraphicIconPath = objectGraphicIconPath;

        dataElement.height = height;
        dataElement.width = width;
        dataElement.depth = depth;

        dataElement.startPosition = startPosition;

        CopyCore(dataElement);

        return dataElement;
    }
}
