using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IconController : MonoBehaviour, IDataController
{
    public Search.Icon searchParameters;

    private IconDataManager iconDataManager = new IconDataManager();

    public IDisplay Display { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType { get { return Enums.DataType.Icon; } }
    public ICollection DataList { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Icon>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        iconDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = iconDataManager.GetIconDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {

    }
}