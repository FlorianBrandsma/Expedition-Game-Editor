using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SearchController : MonoBehaviour
{
    private Data data;

    public RectTransform referenceArea;

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public void InitializeController()
    {
        CloseController();

        data = SegmentController.EditorController.PathController.route.data;
        
        if (data == null) return;
        
        InitializeListProperties(data.searchProperties);
        InitializeDataController(data.searchProperties);
    }

    private void InitializeListProperties(SearchProperties searchParameters)
    {
        switch(searchParameters.elementType)
        {
            case Enums.ElementType.Panel: case Enums.ElementType.CompactPanel:

                PanelProperties panelProperties = gameObject.AddComponent<PanelProperties>();
                panelProperties.elementType     = searchParameters.elementType;
                panelProperties.referenceArea   = referenceArea;
                panelProperties.iconType        = searchParameters.iconType;

                break;

            case Enums.ElementType.Tile: case Enums.ElementType.CompactTile:

                TileProperties tileProperties   = gameObject.AddComponent<TileProperties>();
                tileProperties.elementType      = searchParameters.elementType;

                break;

            default: Debug.Log("CASE MISSING:" + searchParameters.elementType); break;
        }
    }

    private void InitializeDataController(SearchProperties searchParameters)
    {
        IDataController dataController = null;

        switch (searchParameters.dataType)
        {
            case Enums.DataType.Tile:               dataController = gameObject.AddComponent<TileDataController>();                 break;
            case Enums.DataType.Icon:               dataController = gameObject.AddComponent<IconDataController>();                 break;
            case Enums.DataType.Model:              dataController = gameObject.AddComponent<ModelDataController>();                break;
            case Enums.DataType.Interactable:       dataController = gameObject.AddComponent<InteractableDataController>();         break;
            case Enums.DataType.WorldInteractable:  dataController = gameObject.AddComponent<WorldInteractableDataController>();    break;
            case Enums.DataType.Region:             dataController = gameObject.AddComponent<RegionDataController>();               break;
            case Enums.DataType.SceneActor:         dataController = gameObject.AddComponent<SceneActorDataController>();           break;

            default: Debug.Log("CASE MISSING:" + searchParameters.dataType); break;
        }

        dataController.SearchProperties = data.searchProperties;
    }

    public void CloseController()
    {
        DestroyImmediate((Object)GetComponent<IProperties>());
        DestroyImmediate((Object)GetComponent<IDataController>());
    }
}
