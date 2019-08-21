using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneController : MonoBehaviour, IDataController
{
    public Search.Region searchParameters;

    private SceneDataManager sceneDataManager   = new SceneDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Region; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Region>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        sceneDataManager.InitializeManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return sceneDataManager.GetSceneDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, SelectionElement.Data resultData) { }

    public void ToggleElement(IDataElement dataElement) { }
}
