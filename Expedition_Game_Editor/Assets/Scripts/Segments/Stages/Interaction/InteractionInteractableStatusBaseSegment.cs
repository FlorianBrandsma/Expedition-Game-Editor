using UnityEngine;
using UnityEngine.UI;

public class InteractionInteractableStatusBaseSegment : MonoBehaviour, ISegment
{
    public RawImage icon;

    public Text nameText;
    public Text stateText;
    public Text locationText;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private InteractionEditor InteractionEditor { get { return (InteractionEditor)DataEditor; } }

    #region Data properties
    private string ModelIconPath
    {
        get { return InteractionEditor.ModelIconPath; }
        set { InteractionEditor.ModelIconPath = value; }
    }

    private string InteractableName
    {
        get { return InteractionEditor.InteractableName; }
        set { InteractionEditor.InteractableName = value; }
    }

    private string LocationName
    {
        get { return InteractionEditor.LocationName; }
        set { InteractionEditor.LocationName = value; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        icon.texture = Resources.Load<Texture2D>(ModelIconPath);

        nameText.text = InteractableName;
        locationText.text = LocationName;
    }

    public void InitializeData() { }

    public void OpenSegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(IElementData elementData) { }
}
