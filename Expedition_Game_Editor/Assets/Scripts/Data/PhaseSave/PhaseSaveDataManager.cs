﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class PhaseSaveDataManager
{
    private static List<PhaseSaveBaseData> phaseSaveDataList;

    private static List<PhaseBaseData> phaseDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.PhaseSave>().First();

        switch (searchParameters.requestType)
        {
            case Search.PhaseSave.RequestType.Custom:

                GetCustomPhaseSaveData(searchParameters);
                break;

            case Search.PhaseSave.RequestType.GetPhaseSaveByChapter:

                GetPhaseSaveDataByChapter(searchParameters);
                break;
        }
        
        if (phaseSaveDataList.Count == 0) return new List<IElementData>();

        GetPhaseData();

        var list = (from phaseSaveData  in phaseSaveDataList
                    join phaseData      in phaseDataList on phaseSaveData.PhaseId equals phaseData.Id
                    select new PhaseSaveElementData()
                    {
                        Id = phaseSaveData.Id,
                        Index = phaseSaveData.Index,

                        ChapterSaveId = phaseSaveData.ChapterSaveId,
                        PhaseId = phaseSaveData.PhaseId,

                        Complete = phaseSaveData.Complete,

                        Name = phaseData.Name,

                        PublicNotes = phaseData.PublicNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetCustomPhaseSaveData(Search.PhaseSave searchParameters)
    {
        phaseSaveDataList = new List<PhaseSaveBaseData>();

        foreach (PhaseSaveBaseData phaseSave in Fixtures.phaseSaveList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(phaseSave.Id))                         continue;
            if (searchParameters.chapterSaveId.Count    > 0 && !searchParameters.chapterSaveId.Contains(phaseSave.ChapterSaveId))   continue;

            var phaseSaveData = new PhaseSaveBaseData();

            phaseSaveData.Id = phaseSave.Id;
            phaseSaveData.Index = phaseSave.Index;

            phaseSaveData.ChapterSaveId = phaseSave.ChapterSaveId;
            phaseSaveData.PhaseId = phaseSave.PhaseId;

            phaseSaveData.Complete = phaseSave.Complete;

            phaseSaveDataList.Add(phaseSaveData);
        }
    }

    private static void GetPhaseSaveDataByChapter(Search.PhaseSave searchParameters)
    {
        phaseSaveDataList = new List<PhaseSaveBaseData>();

        var phaseDataList = Fixtures.phaseList.Where(x => searchParameters.chapterId.Contains(x.ChapterId)).Distinct().ToList();
        var phaseSaveList = Fixtures.phaseSaveList.Where(x => phaseDataList.Select(y => y.Id).Contains(x.PhaseId)).Distinct().ToList();

        foreach(PhaseSaveBaseData phaseSave in phaseSaveList)
        {
            var phaseSaveData = new PhaseSaveBaseData();

            phaseSaveData.Id = phaseSave.Id;
            phaseSaveData.Index = phaseSave.Index;

            phaseSaveData.ChapterSaveId = phaseSave.ChapterSaveId;
            phaseSaveData.PhaseId = phaseSave.PhaseId;

            phaseSaveData.Complete = phaseSave.Complete;

            phaseSaveDataList.Add(phaseSaveData);
        }
    }

    private static void GetPhaseData()
    {
        var phaseSearchParameters = new Search.Phase();
        phaseSearchParameters.id = phaseSaveDataList.Select(x => x.PhaseId).Distinct().ToList();

        phaseDataList = DataManager.GetPhaseData(phaseSearchParameters);
    }

    public static void UpdateData(PhaseSaveElementData elementData)
    {
        var data = Fixtures.phaseSaveList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedComplete)
            data.Complete = elementData.Complete;
    }
}
