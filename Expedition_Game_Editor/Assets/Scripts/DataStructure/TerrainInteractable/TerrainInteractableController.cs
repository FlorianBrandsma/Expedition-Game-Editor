using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainInteractableController : MonoBehaviour, IDataController
{
    public Search.Interactable searchParameters;

    public TerrainInteractableDataManager terrainInteractableDataManager = new TerrainInteractableDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.TerrainInteractable; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Interactable>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        terrainInteractableDataManager.InitializeManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return terrainInteractableDataManager.GetTerrainInteractableDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, SelectionElement.Data resultData)
    {
        var searchElementData = (TerrainInteractableDataElement)searchElement.data.dataElement;

        var terrainInteractableDataElement = DataList.Cast<TerrainInteractableDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.dataController.DataType)
        {
            case Enums.DataType.Interactable:

                var resultElementData = (InteractableDataElement)resultData.dataElement;

                terrainInteractableDataElement.InteractableId = resultElementData.id;
                terrainInteractableDataElement.interactableName = resultElementData.Name;
                terrainInteractableDataElement.objectGraphicIconPath = resultElementData.objectGraphicIconPath;

                break;
        }

        searchElement.data.dataElement = terrainInteractableDataElement;
    }

    public void ToggleElement(IDataElement dataElement) { }
}
