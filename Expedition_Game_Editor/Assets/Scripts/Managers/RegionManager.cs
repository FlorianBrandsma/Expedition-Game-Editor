using System.Collections.Generic;

public class Region
{
    public GeneralData data { get; set; }
    private List<TerrainData> terrain_data = new List<TerrainData>();

    public Region(GeneralData new_data)
    {
        data = new_data;

        GetTerrainData();
    }

    private void GetTerrainData()
    {
        //TEMPORARY
        //terrain_data.data = new GeneralData();
        //terrain_data.id_count = 4;

        //terrain_data.GetData("sql");

        //Debug.Log(terrain_data.list.Count);
    }
}

public class Terrain
{
    private List<TileData> tile_data;
}

public class Tile
{
    private DataList[] object_data;
}

public class RegionManager
{
    public enum Type
    {
        Base,
        Phase,
        Task
    }

    
    //RegionPool?
    //TerrainPool?

    //TilePool
    //ObjectPool

    static float tile_size = 31.75f;
}
