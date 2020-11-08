using UnityEngine;

public class SceneActorData : SceneActorBaseData
{
    public string InteractableName  { get; set; }
    public string ModelIconPath     { get; set; }
    
    public override void GetOriginalValues(SceneActorData originalData)
    {
        InteractableName = originalData.InteractableName;
        ModelIconPath = originalData.ModelIconPath;

        base.GetOriginalValues(originalData);
    }

    public SceneActorData Clone()
    {
        var data = new SceneActorData();

        data.InteractableName = InteractableName;
        data.ModelIconPath = ModelIconPath;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(SceneActorElementData elementData)
    {
        elementData.InteractableName = InteractableName;
        elementData.ModelIconPath = ModelIconPath;

        base.Clone(elementData);
    }
}