using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class SceneActorDataManager
{
    private static List<SceneActorBaseData> sceneActorDataList;
    private static List<WorldInteractableBaseData> worldInteractableDataList;
    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(Search.SceneActor searchParameters)
    {
        GetSceneActorData(searchParameters);

        if (searchParameters.includeAddElement)
            sceneActorDataList.Add(DefaultData(searchParameters.sceneId.First()));

        if (searchParameters.includeRemoveElement)
            sceneActorDataList.Add(new SceneActorBaseData());

        if (sceneActorDataList.Count == 0) return new List<IElementData>();
        
        GetWorldInteractableData();

        GetInteractableData();
        GetModelData();
        GetIconData();

        var list = (from sceneActorData in sceneActorDataList

                    join leftJoin in (from worldInteractableData    in worldInteractableDataList
                                      join interactableData         in interactableDataList on worldInteractableData.InteractableId equals interactableData.Id
                                      join modelData                in modelDataList        on interactableData.ModelId             equals modelData.Id
                                      join iconData                 in iconDataList         on modelData.IconId                     equals iconData.Id
                                      select new { worldInteractableData, interactableData, modelData, iconData }) on sceneActorData.WorldInteractableId equals leftJoin.worldInteractableData.Id into worldInteractableData

                    select new SceneActorElementData()
                    {
                        Id = sceneActorData.Id,

                        SceneId = sceneActorData.SceneId,
                        WorldInteractableId = sceneActorData.WorldInteractableId,

                        SpeechMethod = sceneActorData.SpeechMethod,
                        SpeechText = sceneActorData.SpeechText,
                        ShowTextBox = sceneActorData.ShowTextBox,

                        TargetSceneActorId = sceneActorData.TargetSceneActorId,

                        ChangePosition = sceneActorData.ChangePosition,
                        FreezePosition = sceneActorData.FreezePosition,

                        PositionX = sceneActorData.PositionX,
                        PositionY = sceneActorData.PositionY,
                        PositionZ = sceneActorData.PositionZ,

                        ChangeRotation = sceneActorData.ChangeRotation,
                        FaceTarget = sceneActorData.FaceTarget,

                        RotationX = sceneActorData.RotationX,
                        RotationY = sceneActorData.RotationY,
                        RotationZ = sceneActorData.RotationZ,

                        ModelId = worldInteractableData.FirstOrDefault() != null ? worldInteractableData.FirstOrDefault().modelData.Id : 0,

                        ModelPath = worldInteractableData.FirstOrDefault() != null ? worldInteractableData.FirstOrDefault().modelData.Path : "",
                        ModelIconPath = worldInteractableData.FirstOrDefault() != null ? worldInteractableData.FirstOrDefault().iconData.Path : "",

                        InteractableName = worldInteractableData.FirstOrDefault() != null ? worldInteractableData.FirstOrDefault().interactableData.Name : "",

                        Height = worldInteractableData.FirstOrDefault() != null ? worldInteractableData.FirstOrDefault().modelData.Height : 0,
                        Width = worldInteractableData.FirstOrDefault() != null ? worldInteractableData.FirstOrDefault().modelData.Width : 0,
                        Depth = worldInteractableData.FirstOrDefault() != null ? worldInteractableData.FirstOrDefault().modelData.Depth : 0,

                        Scale = worldInteractableData.FirstOrDefault() != null ? worldInteractableData.FirstOrDefault().interactableData.Scale : 0,

                        SpeechTextLimit = ScenarioManager.SpeechCharacterLimit(sceneActorData.ShowTextBox)

                    }).OrderBy(x => x.Id).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);
        
        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static SceneActorElementData DefaultData(int sceneId)
    {
        var defaultPosition = Vector3.zero;

        if (EditorWorldOrganizer.instance != null)
        {
            defaultPosition = EditorWorldOrganizer.instance.AddElementDefaultPosition();
        }

        return new SceneActorElementData()
        {
            Id = -1,

            WorldInteractableId = -1,
            SceneId = sceneId,

            Scale = 1,

            PositionX = defaultPosition.x,
            PositionY = defaultPosition.y,
            PositionZ = defaultPosition.z,

            SpeechText = ""
        };
    }

    public static void SetDefaultAddValues(List<SceneActorElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();
        
        addElementData.ExecuteType = Enums.ExecuteType.Add;
    }

    private static void GetSceneActorData(Search.SceneActor searchParameters)
    {
        sceneActorDataList = new List<SceneActorBaseData>();
        
        foreach (SceneActorBaseData sceneActor in Fixtures.sceneActorList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(sceneActor.Id))            continue;
            if (searchParameters.excludeId.Count    > 0 && searchParameters.excludeId.Contains(sceneActor.Id))      continue;
            if (searchParameters.sceneId.Count      > 0 && !searchParameters.sceneId.Contains(sceneActor.SceneId))  continue;

            sceneActorDataList.Add(sceneActor);
        }
    }

    private static void GetWorldInteractableData()
    {
        var searchParameters = new Search.WorldInteractable();
        searchParameters.id = sceneActorDataList.Select(x => x.WorldInteractableId).Distinct().ToList();

        worldInteractableDataList = DataManager.GetWorldInteractableData(searchParameters);
    }

    private static void GetInteractableData()
    {
        var searchParameters = new Search.Interactable();
        searchParameters.id = worldInteractableDataList.Select(x => x.InteractableId).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(searchParameters);
    }

    private static void GetModelData()
    {
        var searchParameters = new Search.Model();
        searchParameters.id = interactableDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }
    
    public static void AddData(SceneActorElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.sceneActorList.Count > 0 ? (Fixtures.sceneActorList[Fixtures.sceneActorList.Count - 1].Id + 1) : 1;
            Fixtures.sceneActorList.Add(((SceneActorData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(SceneActorElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.sceneActorList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedWorldInteractableId)
            {
                data.WorldInteractableId = elementData.WorldInteractableId;
            }

            if (elementData.ChangedSpeechMethod)
            {
                data.SpeechMethod = elementData.SpeechMethod;
            }

            if (elementData.ChangedSpeechText)
            {
                data.SpeechText = elementData.SpeechText;
            }

            if (elementData.ChangedShowTextBox)
            {
                data.ShowTextBox = elementData.ShowTextBox;
            }

            if (elementData.ChangedTargetSceneActorId)
            {
                data.TargetSceneActorId = elementData.TargetSceneActorId;
            }

            if (elementData.ChangedChangePosition)
            {
                data.ChangePosition = elementData.ChangePosition;
            }

            if (elementData.ChangedFreezePosition)
            {
                data.FreezePosition = elementData.FreezePosition;
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

            if (elementData.ChangedChangeRotation)
            {
                data.ChangeRotation = elementData.ChangeRotation;
            }

            if (elementData.ChangedFaceTarget)
            {
                data.FaceTarget = elementData.FaceTarget;
            }

            if (elementData.ChangedRotationX)
            {
                data.RotationX = elementData.RotationX;
            }

            if (elementData.ChangedRotationY)
            {
                data.RotationY = elementData.RotationY;
            }

            if (elementData.ChangedRotationZ)
            {
                data.RotationZ = elementData.RotationZ;
            }

            elementData.SetOriginalValues();

        } else { } 
    }

    public static void RemoveData(SceneActorElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveReferences(elementData, dataRequest);

            Fixtures.sceneActorList.RemoveAll(x => x.Id == elementData.Id);

        } else {

            RemoveReferences(elementData, dataRequest);
        }
    }

    private static void RemoveReferences(SceneActorElementData elementData, DataRequest dataRequest)
    {
        RemoveSceneShotPositionTargetReference(elementData, dataRequest);
        RemoveSceneShotRotationTargetReference(elementData, dataRequest);
        RemoveSceneActorTargetReference(elementData, dataRequest);
    }

    private static void RemoveSceneShotPositionTargetReference(SceneActorElementData elementData, DataRequest dataRequest)
    {
        var sceneShotSearchParameters = new Search.SceneShot()
        {
            positionTargetSceneActorId = new List<int>() { elementData.Id }
        };

        var sceneShotDataList = DataManager.GetSceneShotData(sceneShotSearchParameters);

        sceneShotDataList.ForEach(sceneShotData =>
        {
            var sceneShotElementData = new SceneShotElementData()
            {
                Id = sceneShotData.Id,
                PositionTargetSceneActorId = elementData.Id
            };

            sceneShotElementData.SetOriginalValues();

            sceneShotElementData.PositionTargetSceneActorId = 0;

            sceneShotElementData.Update(dataRequest);
        });
    }

    private static void RemoveSceneShotRotationTargetReference(SceneActorElementData elementData, DataRequest dataRequest)
    {
        var sceneShotSearchParameters = new Search.SceneShot()
        {
            rotationTargetSceneActorId = new List<int>() { elementData.Id }
        };

        var sceneShotDataList = DataManager.GetSceneShotData(sceneShotSearchParameters);

        sceneShotDataList.ForEach(sceneShotData =>
        {
            var sceneShotElementData = new SceneShotElementData()
            {
                Id = sceneShotData.Id,
                RotationTargetSceneActorId = elementData.Id
            };

            sceneShotElementData.SetOriginalValues();

            sceneShotElementData.RotationTargetSceneActorId = 0;

            sceneShotElementData.Update(dataRequest);
        });
    }

    private static void RemoveSceneActorTargetReference(SceneActorElementData elementData, DataRequest dataRequest)
    {
        var sceneActorSearchParameters = new Search.SceneActor()
        {
            targetSceneActorId = new List<int>() { elementData.Id }
        };

        var sceneActorDataList = DataManager.GetSceneActorData(sceneActorSearchParameters);

        sceneActorDataList.ForEach(sceneActorData =>
        {
            var sceneActorElementData = new SceneActorElementData()
            {
                Id = sceneActorData.Id,
                TargetSceneActorId = elementData.Id
            };

            sceneActorElementData.SetOriginalValues();

            sceneActorElementData.TargetSceneActorId = 0;

            sceneActorElementData.Update(dataRequest);
        });
    }
}
