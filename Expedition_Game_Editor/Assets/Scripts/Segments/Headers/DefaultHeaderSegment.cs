using UnityEngine;
using UnityEngine.UI;

public class DefaultHeaderSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public ExIndexSwitch indexSwitch;
    public DataElement dataElement;
    public ExInputText inputText;
    public Text idText;
    #endregion

    #region Data Variables
    private int id;
    private int index;
    private new string name;
    #endregion

    #region Data Properties
    public string Name
    {
        get { return name; }
        set
        {
            name = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Chapter:

                    var chapterData = (ChapterElementData)DataEditor.Data.elementData;
                    chapterData.Name = value;

                    break;

                case Enums.DataType.Phase:

                    var phaseData = (PhaseElementData)DataEditor.Data.elementData;
                    phaseData.Name = value;
                    
                    break;

                case Enums.DataType.Quest:

                    var questData = (QuestElementData)DataEditor.Data.elementData;
                    questData.Name = value;

                    break;

                case Enums.DataType.Objective:

                    var objectiveData = (ObjectiveElementData)DataEditor.Data.elementData;
                    objectiveData.Name = value;

                    break;

                case Enums.DataType.Task:

                    var taskData = (TaskElementData)DataEditor.Data.elementData;
                    taskData.Name = value;
                    
                    break;

                case Enums.DataType.Region:

                    var regionData = (RegionElementData)DataEditor.Data.elementData;
                    regionData.Name = value;

                    break;
                    
                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }
    #endregion

    #region Data Methods
    public void UpdateName()
    {
        Name = inputText.InputField.text;
        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        inputText.InitializeElement();
    }

    public void InitializeData()
    {
        InitializeDependencies();

        if (DataEditor.Loaded) return;
        
        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Chapter:    InitializeChapterData();    break;
            case Enums.DataType.Phase:      InitializePhaseData();      break;
            case Enums.DataType.Quest:      InitializeQuestData();      break;
            case Enums.DataType.Objective:  InitializeObjectiveData();  break;
            case Enums.DataType.Task:       InitializeTaskData();       break;
            case Enums.DataType.Region:     InitializeRegionData();     break;
            case Enums.DataType.Terrain:    InitializeTerrainData();    break;

            default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
        }

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, index);
    }

    private void InitializeChapterData()
    {
        var chapterData = (ChapterElementData)DataEditor.Data.elementData;

        id      = chapterData.Id;
        index   = chapterData.Index;
        name    = chapterData.Name;
    }

    private void InitializePhaseData()
    {
        var phaseData = (PhaseElementData)DataEditor.Data.elementData;

        id      = phaseData.Id;
        index   = phaseData.Index;
        name    = phaseData.Name;
    }

    private void InitializeQuestData()
    {
        var questData = (QuestElementData)DataEditor.Data.elementData;

        id      = questData.Id;
        index   = questData.Index;
        name    = questData.Name;
    }

    private void InitializeObjectiveData()
    {
        var objectiveData = (ObjectiveElementData)DataEditor.Data.elementData;

        id      = objectiveData.Id;
        index   = objectiveData.Index;
        name    = objectiveData.Name;
    }

    private void InitializeTaskData()
    {
        var objectiveData = (TaskElementData)DataEditor.Data.elementData;

        id      = objectiveData.Id;
        index   = objectiveData.Index;
        name    = objectiveData.Name;
    }

    private void InitializeRegionData()
    {
        var regionData = (RegionElementData)DataEditor.Data.elementData;

        id      = regionData.Id;
        index   = regionData.Index;
        name    = regionData.Name;
    }

    private void InitializeTerrainData()
    {
        var terrainData = (TerrainElementData)DataEditor.Data.elementData;

        id      = terrainData.Id;
        name    = terrainData.Name;
    }

    public void OpenSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Activate();

        idText.text = id.ToString();

        inputText.InputField.text = name;

        gameObject.SetActive(true);
    }

    public void CloseSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Deactivate();

        gameObject.SetActive(false);
    }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}
