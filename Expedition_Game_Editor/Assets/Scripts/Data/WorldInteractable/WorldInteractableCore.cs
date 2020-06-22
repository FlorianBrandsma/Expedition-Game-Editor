using UnityEngine;
using System.Linq;

public class WorldInteractableCore : GeneralData
{
    private int type;

    private int phaseId;
    private int questId;
    private int objectiveId;

    private int chapterInteractableId;
    private int interactableId;

    //Original
    public int originalInteractableId;
    public int originalQuestId;

    //Changed
    private bool changedInteractableId;
    private bool changedQuestId;

    public bool Changed
    {
        get
        {
            return changedInteractableId || changedQuestId;
        }
    }

    #region Properties
    public int Type
    {
        get { return type; }
        set { type = value; }
    }

    public int ChapterInteractableId
    {
        get { return chapterInteractableId; }
        set { chapterInteractableId = value; }
    }

    public int InteractableId
    {
        get { return interactableId; }
        set
        {
            if (value == interactableId) return;

            changedInteractableId = (value != originalInteractableId);

            interactableId = value;
        }
    }

    public int PhaseId
    {
        get { return phaseId; }
        set { phaseId = value; }
    }

    public int QuestId
    {
        get { return questId; }
        set
        {
            if (value == questId) return;

            changedQuestId = (value != originalQuestId);

            questId = value;
        }
    }

    public int ObjectiveId
    {
        get { return objectiveId; }
        set { objectiveId = value; }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var worldInteractableData = Fixtures.worldInteractableList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedInteractableId)
            worldInteractableData.interactableId = interactableId;

        if (changedQuestId)
            worldInteractableData.questId = questId;
    }

    virtual public void UpdateSearch()
    {
        Update();
    }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalInteractableId = interactableId;
        originalQuestId = questId;
    }

    public void GetOriginalValues()
    {
        interactableId = originalInteractableId;
        questId = originalQuestId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedInteractableId = false;
        changedQuestId = false;
    }

    public void Delete() { }

    public void CloneCore(WorldInteractableElementData elementData)
    {
        CloneGeneralData(elementData);

        elementData.type = type;

        elementData.phaseId = phaseId;
        elementData.questId = questId;
        elementData.objectiveId = objectiveId;

        elementData.chapterInteractableId = chapterInteractableId;
        elementData.interactableId = interactableId;

        elementData.originalQuestId = originalQuestId;

        elementData.originalInteractableId = originalInteractableId;       
    }
    #endregion

    new public virtual void Copy(IElementData dataSource)
    {
        var worldInteractableDataSource = (WorldInteractableElementData)dataSource;

        type = worldInteractableDataSource.type;

        phaseId = worldInteractableDataSource.phaseId;
        questId = worldInteractableDataSource.questId;
        objectiveId = worldInteractableDataSource.objectiveId;

        chapterInteractableId = worldInteractableDataSource.chapterInteractableId;
        interactableId = worldInteractableDataSource.interactableId;
    }
}
