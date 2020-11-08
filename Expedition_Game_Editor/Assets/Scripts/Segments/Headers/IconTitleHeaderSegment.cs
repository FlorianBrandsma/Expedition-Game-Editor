using UnityEngine;
using UnityEngine.UI;

public class IconTitleHeaderSegment : MonoBehaviour, ISegment
{
    public RawImage icon;
    public Text headerText;
    public Text idText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public InteractableSaveEditor InteractableSaveEditor { get { return (InteractableSaveEditor)DataEditor; } }

    #region Data properties
    private int Id
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:
                    return ((SceneActorEditor)DataEditor).Id;

                case Enums.DataType.SceneProp:
                    return ((ScenePropEditor)DataEditor).Id;

                case Enums.DataType.InteractableSave:
                    return ((InteractableSaveEditor)DataEditor).Id;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
    }

    private string Title
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:
                    return ((SceneActorEditor)DataEditor).InteractableName;

                case Enums.DataType.SceneProp:
                    return ((ScenePropEditor)DataEditor).ModelName;

                case Enums.DataType.InteractableSave:
                    return ((InteractableSaveEditor)DataEditor).InteractableName;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
    }

    private string IconPath
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneActor:
                    return ((SceneActorEditor)DataEditor).ModelIconPath;

                case Enums.DataType.SceneProp:
                    return ((ScenePropEditor)DataEditor).ModelIconPath;

                case Enums.DataType.InteractableSave:
                    return ((InteractableSaveEditor)DataEditor).ModelIconPath;

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
        icon.texture = Resources.Load<Texture2D>(IconPath);
        headerText.text = Title;
        idText.text = Id.ToString();

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }
}
