using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class DefaultHeaderSegment : MonoBehaviour, ISegment
{
    private SegmentController segmentController;
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

            switch (DataEditor.Data.DataController.DataType)
            {
                case Enums.DataType.Chapter:

                    var chapterData = DataEditor.Data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();
                    chapterData.Name = value;

                    break;

                case Enums.DataType.Phase:

                    var phaseData = DataEditor.Data.ElementData.Cast<PhaseDataElement>().FirstOrDefault();
                    phaseData.Name = value;

                    break;

                case Enums.DataType.Quest:

                    var questData = DataEditor.Data.ElementData.Cast<QuestDataElement>().FirstOrDefault();
                    questData.Name = value;

                    break;

                case Enums.DataType.Objective:

                    var objectiveData = DataEditor.Data.ElementData.Cast<ObjectiveDataElement>().FirstOrDefault();
                    objectiveData.Name = value;

                    break;

                case Enums.DataType.Region:

                    var regionData = DataEditor.Data.ElementData.Cast<RegionDataElement>().FirstOrDefault();
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
    public void InitializeSegment()
    {
        segmentController = GetComponent<SegmentController>();
        DataEditor = segmentController.editorController.pathController.dataEditor;

        switch (DataEditor.Data.DataController.DataType)
        {
            case Enums.DataType.Chapter:    InitializeChapterData();    break;
            case Enums.DataType.Phase:      InitializePhaseData();      break;
            case Enums.DataType.Quest:      InitializeQuestData();      break;
            case Enums.DataType.Objective:  InitializeObjectiveData();  break;
            case Enums.DataType.Region:     InitializeRegionData();     break;
            case Enums.DataType.Terrain:    InitializeTerrainData();    break;
            default:                        Debug.Log("CASE MISSING");  break;
        }

        if (indexSwitch != null)
            indexSwitch.InitializeSwitch(this, index, DataEditor.Data.DataController.DataList.Count - 1); 
    }

    private void InitializeChapterData()
    {
        var chapterData = DataEditor.Data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();

        id = chapterData.id;
        index = chapterData.Index;
        name = chapterData.Name;
    }

    private void InitializePhaseData()
    {
        var phaseData = DataEditor.Data.ElementData.Cast<PhaseDataElement>().FirstOrDefault();

        id = phaseData.id;
        index = phaseData.Index;
        name = phaseData.Name;
        icon = phaseData.icon;
    }

    private void InitializeQuestData()
    {
        var questData = DataEditor.Data.ElementData.Cast<QuestDataElement>().FirstOrDefault();

        id = questData.id;
        index = questData.Index;
        name = questData.Name;
    }

    private void InitializeObjectiveData()
    {
        var objectiveData = DataEditor.Data.ElementData.Cast<ObjectiveDataElement>().FirstOrDefault();

        id = objectiveData.id;
        index = objectiveData.Index;
        name = objectiveData.Name;
    }

    private void InitializeRegionData()
    {
        var regionData = DataEditor.Data.ElementData.Cast<RegionDataElement>().FirstOrDefault();

        id = regionData.id;
        index = regionData.Index;
        name = regionData.Name;
    }

    private void InitializeTerrainData()
    {
        var terrainData = DataEditor.Data.ElementData.Cast<TerrainDataElement>().FirstOrDefault();

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
