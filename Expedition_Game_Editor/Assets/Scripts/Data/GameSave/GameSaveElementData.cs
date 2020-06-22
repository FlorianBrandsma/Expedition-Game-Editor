using UnityEngine;
using System.Collections.Generic;

public class GameSaveElementData : GeneralData, IElementData
{
    public List<ChapterSaveElementData> chapterSaveDataList;
    public List<PhaseSaveElementData> phaseSaveDataList;
    public List<QuestSaveElementData> questSaveDataList;
    public List<ObjectiveSaveElementData> objectiveSaveDataList;
    public List<TaskSaveElementData> taskSaveDataList;
    public List<InteractionSaveElementData> interactionSaveDataList;

    public DataElement DataElement { get; set; }
    public bool Changed { get; set; }

    public void Update() { }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public void SetOriginalValues()
    {
        chapterSaveDataList.ForEach(x => x.SetOriginalValues());
        phaseSaveDataList.ForEach(x => x.SetOriginalValues());
        questSaveDataList.ForEach(x => x.SetOriginalValues());
        objectiveSaveDataList.ForEach(x => x.SetOriginalValues());
        taskSaveDataList.ForEach(x => x.SetOriginalValues());
        interactionSaveDataList.ForEach(x => x.SetOriginalValues());
    }

    public void GetOriginalValues() { }

    public void ClearChanges() { }

    public IElementData Clone()
    {
        var elementData = new EditorWorldElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    new public virtual void Copy(IElementData dataSource) { }
}
