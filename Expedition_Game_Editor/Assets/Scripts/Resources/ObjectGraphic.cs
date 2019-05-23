using UnityEngine;
using System.Collections;
using System.Linq;

namespace Source
{
    public class ObjectGraphic : MonoBehaviour
    {
        public Route route = new Route();

        public int id;

        public string name;
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
            return route.data.ElementData.Cast<GeneralData>().FirstOrDefault();
        }
    }
}

