using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RegionOrganizer : MonoBehaviour, IOrganizer
{
    private CameraManager manager;
    private Route route;
    private RegionProperties properties;

    //private Region region;

    //private RegionData region_data;
    //private DataList terrain_data;
    //private DataList tile_data;
    //private DataList[] object_data;

    public void InitializeOrganizer()
    {
        manager = GetComponent<CameraManager>();
        route = manager.cameraProperties.route;
    }

    public void SetProperties()
    {
        properties = manager.cameraProperties.GetComponent<RegionProperties>();
    }

    public void GetData()
    {
        //Combine
        //manager.properties.dataList.GetData("sql");

        GetRegionData();

        //Temporary: camera only needs one data bit to render
        //manager.properties.dataList.SetData(region.data);

        //GetRegion
        //GetTerrains(region)
        //GetTiles(terrain)
        //GetObjects(tile)

        //Region
        //>Terrain
        //>>Tile
        //>>>Objects[] (pool)
        //>>>>Object
    }

    private void GetRegionData()
    {
        //region = new Region(route.GeneralData());

        //var test = new RegionData();

        //region_data.GetData("sql");

        //GetTerrainData();
    }

    //private void GetTerrainData()
    //{
    //    terrain_data.GetData("sql");

    //    GetTileData();
    //}

    //private void GetTileData()
    //{
    //    tile_data.GetData("sql");

    //    GetObjectData();
    //}

    //private void GetObjectData()
    //{
    //    //object_data.GetData("sql");
    //}

    public void UpdateData()
    {
        SetData();
    }

    public void SetData()
    {
        //local_data_list = data_list;

        //SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/Button");

        //for (int i = 0; i < local_data_list.Count; i++)
        //{
        //    SelectionElement element = listManager.SpawnElement(element_list, element_prefab, local_data_list[i]);
        //    element_list_local.Add(element);

        //    listManager.element_list.Add(element);

        //    string label = listManager.listProperties.dataList.data.table + " " + i;
        //    element.GetComponent<EditorButton>().label.text = label;

        //    //Debugging
        //    element.name = label;

        //    SetElement(element);
        //}
    }

    public void ResetData(ICollection filter)
    {
        CloseOrganizer();
        SetData();
    }
    
    public void ClearOrganizer() { }

    public void CloseOrganizer()
    {
        //listManager.ResetElement(element_list_local);

        DestroyImmediate(this);
    }
}
