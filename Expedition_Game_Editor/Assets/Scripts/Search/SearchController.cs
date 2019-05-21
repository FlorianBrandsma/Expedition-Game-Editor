using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SearchController : MonoBehaviour
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public void InitializeController()
    {
        CloseController();

        Data data = SegmentController.editorController.pathController.route.data;
        
        if (data.SearchParameters == null) return;
        
        var searchParameters = data.SearchParameters.Cast<SearchParameters>().FirstOrDefault();

        switch (searchParameters.dataType)
        {
            case Enums.DataType.ObjectGraphic:

                ObjectGraphicController objectGraphicController = gameObject.AddComponent<ObjectGraphicController>();
                objectGraphicController.SearchParameters = data.SearchParameters;

                break;

            case Enums.DataType.Element:

                ElementController elementController = gameObject.AddComponent<ElementController>();
                elementController.SearchParameters = data.SearchParameters;

                break;
        }
    }

    public void CloseController()
    {
        DestroyImmediate((UnityEngine.Object)GetComponent<IDataController>());
    }
}
