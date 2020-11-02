using UnityEngine;
using UnityEngine.UI;

public class TitleHeaderSegment : MonoBehaviour, ISegment
{
    public Text idText;
    public Text headerText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    #region Data properties
    private int Id
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.SceneShot:
                    return ((SceneShotEditor)DataEditor).Id;

                case Enums.DataType.ChapterSave:
                    return ((ChapterSaveEditor)DataEditor).Id;

                case Enums.DataType.PhaseSave:
                    return ((PhaseSaveEditor)DataEditor).Id;

                case Enums.DataType.QuestSave:
                    return ((QuestSaveEditor)DataEditor).Id;

                case Enums.DataType.ObjectiveSave:
                    return ((ObjectiveSaveEditor)DataEditor).Id;

                case Enums.DataType.TaskSave:
                    return ((TaskSaveEditor)DataEditor).Id;

                case Enums.DataType.InteractionSave:
                    return ((InteractionSaveEditor)DataEditor).Id;
                    
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
                case Enums.DataType.SceneShot:
                    return ((SceneShotEditor)DataEditor).Description;

                case Enums.DataType.ChapterSave:
                    return ((ChapterSaveEditor)DataEditor).Name;

                case Enums.DataType.PhaseSave:
                    return ((PhaseSaveEditor)DataEditor).Name;

                case Enums.DataType.QuestSave:
                    return ((QuestSaveEditor)DataEditor).Name;

                case Enums.DataType.ObjectiveSave:
                    return ((ObjectiveSaveEditor)DataEditor).Name;

                case Enums.DataType.TaskSave:
                    return ((TaskSaveEditor)DataEditor).Name;

                case Enums.DataType.InteractionSave:
                    return ((InteractionSaveEditor)DataEditor).Name;

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
    }

    public void InitializeSegment() { }

    public void OpenSegment()
    {
        idText.text = Id.ToString();
        headerText.text = Name;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }
}
