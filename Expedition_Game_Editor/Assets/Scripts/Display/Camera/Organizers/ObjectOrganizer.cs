using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class ObjectOrganizer : MonoBehaviour, IOrganizer
{
    private CameraManager cameraManager { get { return GetComponent<CameraManager>(); } }
    private ObjectProperties properties;

    static public List<ObjectGraphic> graphic_list = new List<ObjectGraphic>();

    private IDataController dataController;
    private List<GeneralData> generalData_list;

    public void InitializeOrganizer()
    {
        dataController = cameraManager.cameraProperties.segmentController.dataController;
    }

    public void SetProperties()
    {
        properties = cameraManager.cameraProperties.GetComponent<ObjectProperties>();
    }

    public void SetData()
    {
        if(dataController != null)
        {
            SetData(dataController.data_list);
        } else {

            SetData(cameraManager.cameraProperties.segmentController.editorController.pathController.route.data.element);
        }
    }

    public void SetData(IEnumerable list)
    {
        generalData_list = list.Cast<GeneralData>().ToList();

        foreach (var data in list)
        {
            IEnumerable new_data = new[] { data };

            ObjectGraphic graphic_prefab = LoadGraphic(new_data);

            if (graphic_prefab == null) continue;

            ObjectGraphic graphic = cameraManager.SpawnGraphic(graphic_list, graphic_prefab);
            cameraManager.graphic_list.Add(graphic);

            graphic.route.data = new Data(dataController, new_data);

            //Debugging
            GeneralData generalData = (GeneralData)data;
            graphic.name = generalData.table + generalData.id;
            //

            SetGraphic(graphic);
        }
    }

    public void UpdateData()
    {
        SetData();
    }

    public void ResetData(ICollection filter)
    {
        CloseOrganizer();
        SetData();
    }

    private ObjectGraphic LoadGraphic(IEnumerable data)
    {
        ObjectGraphic new_graphic = null;

        var general_data = data.Cast<GeneralData>().FirstOrDefault();

        switch (general_data.table)
        {
            case "Item":

                ItemDataElement itemDataElement = data.Cast<ItemDataElement>().FirstOrDefault();
                new_graphic = Resources.Load<ObjectGraphic>("Objects/" + itemDataElement.object_name);

                break;

            case "Element":

                ElementDataElement elementDataElement = data.Cast<ElementDataElement>().FirstOrDefault();
                new_graphic = Resources.Load<ObjectGraphic>("Objects/" + elementDataElement.object_name);

                break;
        }

        return new_graphic;
    }

    void SetGraphic(ObjectGraphic graphic)
    {
        //graphic.SetGraphic();
        
        graphic.transform.localPosition = new Vector2(  graphic.transform.localPosition.x, 
                                                        properties.pivot_position[(int)graphic.pivot]);

        graphic.gameObject.SetActive(true);
    }

    private void CloseCamera()
    {
        cameraManager.ResetGraphics();
    }

    public void CloseOrganizer()
    {
        CloseCamera();

        DestroyImmediate(this);
    }
}
