using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PhaseGeneralDefaultValuesSegment : MonoBehaviour, ISegment
{
    public Text nameText;
    public Text stateText;
    public Text locationText;
    public Text timeText;

    public EditorElement editButton;
    public RawImage buttonIcon;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private PhaseEditor PhaseEditor             { get { return (PhaseEditor)DataEditor; } }

    #region Data properties
    private int Id
    {
        get { return PhaseEditor.Id; }
    }

    private int DefaultRegionId
    {
        get { return PhaseEditor.DefaultRegionId; }
        set { PhaseEditor.DefaultRegionId = value; }
    }

    private string InteractableName
    {
        get { return PhaseEditor.InteractableName; }
        set { PhaseEditor.InteractableName = value; }
    }

    private string LocationName
    {
        get { return PhaseEditor.LocationName; }
        set { PhaseEditor.LocationName = value; }
    }

    private string ModelIconPath
    {
        get { return PhaseEditor.ModelIconPath; }
        set { PhaseEditor.ModelIconPath = value; }
    }

    private int DefaultTime
    {
        get { return PhaseEditor.DefaultTime; }
        set { PhaseEditor.DefaultTime = value; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment()
    {
        nameText.text = InteractableName;
        locationText.text = LocationName;
        timeText.text = TimeManager.FormatTime(DefaultTime);

        InitializeEditButton();
    }

    private void InitializeEditButton()
    {
        //Get region data
        var searchProperties = new SearchProperties(Enums.DataType.Region);
        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();
        
        searchParameters.phaseId = new List<int>() { Id };
        searchParameters.type = Enums.RegionType.Controllable;

        SegmentController.DataController.GetData(searchProperties);
      
        var regionData = SegmentController.DataController.Data.dataList.Where(x => x.Id == DefaultRegionId).FirstOrDefault();

        //Assign data
        editButton.DataElement.Id = regionData.Id;
        editButton.DataElement.Data = SegmentController.DataController.Data;

        editButton.DataElement.Path = SegmentController.EditorController.PathController.route.path;

        editButton.InitializeElement();

        buttonIcon.texture = Resources.Load<Texture2D>(ModelIconPath);
    }

    public void OpenSegment() { }

    public void SetSearchResult(IElementData elementData) { }

    public void CloseSegment() { }
}