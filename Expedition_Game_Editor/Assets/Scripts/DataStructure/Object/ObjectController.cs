using UnityEngine;
using System.Collections;
using System.Linq;

public class ObjectController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.Object; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    ObjectManager objectManager = new ObjectManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = objectManager.GetObjectDataElements(this);

        var objectDataElements = data_list.Cast<ObjectDataElement>();

        Debug.Log(segmentController.editorController.pathController.route.data.element.Cast<ElementDataElement>().FirstOrDefault().object_id);
        //objectDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //objectDataElements[0].Update();
    }
}
