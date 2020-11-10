using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class InteractableDataManager
{
    private static List<InteractableBaseData> interactableDataList;

    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Interactable>().First();
        
        GetInteractableData(searchParameters);

        if (interactableDataList.Count == 0) return new List<IElementData>();

        GetModelData();
        GetIconData();

        var list = (from interactableData   in interactableDataList
                    join modelData          in modelDataList    on interactableData.ModelId equals modelData.Id
                    join iconData           in iconDataList     on modelData.IconId         equals iconData.Id
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
                        
                        ModelPath = modelData.Path,
                        ModelIconPath = iconData.Path,

                        Height = modelData.Height,
                        Width = modelData.Width,
                        Depth = modelData.Depth

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
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

    public static void UpdateData(InteractableElementData elementData)
    {
        var data = Fixtures.interactableList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedModelId)
            data.ModelId = elementData.ModelId;

        if (elementData.ChangedName)
            data.Name = elementData.Name;

        if (elementData.ChangedScale)
            data.Scale = elementData.Scale;

        if (elementData.ChangedHealth)
            data.Health = elementData.Health;

        if (elementData.ChangedHunger)
            data.Hunger = elementData.Hunger;

        if (elementData.ChangedThirst)
            data.Thirst = elementData.Thirst;

        if (elementData.ChangedWeight)
            data.Weight = elementData.Weight;

        if (elementData.ChangedSpeed)
            data.Speed = elementData.Speed;

        if (elementData.ChangedStamina)
            data.Stamina = elementData.Stamina;
    }

    static public void UpdateIndex(InteractableElementData elementData)
    {
        var data = Fixtures.interactableList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
