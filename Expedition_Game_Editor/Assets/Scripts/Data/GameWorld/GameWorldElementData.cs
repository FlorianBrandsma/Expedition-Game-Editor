using UnityEngine;
using System.Collections.Generic;

public class GameWorldElementData : GeneralData, IElementData
{
    public Vector3 tempPlayerPosition;

    public ChapterElementData chapterData;
    public PhaseElementData phaseData;

    public List<RegionElementData> regionDataList;

    public DataElement DataElement { get; set; }
    public bool Changed { get; set; }

    public void Update() { }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public void SetOriginalValues()
    {
        //terrainDataList.ForEach(x => x.SetOriginalValues());
    }

    public void GetOriginalValues() { }

    public void ClearChanges() { }

    public IElementData Clone()
    {
        var elementData = new EditorWorldElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    new public virtual void Copy(IElementData dataSource)
    {
        var worldDataSource = (GameWorldElementData)dataSource;
    }
}
