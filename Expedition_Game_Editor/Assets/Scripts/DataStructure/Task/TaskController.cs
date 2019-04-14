using UnityEngine;
using System.Collections;
using System.Linq;

public class TaskController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.Task; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    TaskManager taskManager = new TaskManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = taskManager.GetTaskDataElements(this);

        var TaskDataElements = data_list.Cast<TaskDataElement>();

        //TaskDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //TaskDataElements[0].Update();
    }
}
