using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<PhaseData> phaseDataList;

    public PhaseDataManager(PhaseController phaseController)
    {
        DataController = phaseController;
    }

    public List<IDataElement> GetDataElements(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Phase>().First();

        GetCustomPhaseData(searchParameters);

        if (phaseDataList.Count == 0) return new List<IDataElement>();

        var list = (from phaseData in phaseDataList
                    select new PhaseDataElement()
                    {
                        Id = phaseData.Id,
                        Index = phaseData.Index,

                        ChapterId = phaseData.chapterId,

                        Name = phaseData.name,
                        PublicNotes = phaseData.notes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    private void GetCustomPhaseData(Search.Phase searchParameters)
    {
        phaseDataList = new List<PhaseData>();

        foreach(Fixtures.Phase phase in Fixtures.phaseList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(phase.Id)) continue;
            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(phase.chapterId)) continue;

            var phaseData = new PhaseData();

            phaseData.Id = phase.Id;
            phaseData.Index = phase.Index;

            phaseData.chapterId = phase.chapterId;
            phaseData.name = phase.name;
            phaseData.notes = phase.publicNotes;

            phaseDataList.Add(phaseData);
        }
    }

    internal class PhaseData : GeneralData
    {
        public int chapterId;
        public string name;
        public string notes;
    }
}
