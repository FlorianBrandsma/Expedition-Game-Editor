using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SearchController : MonoBehaviour
{
    private Data data;

    public RectTransform referenceArea;
    public bool enableIcon;

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public void InitializeController()
    {
        CloseController();

        data = SegmentController.editorController.pathController.route.data;
        
        if (data.SearchParameters == null) return;
        
        var searchParameters = data.SearchParameters.Cast<SearchParameters>().FirstOrDefault();

        InitializeListProperties(searchParameters.elementType);
        InitializeDataController(searchParameters.dataType);
    }

    private void InitializeListProperties(Enums.ElementType elementType)
    {
        switch(elementType)
        {
            case Enums.ElementType.Panel: case Enums.ElementType.CompactPanel:

                PanelProperties panelProperties = gameObject.AddComponent<PanelProperties>();
                panelProperties.elementType = elementType;
                panelProperties.referenceArea = referenceArea;
                panelProperties.icon = enableIcon;

                break;

            case Enums.ElementType.Tile: case Enums.ElementType.CompactTile:

                TileProperties tileProperties = gameObject.AddComponent<TileProperties>();
                tileProperties.elementType = elementType;

                break;

            default: Debug.Log("CASE MISSING:" + elementType); break;
        }
    }

    private void InitializeDataController(Enums.DataType dataType)
    {
        switch (dataType)
        {
            case Enums.DataType.Icon:

                IconController iconController = gameObject.AddComponent<IconController>();
                iconController.SearchParameters = data.SearchParameters;

                break;

            case Enums.DataType.ObjectGraphic:

                ObjectGraphicController objectGraphicController = gameObject.AddComponent<ObjectGraphicController>();
                objectGraphicController.SearchParameters = data.SearchParameters;

                break;

            case Enums.DataType.Element:

                ElementController elementController = gameObject.AddComponent<ElementController>();
                elementController.SearchParameters = data.SearchParameters;

                break;

            default: Debug.Log("CASE MISSING:" + dataType); break;
        }
    }

    public void CloseController()
    {
        DestroyImmediate((UnityEngine.Object)GetComponent<IProperties>());
        DestroyImmediate((UnityEngine.Object)GetComponent<IDataController>());
    }
}
