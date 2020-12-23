using UnityEngine;
using UnityEngine.UI;

public class WorldElementHeaderSegment : MonoBehaviour, ISegment
{
    public RawImage icon;
    public Text headerText;
    public Text idText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    #region Data properties
    private int Id
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.WorldObject:
                    return ((WorldObjectEditor)DataEditor).Id;

                case Enums.DataType.Phase:
                    return ((PhaseEditor)DataEditor).Id;

                case Enums.DataType.InteractionDestination:
                    return ((InteractionDestinationEditor)DataEditor).Id;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
    }

    private string Name
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.WorldObject:
                    return ((WorldObjectEditor)DataEditor).ModelName;

                case Enums.DataType.Phase:
                    return ((PhaseEditor)DataEditor).InteractableName;

                case Enums.DataType.InteractionDestination:
                    return ((InteractionDestinationEditor)DataEditor).InteractableName;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
    }

    private string ModelIconPath
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.WorldObject:
                    return ((WorldObjectEditor)DataEditor).ModelIconPath;

                case Enums.DataType.Phase:
                    return ((PhaseEditor)DataEditor).ModelIconPath;

                case Enums.DataType.InteractionDestination:
                    return ((InteractionDestinationEditor)DataEditor).ModelIconPath;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
    }
    #endregion

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
        icon.texture = Resources.Load<Texture2D>(ModelIconPath);
        headerText.text = Name;
        idText.text = Id.ToString();

        headerText.fontSize = StyleManager.headerFontSize;
        headerText.resizeTextMaxSize = StyleManager.headerFontSize;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }
}