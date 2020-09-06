using UnityEngine;

public class ObjectiveData : ObjectiveBaseData
{
    public override void GetOriginalValues(ObjectiveData originalData)
    {
        base.GetOriginalValues(originalData);
    }

    public ObjectiveData Clone()
    {
        var data = new ObjectiveData();
        
        base.Clone(data);

        return data;
    }

    public virtual void Clone(ObjectiveElementData elementData)
    {
        base.Clone(elementData);
    }
}
