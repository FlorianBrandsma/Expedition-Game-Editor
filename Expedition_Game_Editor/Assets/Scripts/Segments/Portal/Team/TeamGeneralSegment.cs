using UnityEngine;
using UnityEngine.UI;

public class TeamGeneralSegment : MonoBehaviour, ISegment
{
    public Text descriptionText;
    
    public EditorElement teamButton;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    #region Data properties
    public string Description
    {
        get { return ((TeamEditor)DataEditor).Description; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment()
    {
        InitializeTeamButton();
    }

    private void InitializeTeamButton()
    {
        teamButton.DataElement.Id   = DataEditor.EditData.Id;
        teamButton.DataElement.Data = DataEditor.Data.dataController.Data;
        teamButton.DataElement.Path = SegmentController.EditorController.PathController.route.path;

        teamButton.InitializeElement();
    }
    
    public void OpenSegment()
    {
        descriptionText.text = Description;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
