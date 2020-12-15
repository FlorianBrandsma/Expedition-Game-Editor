using UnityEngine;
using System.Collections.Generic;

public class CameraFilterOverlay : MonoBehaviour, IOverlay
{
    private ExCameraFilter cameraFilterPrefab;

    private List<ExCameraFilter> cameraFilterList = new List<ExCameraFilter>();

    private OverlayManager OverlayManager { get { return GetComponent<OverlayManager>(); } }

    private void Awake()
    {
        cameraFilterPrefab = Resources.Load<ExCameraFilter>("Elements/UI/CameraFilter");
    }

    public void InitializeOverlay(IDisplayManager displayManager) { }

    public void ActivateOverlay(IOrganizer organizer) { }

    public ExCameraFilter SpawnCameraFilter(Texture2D texture)
    {
        var cameraFilter = (ExCameraFilter)PoolManager.SpawnObject(cameraFilterPrefab);

        if (!cameraFilterList.Contains(cameraFilter))
            cameraFilterList.Add(cameraFilter);

        cameraFilter.transform.SetParent(OverlayManager.layer[1], false);

        cameraFilter.RawImage.texture = texture;

        cameraFilter.gameObject.SetActive(true);

        return cameraFilter;
    }

    public void UpdateOverlay() { }

    public void SetOverlay() { }

    public void CloseOverlay()
    {
        CloseCameraFilters();

        DestroyImmediate(this);
    }

    public void CloseCameraFilters()
    {
        cameraFilterList.ForEach(x => PoolManager.ClosePoolObject(x.GetComponent<IPoolable>()));

        cameraFilterList.Clear();
    }
}
