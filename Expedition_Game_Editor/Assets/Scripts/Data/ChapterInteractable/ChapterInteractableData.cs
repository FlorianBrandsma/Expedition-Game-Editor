using UnityEngine;

public class ChapterInteractableData : ChapterInteractableBaseData
{
    public string InteractableName  { get; set; }

    public string ModelIconPath     { get; set; }

    public override void GetOriginalValues(ChapterInteractableData originalData)
    {
        InteractableName    = originalData.InteractableName;

        ModelIconPath       = originalData.ModelIconPath;

        base.GetOriginalValues(originalData);
    }

    public ChapterInteractableData Clone()
    {
        var data = new ChapterInteractableData();
        
        data.InteractableName   = InteractableName;

        data.ModelIconPath      = ModelIconPath;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(ChapterInteractableElementData elementData)
    {
        elementData.InteractableName    = InteractableName;

        elementData.ModelIconPath       = ModelIconPath;

        base.Clone(elementData);
    }
}
