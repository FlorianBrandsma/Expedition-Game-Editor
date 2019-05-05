using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TaskController : MonoBehaviour, IDataController
{
    public int temp_id_count;

    public SearchParameters searchParameters;

    private TaskManager taskManager             = new TaskManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Task; } }
    public ICollection DataList                 { get; set; }

    public SearchParameters SearchParameters
    {
        get { return searchParameters; }
        set { searchParameters = value; }
    }

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        DataList = taskManager.GetTaskDataElements(this);

        var taskDataElements = DataList.Cast<TaskDataElement>();

        //taskDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //taskDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }

    public void ReplaceData(IEnumerable dataElement)
    {

    }
}