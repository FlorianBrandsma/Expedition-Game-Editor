using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class PhaseSaveDataManager
{
    private static List<PhaseBaseData> phaseDataList;

    private static List<PhaseSaveBaseData> phaseSaveDataList;
    
    public static List<IElementData> GetData(Search.PhaseSave searchParameters)
    {
        GetPhaseData(searchParameters);

        if (phaseDataList.Count == 0) return new List<IElementData>();

        GetPhaseSaveData(searchParameters);
        
        var list = (from phaseData      in phaseDataList
                    join phaseSaveData  in phaseSaveDataList on phaseData.Id equals phaseSaveData.PhaseId
                    select new PhaseSaveElementData()
                    {
                        Id = phaseSaveData.Id,
                        
                        PhaseId = phaseSaveData.PhaseId,

                        Complete = phaseSaveData.Complete,

                        Index = phaseData.Index,

                        Name = phaseData.Name,

                        EditorNotes = phaseData.EditorNotes,
                        GameNotes = phaseData.GameNotes,

                        ChapterId = phaseData.ChapterId

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static PhaseSaveElementData DefaultData(int saveId, int phaseId)
    {
        return new PhaseSaveElementData()
        {
            Id = -1,

            SaveId = saveId,
            PhaseId = phaseId
        };
    }

    private static void GetPhaseData(Search.PhaseSave searchParameters)
    {
        phaseDataList = new List<PhaseBaseData>();

        foreach (PhaseBaseData phase in Fixtures.phaseList)
        {
            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(phase.ChapterId)) continue;

            phaseDataList.Add(phase);
        }
    }

    private static void GetPhaseSaveData(Search.PhaseSave searchParameters)
    {
        searchParameters.phaseId = phaseDataList.Select(x => x.Id).Distinct().ToList();

        phaseSaveDataList = DataManager.GetPhaseSaveData(searchParameters);
    }

    public static void AddData(PhaseSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.phaseSaveList.Count > 0 ? (Fixtures.phaseSaveList[Fixtures.phaseSaveList.Count - 1].Id + 1) : 1;
            Fixtures.phaseSaveList.Add(((PhaseSaveData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(PhaseSaveElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.phaseSaveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedComplete)
            {
                data.Complete = elementData.Complete;
            }

            elementData.SetOriginalValues();

        } else { }  
    }

    public static void RemoveData(PhaseSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.phaseSaveList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
