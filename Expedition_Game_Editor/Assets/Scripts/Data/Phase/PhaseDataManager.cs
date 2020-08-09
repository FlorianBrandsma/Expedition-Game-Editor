using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<PhaseData> phaseDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ChapterData> chapterDataList;
    private List<DataManager.PartyMemberData> partyMemberDataList;
    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TerrainData> terrainDataList;
    private List<DataManager.TileSetData> tileSetDataList;

    public PhaseDataManager(PhaseController phaseController)
    {
        DataController = phaseController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Phase>().First();

        GetPhaseData(searchParameters);

        if (phaseDataList.Count == 0) return new List<IElementData>();

        GetChapterData();
        GetPartyMemberData();
        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        GetRegionData();
        GetTileSetData();
        GetTerrainData();

        var list = (from phaseData      in phaseDataList
                    join chapterData    in chapterDataList on phaseData.chapterId equals chapterData.Id

                    join leftJoin in (from partyMemberData      in partyMemberDataList
                                      join interactableData     in interactableDataList     on partyMemberData.interactableId   equals interactableData.Id
                                      join objectGraphicData    in objectGraphicDataList    on interactableData.objectGraphicId equals objectGraphicData.Id
                                      join iconData             in iconDataList             on objectGraphicData.iconId         equals iconData.Id
                                      select new { partyMemberData, interactableData, objectGraphicData, iconData }) on chapterData.Id equals leftJoin.partyMemberData.chapterId into partyMemberData

                    select new PhaseElementData()
                    {
                        Id = phaseData.Id,
                        Index = phaseData.Index,

                        ChapterId = phaseData.chapterId,

                        Name = phaseData.name,

                        DefaultRegionId = phaseData.defaultRegionId,

                        DefaultPositionX = phaseData.defaultPositionX,
                        DefaultPositionY = phaseData.defaultPositionY,
                        DefaultPositionZ = phaseData.defaultPositionZ,

                        DefaultRotationX = phaseData.defaultRotationX,
                        DefaultRotationY = phaseData.defaultRotationY,
                        DefaultRotationZ = phaseData.defaultRotationZ,

                        scaleMultiplier = partyMemberData.First().interactableData.scaleMultiplier,

                        DefaultTime = phaseData.defaultTime,

                        PublicNotes = phaseData.publicNotes,
                        PrivateNotes = phaseData.privateNotes,

                        terrainTileId = TerrainTileId(phaseData.defaultRegionId, phaseData.defaultPositionX, phaseData.defaultPositionZ),

                        partyMemberId = partyMemberData.First().partyMemberData.Id,
                        
                        objectGraphicId = partyMemberData.First().objectGraphicData.Id,
                        objectGraphicPath = partyMemberData.First().objectGraphicData.path,

                        objectGraphicIconPath = partyMemberData.First().iconData.path,
                        
                        height = partyMemberData.First().objectGraphicData.height,
                        width = partyMemberData.First().objectGraphicData.width,
                        depth = partyMemberData.First().objectGraphicData.depth,

                        interactableName = partyMemberData.First().interactableData.name,
                        locationName = LocationName(phaseData.defaultRegionId, phaseData.defaultPositionX, phaseData.defaultPositionY, phaseData.defaultPositionZ)
                        
                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private void GetPhaseData(Search.Phase searchParameters)
    {
        phaseDataList = new List<PhaseData>();

        foreach(Fixtures.Phase phase in Fixtures.phaseList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(phase.Id)) continue;
            if (searchParameters.chapterId.Count    > 0 && !searchParameters.chapterId.Contains(phase.chapterId)) continue;

            var phaseData = new PhaseData();

            phaseData.Id = phase.Id;
            phaseData.Index = phase.Index;

            phaseData.chapterId = phase.chapterId;

            phaseData.name = phase.name;

            phaseData.defaultRegionId = phase.defaultRegionId;

            phaseData.defaultPositionX = phase.defaultPositionX;
            phaseData.defaultPositionY = phase.defaultPositionY;
            phaseData.defaultPositionZ = phase.defaultPositionZ;

            phaseData.defaultRotationX = phase.defaultRotationX;
            phaseData.defaultRotationY = phase.defaultRotationY;
            phaseData.defaultRotationZ = phase.defaultRotationZ;

            phaseData.defaultTime = phase.defaultTime;

            phaseData.publicNotes = phase.publicNotes;
            phaseData.privateNotes = phase.privateNotes;

            phaseDataList.Add(phaseData);
        }
    }

    internal void GetChapterData()
    {
        var chapterSearchParameters = new Search.Chapter();

        chapterSearchParameters.id = phaseDataList.Select(x => x.chapterId).Distinct().ToList();

        chapterDataList = dataManager.GetChapterData(chapterSearchParameters);
    }

    internal void GetPartyMemberData()
    {
        var partyMemberSearchParameters = new Search.PartyMember();

        partyMemberSearchParameters.chapterId = chapterDataList.Select(x => x.Id).Distinct().ToList();

        partyMemberDataList = dataManager.GetPartyMemberData(partyMemberSearchParameters);
    }

    internal void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = partyMemberDataList.Select(x => x.interactableId).Distinct().ToList();

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
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = objectGraphicDataList.Select(x => x.iconId).Distinct().ToList();

        iconDataList = dataManager.GetIconData(iconSearchParameters);
    }

    internal void GetRegionData()
    {
        var regionSearchParameters = new Search.Region();
        regionSearchParameters.id = phaseDataList.Select(x => x.defaultRegionId).Distinct().ToList();

        regionDataList = dataManager.GetRegionData(regionSearchParameters);
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
    
    internal int TerrainTileId(int regionId, float positionX, float positionZ)
    {
        var terrainId = Fixtures.GetTerrain(regionId, positionX, positionZ);

        var terrainTileId = Fixtures.GetTerrainTile(terrainId, positionX, positionZ);

        return terrainTileId;
    }

    internal string LocationName(int regionId, float positionX, float positionY, float positionZ)
    {
        var region = regionDataList.Where(x => x.Id == regionId).First();

        var terrainId = Fixtures.GetTerrain(regionId, positionX, positionZ);

        var terrain = terrainDataList.Where(x => x.Id == terrainId).FirstOrDefault();
        
        return region.name + ", " + terrain.name;
    }

    internal class PhaseData : GeneralData
    {
        public int chapterId;

        public string name;

        public int defaultRegionId;

        public float defaultPositionX;
        public float defaultPositionY;
        public float defaultPositionZ;

        public int defaultRotationX;
        public int defaultRotationY;
        public int defaultRotationZ;

        public int defaultTime;

        public string publicNotes;
        public string privateNotes;
    }
}
