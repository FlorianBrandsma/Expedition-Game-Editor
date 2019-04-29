using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TaskController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.Task; } }

    public ICollection dataList { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    TaskManager taskManager = new TaskManager();

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        dataList = taskManager.GetTaskDataElements(this);

        var taskDataElements = dataList.Cast<TaskDataElement>();

        //taskDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //taskDataElements[0].Update();
    }
}