using UnityEngine;

public class ChapterInteractableElementData : ChapterInteractableCore, IElementData
{
    public DataElement DataElement { get; set; }

    public ChapterInteractableElementData() : base()
    {
        DataType = Enums.DataType.ChapterInteractable;
    }

    public string interactableName;
    public string objectGraphicIconPath;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        ClearChanges();
    }

    public new void GetOriginalValues() { }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IElementData Clone()
    {
        var elementData = new ChapterInteractableElementData();

        Debug.Log("Probably remove this");
        elementData.DataElement = DataElement;
        
        CloneCore(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var chapterInteractableDataSource = (ChapterInteractableElementData)dataSource;

        interactableName = chapterInteractableDataSource.interactableName;
        objectGraphicIconPath = chapterInteractableDataSource.objectGraphicIconPath;

        SetOriginalValues();
    }
}
