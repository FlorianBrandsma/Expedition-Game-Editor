using UnityEngine;
using UnityEngine.UI;

public class InteractionInteractableStatusBaseSegment : MonoBehaviour, ISegment
{
    private InteractionElementData InteractionData { get { return (InteractionElementData)DataEditor.ElementData; } }
    
    public RawImage icon;

    public Text nameText;
    public Text stateText;
    public Text locationText;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        icon.texture = Resources.Load<Texture2D>(InteractionData.ModelIconPath);

        nameText.text = InteractionData.InteractableName;
        locationText.text = InteractionData.LocationName;
    }

    public void InitializeData() { }

    public void OpenSegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
