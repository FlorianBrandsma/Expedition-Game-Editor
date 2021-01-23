using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class PlayerSaveDataManager
{
    public static PlayerSaveElementData DefaultData(int saveId, int gameId)
    {
        //Chapter
        var chapterSearchParameters = new Search.Chapter()
        {
            //Game id
        };

        var chapterData = DataManager.GetChapterData(chapterSearchParameters).Where(x => x.Index == 0).First();

        //Phase
        var phaseSearchParameters = new Search.Phase()
        {
            chapterId = new List<int>() { chapterData.Id }
        };

        var phaseData = DataManager.GetPhaseData(phaseSearchParameters).Where(x => x.Index == 0).First();

        //World interactable
        var worldInteractableSearchParameters = new Search.WorldInteractable()
        {
            chapterId = new List<int>() { chapterData.Id }
        };

        var worldInteractableData = DataManager.GetWorldInteractableData(worldInteractableSearchParameters).First();

        return new PlayerSaveElementData()
        {
            Id = -1,

            SaveId = saveId,

            RegionId = phaseData.DefaultRegionId,
            WorldInteractableId = worldInteractableData.Id,

            PositionX = phaseData.DefaultPositionX,
            PositionY = phaseData.DefaultPositionY,
            PositionZ = phaseData.DefaultPositionZ,

            GameTime = phaseData.DefaultTime
        };
    }

    public static void AddData(PlayerSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.playerSaveList.Count > 0 ? (Fixtures.playerSaveList[Fixtures.playerSaveList.Count - 1].Id + 1) : 1;
            Fixtures.playerSaveList.Add(((PlayerSaveData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(PlayerSaveElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.playerSaveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedRegionId)
            {
                data.RegionId = elementData.RegionId;
            }

            if (elementData.ChangedWorldInteractableId)
            {
                data.WorldInteractableId = elementData.WorldInteractableId;
            }

            if (elementData.ChangedPositionX)
            {
                data.PositionX = elementData.PositionX;
            }

            if (elementData.ChangedPositionY)
            {
                data.PositionY = elementData.PositionY;
            }

            if (elementData.ChangedPositionZ)
            {
                data.PositionZ = elementData.PositionZ;
            }

            if (elementData.ChangedGameTime)
            {
                data.GameTime = elementData.GameTime;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    static public void RemoveData(PlayerSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.playerSaveList.RemoveAll(x => x.Id == elementData.Id);
            
        } else { }
    }
    
    public static void UpdateReferences(DataRequest dataRequest)
    {
        //Save
        var saveSearchParameters = new Search.Save();

        var saveDataList = DataManager.GetSaveData(saveSearchParameters);

        if (saveDataList.Count == 0) return;

        //PlayerSave
        var playerSaveSearchParameters = new Search.PlayerSave()
        {
            saveId = saveDataList.Select(x => x.Id).ToList()
        };

        var playerSaveDataList = DataManager.GetPlayerSaveData(playerSaveSearchParameters);

        if (playerSaveDataList.Count == 0) return;

        foreach (SaveBaseData saveData in saveDataList)
        {
            var playerSaveData = playerSaveDataList.Where(x => x.SaveId == saveData.Id).First();

            //ChapterSave
            var chapterSaveSearchParameters = new Search.ChapterSave()
            {
                saveId = new List<int>() { saveData.Id },
                complete = false
            };

            //Get data from chapter save data manager so the elements are sorted by index
            var chapterSaveElementData = (ChapterSaveElementData)ChapterSaveDataManager.GetData(chapterSaveSearchParameters).First();

            if (chapterSaveElementData == null) continue;

            //PhaseSave
            var phaseSaveSearchParameters = new Search.PhaseSave()
            {
                saveId = saveDataList.Select(x => x.Id).ToList(),
                chapterId = new List<int>() { chapterSaveElementData.ChapterId },
                complete = false
            };

            var phaseSaveElementData = (PhaseSaveElementData)PhaseSaveDataManager.GetData(phaseSaveSearchParameters).First();

            //Region
            var regionSearchParameters = new Search.Region()
            {
                phaseId = new List<int>() { phaseSaveElementData.Id }
            };

            var regionDataList = DataManager.GetRegionData(regionSearchParameters);

            //World interactable
            var worldInteractableSearchParameters = new Search.WorldInteractable()
            {
                chapterId = new List<int>() { chapterSaveElementData.ChapterId }
            };

            var worldInteractableDataList = DataManager.GetWorldInteractableData(worldInteractableSearchParameters);
            
            var playerSaveElementData = new PlayerSaveElementData()
            {
                Id = playerSaveData.Id,
                RegionId = playerSaveData.RegionId,
                WorldInteractableId = playerSaveData.WorldInteractableId,

                PositionX = playerSaveData.PositionX,
                PositionY = playerSaveData.PositionY,
                PositionZ = playerSaveData.PositionZ,

                GameTime = playerSaveData.GameTime
            };

            playerSaveElementData.SetOriginalValues();

            //Pick first region if current saved region is not contained in list
            if (!regionDataList.Select(x => x.Id).Contains(playerSaveData.RegionId))
            {
                //Phase
                var phaseSeachParameters = new Search.Phase()
                {
                    id = new List<int>() { phaseSaveElementData.PhaseId }
                };

                var phaseData = DataManager.GetPhaseData(phaseSeachParameters).First();

                playerSaveElementData.RegionId = regionDataList.First().Id;
                
                playerSaveElementData.PositionX = phaseData.DefaultPositionX;
                playerSaveElementData.PositionY = phaseData.DefaultPositionY;
                playerSaveElementData.PositionZ = phaseData.DefaultPositionZ;

                playerSaveElementData.GameTime = phaseData.DefaultTime;
            }

            var filteredWorldInteractableDataList = worldInteractableDataList.Where(x => x.ChapterId == chapterSaveElementData.ChapterId).ToList();

            //Pick first world interactable if current saved world interactable is not contained in list
            if (!filteredWorldInteractableDataList.Select(x => x.Id).Contains(playerSaveData.WorldInteractableId))
                playerSaveElementData.WorldInteractableId = filteredWorldInteractableDataList.Count > 0 ? filteredWorldInteractableDataList.First().Id : 0;

            playerSaveElementData.Update(dataRequest);
        }
    }
}
