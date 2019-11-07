using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneDataManager
{
    private SceneController sceneController;

    private SceneDataElement basicSceneData;

    private Enums.RegionType regionType;

    private DataManager dataManager = new DataManager();

    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TileSetData> tileSetDataList;
    private List<DataManager.TerrainData> terrainDataList;
    private List<DataManager.TerrainTileData> terrainTileDataList;
    private List<DataManager.SceneObjectData> sceneObjectDataList;
    private List<DataManager.InteractionData> interactionDataList;
    
    private List<DataManager.SceneInteractableData> sceneInteractableDataList;
    private List<DataManager.PhaseInteractableData> phaseInteractableDataList = new List<DataManager.PhaseInteractableData>();
    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;

    private List<DataManager.ObjectiveData> objectiveDataList;
    private List<DataManager.QuestData> questDataList;
    
    public SceneDataManager(SceneController sceneController)
    {
        this.sceneController = sceneController;
    }

    public List<IDataElement> GetSceneDataElements(IEnumerable searchParameters)
    {
        regionType = ((RegionDataElement)sceneController.SegmentController.Path.FindLastRoute(Enums.DataType.Region).data.dataElement).type;

        var searchData = searchParameters.Cast<Search.Scene>().FirstOrDefault();

        GetRegionData(searchData);
        GetTileSetData();
        GetTerrainData();
        GetTerrainTileData();
        GetInteractionData(searchData);
        GetSceneObjectData(searchData);

        GetSceneInteractableData();
        GetInteractableData();
        GetObjectGraphicData();

        GetObjectiveData();
        GetQuestData();

        GetPhaseInteractableData();

        basicSceneData = GetBasicSceneData();

        var sceneStartPosition = GetSceneStartPosition();
        
        var list = (
            from regionData in regionDataList
            join tileSetData in tileSetDataList on regionData.tileSetId equals tileSetData.Id
            select new SceneDataElement
            {
                DataType = Enums.DataType.Scene,

                Id = regionData.Id,
                Index = regionData.Index,

                regionSize = regionData.regionSize,
                terrainSize = regionData.terrainSize,

                tileSetName = tileSetData.name,
                tileSize = tileSetData.tileSize,

                startPosition = sceneStartPosition,

                terrainDataList = (
                from terrainData in terrainDataList
                select new SceneDataElement.TerrainData()
                {
                    DataType = Enums.DataType.Terrain,

                    Id = terrainData.Id,
                    Index = terrainData.Index,

                    terrainTileDataList = (
                    from terrainTileData in terrainTileDataList
                    where terrainTileData.terrainId == terrainData.Id
                    select new TerrainTileDataElement()
                    {
                        DataType = Enums.DataType.TerrainTile,

                        Id = terrainTileData.Id,
                        Index = terrainTileData.Index,

                        TileId = terrainTileData.tileId

                    }).OrderBy(x => x.Index).ToList(),

                    sceneInteractableDataList = regionType != Enums.RegionType.Interaction ? (
                    from sceneInteractableData  in sceneInteractableDataList
                    join interactionData        in interactionDataList      on sceneInteractableData.Id             equals interactionData.sceneInteractableId
                    join interactableData       in interactableDataList     on sceneInteractableData.interactableId equals interactableData.Id
                    join objectGraphicData      in objectGraphicDataList    on interactableData.objectGraphicId     equals objectGraphicData.Id
                    where interactionData.terrainId == terrainData.Id
                    select new SceneInteractableDataElement()
                    {
                        DataType = Enums.DataType.SceneInteractable,

                        Id = sceneInteractableData.Id,
                        terrainTileId = interactionData.terrainTileId,

                        positionX = interactionData.positionX,
                        positionY = interactionData.positionY,
                        positionZ = interactionData.positionZ,

                        rotationX = interactionData.rotationX,
                        rotationY = interactionData.rotationY,
                        rotationZ = interactionData.rotationZ,

                        scaleMultiplier = interactionData.scaleMultiplier,

                        animation = interactionData.animation,

                        objectGraphicId = objectGraphicData.Id,
                        objectGraphicPath = objectGraphicData.path,

                        startPosition = sceneStartPosition

                    }).ToList() : new List<SceneInteractableDataElement>(),

                    interactionDataList = regionType == Enums.RegionType.Interaction ? (
                    from interactionData        in interactionDataList
                    join sceneInteractableData  in sceneInteractableDataList    on interactionData.sceneInteractableId  equals sceneInteractableData.Id
                    join interactableData       in interactableDataList         on sceneInteractableData.interactableId equals interactableData.Id
                    join objectGraphicData      in objectGraphicDataList        on interactableData.objectGraphicId     equals objectGraphicData.Id

                    join leftJoin in (from objectiveData in objectiveDataList
                                      select new { objectiveData }) on sceneInteractableData.objectiveId equals leftJoin.objectiveData.Id into objectiveData

                    join leftJoin in (from phaseInteractableData in phaseInteractableDataList
                                      select new { phaseInteractableData }) on sceneInteractableData.Id equals leftJoin.phaseInteractableData.sceneInteractableId into phaseInteractableData

                    where interactionData.terrainId == terrainData.Id
                    select new InteractionDataElement()
                    {
                        DataType = Enums.DataType.Interaction,

                        Id = interactionData.Id,
                        SceneInteractableId = interactionData.sceneInteractableId,
                        TerrainTileId = interactionData.terrainTileId,

                        PositionX = interactionData.positionX,
                        PositionY = interactionData.positionY,
                        PositionZ = interactionData.positionZ,

                        RotationX = interactionData.rotationX,
                        RotationY = interactionData.rotationY,
                        RotationZ = interactionData.rotationZ,

                        ScaleMultiplier = interactionData.scaleMultiplier,

                        Animation = interactionData.animation,

                        objectiveId =   objectiveData.FirstOrDefault()          != null ? objectiveData.FirstOrDefault().objectiveData.Id : 0,
                        questId =       objectiveData.FirstOrDefault()          != null ? objectiveData.FirstOrDefault().objectiveData.questId :
                                        phaseInteractableData.FirstOrDefault()  != null ? phaseInteractableData.FirstOrDefault().phaseInteractableData.questId : 0,

                        objectGraphicId = objectGraphicData.Id,
                        objectGraphicPath = objectGraphicData.path,

                        startPosition = sceneStartPosition

                    }).ToList() : new List<InteractionDataElement>(),
                    
                    sceneObjectDataList = (
                    from sceneObjectData    in sceneObjectDataList
                    join objectGraphicData  in objectGraphicDataList on sceneObjectData.objectGraphicId equals objectGraphicData.Id
                    where sceneObjectData.terrainId == terrainData.Id
                    select new SceneObjectDataElement()
                    {
                        DataType = Enums.DataType.SceneObject,

                        Id = sceneObjectData.Id,
                        TerrainTileId = sceneObjectData.terrainTileId,

                        PositionX = sceneObjectData.positionX,
                        PositionY = sceneObjectData.positionY,
                        PositionZ = sceneObjectData.positionZ,

                        RotationX = sceneObjectData.rotationX,
                        RotationY = sceneObjectData.rotationY,
                        RotationZ = sceneObjectData.rotationZ,

                        ScaleMultiplier = sceneObjectData.scaleMultiplier,

                        Animation = sceneObjectData.animation,

                        ObjectGraphicId = objectGraphicData.Id,
                        objectGraphicPath = objectGraphicData.path,

                        startPosition = sceneStartPosition

                    }).ToList()

                }).OrderBy(x => x.Index).ToList()

            }).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    private SceneDataElement GetBasicSceneData()
    {
        var sceneData = (from regionData in regionDataList
                         join tileSetData in tileSetDataList on regionData.tileSetId equals tileSetData.Id
                         select new SceneDataElement
                         {
                             regionSize = regionData.regionSize,
                             terrainSize = regionData.terrainSize,
                             tileSize = tileSetData.tileSize

                         }).FirstOrDefault();

        return sceneData;
    }

    private Vector2 GetSceneStartPosition()
    {
        var sceneStartPosition = new Vector2(-(basicSceneData.regionSize * basicSceneData.terrainSize * basicSceneData.tileSize / 2),
                                              (basicSceneData.regionSize * basicSceneData.terrainSize * basicSceneData.tileSize / 2));

        return sceneStartPosition;
    }

    internal void GetRegionData(Search.Scene searchData)
    {
        var searchParameters = new Search.Region();
        searchParameters.id = searchData.regionId;

        regionDataList = dataManager.GetRegionData(searchParameters);
    }

    internal void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.tileSetId).Distinct().ToList();

        tileSetDataList = dataManager.GetTileSetData(tileSetSearchParameters);
    }

    internal void GetTerrainData()
    {
        var terrainSearchParameters = new Search.Terrain();
        terrainSearchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = dataManager.GetTerrainData(terrainSearchParameters);
    }

    internal void GetTerrainTileData()
    {
        var terrainTileSearchParameters = new Search.TerrainTile();
        terrainTileSearchParameters.terrainId = terrainDataList.Select(x => x.Id).Distinct().ToList();

        terrainTileDataList = dataManager.GetTerrainTileData(terrainTileSearchParameters);
    }

    internal void GetInteractionData(Search.Scene searchData)
    {
        var interactionSearchParameters = new Search.Interaction();
        interactionSearchParameters.regionId = searchData.regionId;
        interactionSearchParameters.objectiveId = searchData.objectiveId;
        
        interactionDataList = dataManager.GetInteractionData(interactionSearchParameters);

        if(regionType != Enums.RegionType.Interaction)
            interactionDataList = interactionDataList.OrderBy(x => x.Index).GroupBy(x => x.sceneInteractableId).Select(x => x.FirstOrDefault()).ToList();
    }

    internal void GetSceneObjectData(Search.Scene searchData)
    {
        var sceneObjectSearchParameters = new Search.SceneObject();
        sceneObjectSearchParameters.regionId = searchData.id;

        sceneObjectDataList = dataManager.GetSceneObjectData(sceneObjectSearchParameters);
    }

    internal void GetSceneInteractableData()
    {
        var sceneInteractableSearchParameters = new Search.SceneInteractable();
        sceneInteractableSearchParameters.id = interactionDataList.Select(x => x.sceneInteractableId).Distinct().ToList();

        sceneInteractableDataList = dataManager.GetSceneInteractableData(sceneInteractableSearchParameters);
    }

    internal void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = sceneInteractableDataList.Select(x => x.interactableId).Distinct().ToList();

        interactableDataList = dataManager.GetInteractableData(interactableSearchParameters);
    }

    internal void GetObjectGraphicData()
    {
        var objectGraphicSearchParameters = new Search.ObjectGraphic();
        objectGraphicSearchParameters.id = interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList().Union(sceneObjectDataList.Select(x => x.objectGraphicId).Distinct().ToList()).Distinct().ToList();

        objectGraphicDataList = dataManager.GetObjectGraphicData(objectGraphicSearchParameters);
    }

    internal void GetObjectiveData()
    {
        var objectiveSearchParameters = new Search.Objective();
        objectiveSearchParameters.id = sceneInteractableDataList.Select(x => x.objectiveId).Distinct().ToList();

        objectiveDataList = dataManager.GetObjectiveData(objectiveSearchParameters);
    }

    internal void GetQuestData()
    {
        var questSearchParameters = new Search.Quest();
        questSearchParameters.id = objectiveDataList.Select(x => x.questId).Distinct().ToList();

        questDataList = dataManager.GetQuestData(questSearchParameters);
    }

    internal void GetPhaseInteractableData()
    {
        var phaseInteractableSearchParameters = new Search.PhaseInteractable();
        phaseInteractableSearchParameters.sceneInteractableId = sceneInteractableDataList.Select(x => x.Id).Distinct().ToList();

        phaseInteractableDataList = dataManager.GetPhaseInteractableData(phaseInteractableSearchParameters);
    }
}