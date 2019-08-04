using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class DefaultHeaderSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    #region UI

    public IndexSwitch indexSwitch;
    public SelectionElement selectionElement;
    public InputField inputField;
    public Text idText;

    #endregion

    #region Data Variables

    private int id;
    private int index;
    private string name;
    private string icon;

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

                    var chapterData = (ChapterDataElement)DataEditor.Data.dataElement;
                    chapterData.Name = value;

                    break;

                case Enums.DataType.Phase:

                    var phaseData = (PhaseDataElement)DataEditor.Data.dataElement;
                    phaseData.Name = value;

                    break;

                case Enums.DataType.Quest:

                    var questData = (QuestDataElement)DataEditor.Data.dataElement;
                    questData.Name = value;

                    break;

                case Enums.DataType.Objective:

                    var objectiveData = (ObjectiveDataElement)DataEditor.Data.dataElement;
                    objectiveData.Name = value;

                    break;

                case Enums.DataType.Region:

                    var regionData = (RegionDataElement)DataEditor.Data.dataElement;
                    regionData.Name = value;

                    break;
                    
                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }
    #endregion

    #region Data Methods
    public void UpdateName()
    {
        Name = inputField.text;
        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeDependencies();

        InitializeData();

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, index, DataEditor.Data.dataController.DataList.Count - 1); 
    }

    public void InitializeData()
    {
        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Chapter:    InitializeChapterData();    break;
            case Enums.DataType.Phase:      InitializePhaseData();      break;
            case Enums.DataType.Quest:      InitializeQuestData();      break;
            case Enums.DataType.Objective:  InitializeObjectiveData();  break;
            case Enums.DataType.Region:     InitializeRegionData();     break;
            case Enums.DataType.Terrain:    InitializeTerrainData();    break;
            default: Debug.Log("CASE MISSING"); break;
        }
    }

    private void InitializeChapterData()
    {
        var chapterData = (ChapterDataElement)DataEditor.Data.dataElement;

        id = chapterData.id;
        index = chapterData.Index;
        name = chapterData.Name;
    }

    private void InitializePhaseData()
    {
        var phaseData = (PhaseDataElement)DataEditor.Data.dataElement;

        id = phaseData.id;
        index = phaseData.Index;
        name = phaseData.Name;
    }

    private void InitializeQuestData()
    {
        var questData = (QuestDataElement)DataEditor.Data.dataElement;

        id = questData.id;
        index = questData.Index;
        name = questData.Name;
    }

    private void InitializeObjectiveData()
    {
        var objectiveData = (ObjectiveDataElement)DataEditor.Data.dataElement;

        id = objectiveData.id;
        index = objectiveData.Index;
        name = objectiveData.Name;
    }

    private void InitializeRegionData()
    {
        var regionData = (RegionDataElement)DataEditor.Data.dataElement;

        id = regionData.id;
        index = regionData.Index;
        name = regionData.Name;
    }

    private void InitializeTerrainData()
    {
        var terrainData = (TerrainDataElement)DataEditor.Data.dataElement;

        id = terrainData.id;
        name = terrainData.Name;
    }

    public void OpenSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Activate();

        idText.text = id.ToString();

        inputField.text = name;

        gameObject.SetActive(true);
    }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {
        if (indexSwitch != null)
            indexSwitch.Deactivate();

        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
    #endregion
}
