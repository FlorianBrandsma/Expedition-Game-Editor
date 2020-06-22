using UnityEngine;
using System.Collections.Generic;

public class GameSaveDataElement : GeneralData, IDataElement
{
    public List<ChapterSaveDataElement> chapterSaveDataList;
    public List<PhaseSaveDataElement> phaseSaveDataList;
    public List<QuestSaveDataElement> questSaveDataList;
    public List<ObjectiveSaveDataElement> objectiveSaveDataList;
    public List<TaskSaveDataElement> taskSaveDataList;
    public List<InteractionSaveDataElement> interactionSaveDataList;

    #region DataElement
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

    public IDataElement Clone()
    {
        var dataElement = new EditorWorldDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    new public virtual void Copy(IDataElement dataSource) { }
    #endregion
}
