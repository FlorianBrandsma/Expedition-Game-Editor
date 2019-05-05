using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SearchController : MonoBehaviour
{
    private Data data;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType DataType              { get; set; }

    public void InitializeController()
    {
        data = SegmentController.editorController.pathController.route.data;

        if (data.element.GetType() != typeof(SearchData[])) return;

        SearchData searchData = data.element.Cast<SearchData>().FirstOrDefault();

        DataType = data.controller.DataType;

        switch (DataType)
        {
            case Enums.DataType.ObjectGraphic:

                gameObject.AddComponent<ObjectGraphicController>();
                GetComponent<ObjectGraphicController>().temp_id_count = 5;

                break;

            case Enums.DataType.Element:

                gameObject.AddComponent<ElementController>();
                GetComponent<ElementController>().temp_id_count = 4;
                break;
        }

        GetComponent<IDataController>().SearchParameters = searchData.searchParameters.Copy();
        GetComponent<IDataController>().SearchParameters.searching = true;
    }

    public void CloseController()
    {
        DestroyImmediate((UnityEngine.Object)GetComponent<IDataController>());
    }
}
