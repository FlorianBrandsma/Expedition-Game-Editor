using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ElementController : MonoBehaviour, IDataController
{
    [HideInInspector]
    public List<int> idList = new List<int>();
    public int temp_id_count;

    public SearchParameters searchParameters;

    private ElementManager elementManager       = new ElementManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Element; } }
    public ICollection DataList                 { get; set; }

    public SearchParameters SearchParameters
    {
        get { return searchParameters; }
        set { searchParameters = value; }
    }

    public void InitializeController()
    {
        for (int id = 1; id <= temp_id_count; id++)
        {
            if(!searchParameters.exclusedIdList.Contains(id))
                idList.Add(id);
        }

        GetData(idList);
    }

    public void GetData(List<int> idList)
    {
        DataList = elementManager.GetElementDataElements(idList);

        var elementDataElements = DataList.Cast<ElementDataElement>();

        //elementDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //elementDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }

    public void ReplaceData(IEnumerable dataElement)
    {
        var elementDataElement = dataElement.Cast<ElementDataElement>().FirstOrDefault();

        var replacementList = DataList.Cast<ElementDataElement>().ToList();
        replacementList[elementDataElement.index] = elementDataElement;

        DataList = replacementList; 
    }
}
