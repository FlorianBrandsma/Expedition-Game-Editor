using UnityEngine;
using UnityEngine.UI;

public class WorldElementHeaderSegment : MonoBehaviour, ISegment
{
    public RawImage icon;
    public Text headerText;
    public Text idText;

    private string modelIconPath;
    private string header;
    private int id;

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        InitializeDependencies();

        if (DataEditor.Loaded) return;
    }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.WorldObject:            InitializeWorldObjectData();            break;
            case Enums.DataType.Phase:                  InitializePhaseData();                  break;
            case Enums.DataType.InteractionDestination: InitializeInteractionDestinationData(); break;

            default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
        }
        
        icon.texture = Resources.Load<Texture2D>(modelIconPath);
        headerText.text = header;
        idText.text = id.ToString();

        gameObject.SetActive(true);
    }
    
    private void InitializeWorldObjectData()
    {
        var worldObjectData = (WorldObjectElementData)DataEditor.ElementData;

        id = worldObjectData.Id;
        header = worldObjectData.ModelName;
        modelIconPath = worldObjectData.ModelIconPath;

        headerText.fontSize = StyleManager.headerFontSize;
        headerText.resizeTextMaxSize = StyleManager.headerFontSize;
    }

    private void InitializePhaseData()
    {
        var phaseData = (PhaseElementData)DataEditor.ElementData;

        id = phaseData.PartyMemberId;
        header = phaseData.InteractableName;
        modelIconPath = phaseData.ModelIconPath;

        headerText.fontSize = StyleManager.headerFontSize;
        headerText.resizeTextMaxSize = StyleManager.headerFontSize;
    }

    private void InitializeInteractionDestinationData()
    {
        var interactionDestinationData = (InteractionDestinationElementData)DataEditor.ElementData;

        id = interactionDestinationData.Id;
        header = interactionDestinationData.InteractableName;
        modelIconPath = interactionDestinationData.ModelIconPath;

        headerText.fontSize = StyleManager.headerFontSize;
        headerText.resizeTextMaxSize = StyleManager.headerFontSize;
    }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }

    public void SetSearchResult(DataElement dataElement) { }
}