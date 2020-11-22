using UnityEngine;
using UnityEngine.UI;

public class InputHeaderSegment : MonoBehaviour, ISegment
{
    public ExIndexSwitch indexSwitch;
    public ExInputText inputText;
    public Text idText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    #region Data properties
    public int Id
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Region:
                    return ((RegionEditor)DataEditor).Id;

                case Enums.DataType.Chapter:
                    return ((ChapterEditor)DataEditor).Id;

                case Enums.DataType.Phase:
                    return ((PhaseEditor)DataEditor).Id;

                case Enums.DataType.Quest:
                    return ((QuestEditor)DataEditor).Id;

                case Enums.DataType.Objective:
                    return ((ObjectiveEditor)DataEditor).Id;

                case Enums.DataType.Task:
                    return ((TaskEditor)DataEditor).Id;

                case Enums.DataType.Scene:
                    return ((SceneEditor)DataEditor).Id;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
    }

    public int Index
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Region:
                    return ((RegionEditor)DataEditor).Index;

                case Enums.DataType.Chapter:
                    return ((ChapterEditor)DataEditor).Index;

                case Enums.DataType.Phase:
                    return ((PhaseEditor)DataEditor).Index;

                case Enums.DataType.Quest:
                    return ((QuestEditor)DataEditor).Index;

                case Enums.DataType.Objective:
                    return ((ObjectiveEditor)DataEditor).Index;

                case Enums.DataType.Task:
                    return ((TaskEditor)DataEditor).Index;

                case Enums.DataType.Scene:
                    return ((SceneEditor)DataEditor).Index;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
    }

    public string Name
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Region:
                    return ((RegionEditor)DataEditor).Name;

                case Enums.DataType.Chapter:
                    return ((ChapterEditor)DataEditor).Name;

                case Enums.DataType.Phase:
                    return ((PhaseEditor)DataEditor).Name;

                case Enums.DataType.Quest:
                    return ((QuestEditor)DataEditor).Name;

                case Enums.DataType.Objective:
                    return ((ObjectiveEditor)DataEditor).Name;

                case Enums.DataType.Task:
                    return ((TaskEditor)DataEditor).Name;

                case Enums.DataType.Scene:
                    return ((SceneEditor)DataEditor).Name;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Region:

                    var regionEditor = (RegionEditor)DataEditor;
                    regionEditor.Name = value;

                    break;

                case Enums.DataType.Chapter:

                    var chapterEditor = (ChapterEditor)DataEditor;
                    chapterEditor.Name = value;

                    break;

                case Enums.DataType.Phase:

                    var phaseEditor = (PhaseEditor)DataEditor;
                    phaseEditor.Name = value;

                    break;

                case Enums.DataType.Quest:

                    var questEditor = (QuestEditor)DataEditor;
                    questEditor.Name = value;

                    break;

                case Enums.DataType.Objective:

                    var objectiveEditor = (ObjectiveEditor)DataEditor;
                    objectiveEditor.Name = value;

                    break;

                case Enums.DataType.Task:

                    var taskEditor = (TaskEditor)DataEditor;
                    taskEditor.Name = value;

                    break;

                case Enums.DataType.Scene:

                    var sceneEditor = (SceneEditor)DataEditor;
                    sceneEditor.Name = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
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
        
        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, Index);
    }

    public void InitializeSegment()
    {
        inputText.InitializeElement();
    }
    
    public void OpenSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Activate();

        idText.text = Id.ToString();

        inputText.InputField.text = Name;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateName()
    {
        Name = inputText.InputField.text;
        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Deactivate();

        gameObject.SetActive(false);
    }
}