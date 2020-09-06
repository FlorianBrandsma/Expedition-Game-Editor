using UnityEngine;

public class QuestData : QuestBaseData
{
    public override void GetOriginalValues(QuestData originalData)
    {
        base.GetOriginalValues(originalData);
    }

    public QuestData Clone()
    {
        var data = new QuestData();
        
        base.Clone(data);

        return data;
    }

    public virtual void Clone(QuestElementData elementData)
    {
        base.Clone(elementData);
    }
}
