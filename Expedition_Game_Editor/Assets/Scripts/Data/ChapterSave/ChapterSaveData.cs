﻿using UnityEngine;

public class ChapterSaveData : ChapterSaveBaseData
{
    public int Index            { get; set; }

    public string Name          { get; set; }

    public string PublicNotes   { get; set; }
    public string PrivateNotes  { get; set; }

    public override void GetOriginalValues(ChapterSaveData originalData)
    {
        Index           = originalData.Index;

        Name            = originalData.Name;

        PublicNotes     = originalData.PublicNotes;
        PrivateNotes    = originalData.PrivateNotes;

        base.GetOriginalValues(originalData);
    }

    public ChapterSaveData Clone()
    {
        var data = new ChapterSaveData();

        data.Index          = Index;

        data.Name           = Name;

        data.PublicNotes    = PublicNotes;
        data.PrivateNotes   = PrivateNotes;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(ChapterSaveElementData elementData)
    {
        elementData.Index           = Index;

        elementData.Name            = Name;

        elementData.PublicNotes     = PublicNotes;
        elementData.PrivateNotes    = PrivateNotes;

        base.Clone(elementData);
    }
}