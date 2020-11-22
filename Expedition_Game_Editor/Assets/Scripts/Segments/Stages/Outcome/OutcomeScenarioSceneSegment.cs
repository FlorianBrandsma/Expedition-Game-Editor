using UnityEngine;

public class OutcomeScenarioSceneSegment : MonoBehaviour, ISegment
{
    public EditorElement sceneButton;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private PhaseEditor PhaseEditor { get { return (PhaseEditor)DataEditor; } }

    #region Data properties
    private int Id
    {
        get { return PhaseEditor.Id; }
    }

    private int DefaultRegionId
    {
        get { return PhaseEditor.DefaultRegionId; }
        set { PhaseEditor.DefaultRegionId = value; }
    }

    private string InteractableName
    {
        get { return PhaseEditor.InteractableName; }
        set { PhaseEditor.InteractableName = value; }
    }

    private string LocationName
    {
        get { return PhaseEditor.LocationName; }
        set { PhaseEditor.LocationName = value; }
    }

    private string ModelIconPath
    {
        get { return PhaseEditor.ModelIconPath; }
        set { PhaseEditor.ModelIconPath = value; }
    }

    private int DefaultTime
    {
        get { return PhaseEditor.DefaultTime; }
        set { PhaseEditor.DefaultTime = value; }
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
        InitializeSceneButton();
    }

    private void InitializeSceneButton()
    {
        //Assign data
        sceneButton.DataElement.Id = DataEditor.ElementData.Id;
        sceneButton.DataElement.Data = DataEditor.Data;

        sceneButton.DataElement.Path = SegmentController.EditorController.PathController.route.path;

        sceneButton.InitializeElement();
    }

    public void OpenSegment() { }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
