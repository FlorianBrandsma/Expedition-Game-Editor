using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ChapterDataElement : ChapterCore
{
    public ChapterDataElement() : base() { }

    public List<int> elementIds;

    public List<int> originalElementIds;

    private bool changedElementIds;

    public List<int> ElementIds
    {
        get { return elementIds; }
        set
        {
            if (elementIds.SequenceEqual(value)) return;

            changed = true;
            changedElementIds = true;

            elementIds = value;
        }
    }

    public override void Update()
    {
        if (!changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        originalElementIds = elementIds;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        elementIds = originalElementIds;
    }

    public override void ClearChanges()
    {
        changedElementIds = false;

        base.ClearChanges();

        GetOriginalValues();
    }
}
