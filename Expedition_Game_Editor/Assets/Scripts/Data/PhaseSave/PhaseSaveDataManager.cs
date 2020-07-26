﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseSaveDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<PhaseSaveData> phaseSaveDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.PhaseData> phaseDataList;

    public PhaseSaveDataManager(PhaseSaveController phaseController)
    {
        DataController = phaseController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
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
                    join phaseData      in phaseDataList on phaseSaveData.phaseId equals phaseData.Id
                    select new PhaseSaveElementData()
                    {
                        Id = phaseSaveData.Id,

                        ChapterSaveId = phaseSaveData.chapterSaveId,
                        PhaseId = phaseSaveData.phaseId,

                        Complete = phaseSaveData.complete,

                        name = phaseData.name,

                        publicNotes = phaseData.publicNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private void GetCustomPhaseSaveData(Search.PhaseSave searchParameters)
    {
        phaseSaveDataList = new List<PhaseSaveData>();

        foreach (Fixtures.PhaseSave phaseSave in Fixtures.phaseSaveList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(phaseSave.Id))                         continue;
            if (searchParameters.chapterSaveId.Count    > 0 && !searchParameters.chapterSaveId.Contains(phaseSave.chapterSaveId))   continue;

            var phaseSaveData = new PhaseSaveData();

            phaseSaveData.Id = phaseSave.Id;

            phaseSaveData.chapterSaveId = phaseSave.chapterSaveId;
            phaseSaveData.phaseId = phaseSave.phaseId;

            phaseSaveData.complete = phaseSave.complete;

            phaseSaveDataList.Add(phaseSaveData);
        }
    }

    private void GetPhaseSaveDataByChapter(Search.PhaseSave searchParameters)
    {
        phaseSaveDataList = new List<PhaseSaveData>();

        var phaseDataList = Fixtures.phaseList.Where(x => searchParameters.chapterId.Contains(x.chapterId)).Distinct().ToList();
        var phaseSaveList = Fixtures.phaseSaveList.Where(x => phaseDataList.Select(y => y.Id).Contains(x.phaseId)).Distinct().ToList();

        foreach(Fixtures.PhaseSave phaseSave in phaseSaveList)
        {
            var phaseSaveData = new PhaseSaveData();

            phaseSaveData.Id = phaseSave.Id;

            phaseSaveData.chapterSaveId = phaseSave.chapterSaveId;
            phaseSaveData.phaseId = phaseSave.phaseId;

            phaseSaveData.complete = phaseSave.complete;

            phaseSaveDataList.Add(phaseSaveData);
        }
    }

    internal void GetPhaseData()
    {
        var phaseSearchParameters = new Search.Phase();
        phaseSearchParameters.id = phaseSaveDataList.Select(x => x.phaseId).Distinct().ToList();

        phaseDataList = dataManager.GetPhaseData(phaseSearchParameters);
    }

    internal class PhaseSaveData : GeneralData
    {
        public int chapterSaveId;
        public int phaseId;

        public bool complete;
    }
}