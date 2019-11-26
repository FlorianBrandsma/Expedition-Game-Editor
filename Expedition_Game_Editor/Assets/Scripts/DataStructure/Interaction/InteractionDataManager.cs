using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractionDataManager : IDataManager
{
    public IDataController DataController { get; set; }
    private List<InteractionData> interactionDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.SceneInteractableData> sceneInteractableDataList;
    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    private List<DataManager.RegionData> regionDataList;

    public InteractionDataManager(IDataController dataController)
    {
        DataController = dataController;
    }

    public List<IDataElement> GetDataElements(IEnumerable searchParameters)
    {
        var objectiveSearchData = searchParameters.Cast<Search.Interaction>().FirstOrDefault();

        GetInteractionData(objectiveSearchData);

        GetSceneInteractableData();
        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        GetRegionData();

        var list = (from interactionData        in interactionDataList

                    join sceneInteractableData  in sceneInteractableDataList    on interactionData.sceneInteractableId  equals sceneInteractableData.Id
                    join interactableData       in interactableDataList         on sceneInteractableData.interactableId equals interactableData.Id
                    join objectGraphicData      in objectGraphicDataList        on interactableData.objectGraphicId     equals objectGraphicData.Id
                    join iconData               in iconDataList                 on objectGraphicData.iconId             equals iconData.Id

                    join leftJoin in (from regionData in regionDataList
                                      select new { regionData }) on interactionData.regionId equals leftJoin.regionData.Id into regionData

                    select new InteractionDataElement()
                    {
                        DataType = Enums.DataType.Interaction,

                        Id = interactionData.Id,
                        Index = interactionData.Index,

                        SceneInteractableId = interactionData.sceneInteractableId,
                        RegionId = interactionData.regionId,
                        TerrainId = interactionData.terrainId,
                        TerrainTileId = interactionData.terrainTileId,

                        Description = interactionData.description,

                        PositionX = interactionData.positionX,
                        PositionY = interactionData.positionY,
                        PositionZ = interactionData.positionZ,

                        RotationX = interactionData.rotationX,
                        RotationY = interactionData.rotationY,
                        RotationZ = interactionData.rotationZ,

                        ScaleMultiplier = interactionData.scaleMultiplier,

                        Animation = interactionData.animation,

                        regionName  = regionData.FirstOrDefault() != null ? regionData.FirstOrDefault().regionData.name : "",
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetInteractionData(Search.Interaction searchParameters)
    {
        interactionDataList = new List<InteractionData>();

        foreach(Fixtures.Interaction interaction in Fixtures.interactionList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(interaction.Id)) continue;
            if (searchParameters.objectiveId.Count > 0 && !searchParameters.objectiveId.Contains(interaction.objectiveId)) continue;
            if (searchParameters.sceneInteractableId.Count > 0 && !searchParameters.sceneInteractableId.Contains(interaction.sceneInteractableId)) continue;

            var interactionData = new InteractionData();

            interactionData.Id = interaction.Id;
            interactionData.Index = interaction.Index;

            interactionData.objectiveId = interaction.objectiveId;
            interactionData.sceneInteractableId = interaction.sceneInteractableId;
            interactionData.regionId = interaction.regionId;
            interactionData.terrainId = interaction.terrainId;
            interactionData.terrainTileId = interaction.terrainTileId;
            
            interactionData.description = interaction.description;

            interactionData.positionX = interaction.positionX;
            interactionData.positionY = interaction.positionY;
            interactionData.positionZ = interaction.positionZ;

            interactionData.rotationX = interaction.rotationX;
            interactionData.rotationY = interaction.rotationY;
            interactionData.rotationZ = interaction.rotationZ;

            interactionData.scaleMultiplier = interaction.scaleMultiplier;

            interactionData.animation = interaction.animation;

            interactionDataList.Add(interactionData);
        }
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

        objectGraphicSearchParameters.id = interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList();

        objectGraphicDataList = dataManager.GetObjectGraphicData(objectGraphicSearchParameters);
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.id = interactionDataList.Select(x => x.regionId).Distinct().ToList();

        regionDataList = dataManager.GetRegionData(searchParameters);
    }

    internal class InteractionData : GeneralData
    {
        public int objectiveId;
        public int sceneInteractableId;
        public int regionId;
        public int terrainId;
        public int terrainTileId;

        public string description;

        public float positionX;
        public float positionY;
        public float positionZ;

        public int rotationX;
        public int rotationY;
        public int rotationZ;

        public float scaleMultiplier;

        public int animation;
    }
}
