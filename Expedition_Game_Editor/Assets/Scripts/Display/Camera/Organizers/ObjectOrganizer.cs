using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class ObjectOrganizer : MonoBehaviour, IOrganizer
{
    private CameraManager cameraManager { get { return GetComponent<CameraManager>(); } }
    private ObjectProperties properties;

    static public List<ObjectGraphic> graphicList = new List<ObjectGraphic>();

    private IDataController dataController;

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
            SetData(dataController.dataList);
    }

    public void SetData(IEnumerable list)
    {
        foreach (var data in list)
        {
            IEnumerable new_data = new[] { data };

            ObjectGraphic graphic_prefab = Resources.Load<ObjectGraphic>("Objects/" + new_data.Cast<ObjectGraphicDataElement>().FirstOrDefault().name);

            if (graphic_prefab == null) continue;

            ObjectGraphic graphic = cameraManager.SpawnGraphic(graphicList, graphic_prefab);
            cameraManager.graphicList.Add(graphic);

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

    void SetGraphic(ObjectGraphic graphic)
    {
        graphic.transform.localPosition = new Vector2(  graphic.transform.localPosition.x, 
                                                        properties.pivotPosition[(int)graphic.pivot]);

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
