using UnityEngine;
using System.Linq;

public class ChapterInteractableCore : GeneralData
{
    private int chapterId;

    private int interactableId;

    //Original
    public int originalInteractableId;

    //Changed
    private bool changedInteractableId;

    public bool Changed
    {
        get
        {
            return changedInteractableId;
        }
    }

    #region Properties
    public int ChapterId
    {
        get { return chapterId; }
        set { chapterId = value; }
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
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var chapterInteractableData = Fixtures.chapterInteractableList.Where(x => x.Id == Id).FirstOrDefault();
        
        if (changedInteractableId)
            chapterInteractableData.interactableId = interactableId;
    }

    public void UpdateSearch()
    {
        Update();
    }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalInteractableId = InteractableId;
    }

    public void GetOriginalValues()
    {
        interactableId = originalInteractableId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedInteractableId = false;
    }

    public void Delete() { }

    public void CloneCore(ChapterInteractableElementData elementData) { }
    #endregion

    new public virtual void Copy(IElementData dataSource)
    {
        var chapterInteractableDataSource = (ChapterInteractableElementData)dataSource;

        chapterId = chapterInteractableDataSource.chapterId;
        interactableId = chapterInteractableDataSource.interactableId;
    }
}
