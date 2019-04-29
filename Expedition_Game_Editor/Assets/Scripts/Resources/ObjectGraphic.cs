using UnityEngine;
using System.Collections;
using System.Linq;

public class ObjectGraphic : MonoBehaviour
{
    public Route route = new Route();

    public int id;

    //Don't know about that "data" variable, old
    //public ObjectGraphicData data;
    public ObjectProperties.Pivot pivot;

    public CameraManager cameraManager { get; set; }

    public void InitializeGraphic(CameraManager cameraManager)
    {
        this.cameraManager = cameraManager;

        cameraManager.backgroundManager.SetBackground(this);
    }

    public GeneralData GeneralData()
    {
        return route.data.element.Cast<GeneralData>().FirstOrDefault();
    }
}
