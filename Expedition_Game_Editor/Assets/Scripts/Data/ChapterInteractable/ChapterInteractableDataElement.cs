using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ChapterInteractableDataElement : ChapterInteractableCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public ChapterInteractableDataElement() : base()
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

    public IDataElement Clone()
    {
        var dataElement = new ChapterInteractableDataElement();

        dataElement.SelectionElement = SelectionElement;
        
        CloneCore(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var chapterInteractableDataSource = (ChapterInteractableDataElement)dataSource;

        interactableName = chapterInteractableDataSource.interactableName;
        objectGraphicIconPath = chapterInteractableDataSource.objectGraphicIconPath;

        SetOriginalValues();
    }
}
