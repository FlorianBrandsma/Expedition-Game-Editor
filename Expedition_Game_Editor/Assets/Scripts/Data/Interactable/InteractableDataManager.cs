using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class InteractableDataManager
{
    private static List<InteractableBaseData> interactableDataList;

    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(Search.Interactable searchParameters)
    {
        GetInteractableData(searchParameters);

        if (searchParameters.includeAddElement)
            interactableDataList.Add(DefaultData(searchParameters.type.First()));

        if (searchParameters.includeRemoveElement)
            interactableDataList.Add(new InteractableBaseData());

        if (interactableDataList.Count == 0) return new List<IElementData>();
        
        GetModelData();
        GetIconData();

        var list = (from interactableData   in interactableDataList

                    join leftJoin in (from modelData    in modelDataList
                                      join iconData     in iconDataList on modelData.IconId equals iconData.Id
                                      select new { modelData, iconData }) on interactableData.ModelId equals leftJoin.modelData.Id into modelData
                                      
                    select new InteractableElementData()
                    {
                        Id = interactableData.Id,
                        Index = interactableData.Index,

                        Type = interactableData.Type,

                        ModelId = interactableData.ModelId,

                        Name = interactableData.Name,
                        
                        Scale = interactableData.Scale,

                        Health = interactableData.Health,
                        Hunger = interactableData.Hunger,
                        Thirst = interactableData.Thirst,

                        Weight = interactableData.Weight,
                        Speed = interactableData.Speed,
                        Stamina = interactableData.Stamina,
                        
                        ModelPath = modelData.FirstOrDefault() != null ? modelData.FirstOrDefault().modelData.Path : "",
                        ModelIconPath = modelData.FirstOrDefault() != null ? modelData.FirstOrDefault().iconData.Path : "",

                        Height = modelData.FirstOrDefault() != null ? modelData.FirstOrDefault().modelData.Height : 0,
                        Width = modelData.FirstOrDefault() != null ? modelData.FirstOrDefault().modelData.Width : 0,
                        Depth = modelData.FirstOrDefault() != null ? modelData.FirstOrDefault().modelData.Depth : 0

                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static InteractableElementData DefaultData(int type)
    {
        return new InteractableElementData()
        {
            Type = type,
            ModelId = 1
        };
    }

    public static void SetDefaultAddValues(List<InteractableElementData> list)
    {
        var addElementData = list.Where(x => x.Id == 0).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;

        addElementData.Index = list.Count - 1;
    }

    private static void GetInteractableData(Search.Interactable searchParameters)
    {
        interactableDataList = new List<InteractableBaseData>();
        
        foreach(InteractableBaseData interactable in Fixtures.interactableList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(interactable.Id))          continue;
            if (searchParameters.excludeId.Count    > 0 && searchParameters.excludeId.Contains(interactable.Id))    continue;
            if (searchParameters.type.Count         > 0 && !searchParameters.type.Contains(interactable.Type))      continue;

            interactableDataList.Add(interactable);
        }
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

    public static void AddData(InteractableElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.interactableList.Count > 0 ? (Fixtures.interactableList[Fixtures.interactableList.Count - 1].Id + 1) : 1;
            Fixtures.interactableList.Add(((InteractableData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(InteractableElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.interactableList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedModelId)
            {
                data.ModelId = elementData.ModelId;
            }

            if (elementData.ChangedName)
            {
                data.Name = elementData.Name;
            }

            if (elementData.ChangedScale)
            {
                data.Scale = elementData.Scale;
            }

            if (elementData.ChangedHealth)
            {
                data.Health = elementData.Health;
            }

            if (elementData.ChangedHunger)
            {
                 data.Hunger = elementData.Hunger;
            }

            if (elementData.ChangedThirst)
            {
                data.Thirst = elementData.Thirst;
            }

            if (elementData.ChangedWeight)
            {
                data.Weight = elementData.Weight;
            }

            if (elementData.ChangedSpeed)
            {
                data.Speed = elementData.Speed;
            }

            if (elementData.ChangedStamina)
            {
                data.Stamina = elementData.Stamina;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    static public void UpdateIndex(InteractableElementData elementData)
    {
        if (!elementData.ChangedIndex) return;

        var data = Fixtures.interactableList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;

        elementData.OriginalData.Index = elementData.Index;
    }

    public static void RemoveData(InteractableElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.interactableList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
