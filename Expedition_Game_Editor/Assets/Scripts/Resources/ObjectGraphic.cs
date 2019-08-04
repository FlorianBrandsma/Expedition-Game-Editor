using UnityEngine;
using System.Collections;
using System.Linq;

namespace Source
{
    public class ObjectGraphic : MonoBehaviour
    {
        public Route route = new Route();

        [HideInInspector]
        public int id;

        public ObjectProperties.Pivot pivot;

        public CameraManager cameraManager { get; set; }

        public void InitializeGraphic(CameraManager cameraManager)
        {
            this.cameraManager = cameraManager;

            cameraManager.backgroundManager.SetBackground(this);
        }

        public GeneralData GeneralData()
        {
            return (GeneralData)route.data.dataElement;
        }
    }
}

