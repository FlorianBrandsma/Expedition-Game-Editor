using UnityEngine;

public class ChapterData : ChapterBaseData
{
    public override void GetOriginalValues(ChapterData originalData)
    {
        base.GetOriginalValues(originalData);
    }

    public ChapterData Clone()
    {
        var data = new ChapterData();
        
        base.Clone(data);

        return data;
    }

    public virtual void Clone(ChapterElementData elementData)
    {
        base.Clone(elementData);
    }
}
