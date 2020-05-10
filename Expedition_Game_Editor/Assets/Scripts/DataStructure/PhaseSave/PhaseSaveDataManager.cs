using UnityEngine;
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

    public List<IDataElement> GetDataElements(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.PhaseSave>().First();

        GetPhaseSaveData(searchParameters);

        if (phaseSaveDataList.Count == 0) return new List<IDataElement>();

        GetPhaseData();

        var list = (from phaseSaveData  in phaseSaveDataList
                    join phaseData      in phaseDataList on phaseSaveData.phaseId equals phaseData.Id
                    select new PhaseSaveDataElement()
                    {
                        Id = phaseSaveData.Id,

                        ChapterSaveId = phaseSaveData.chapterSaveId,
                        PhaseId = phaseSaveData.phaseId,

                        Complete = phaseSaveData.complete,

                        name = phaseData.name,

                        publicNotes = phaseData.publicNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetPhaseSaveData(Search.PhaseSave searchParameters)
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
