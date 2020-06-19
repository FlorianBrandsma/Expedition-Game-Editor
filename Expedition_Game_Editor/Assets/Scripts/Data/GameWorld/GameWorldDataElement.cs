using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameWorldDataElement : GeneralData, IDataElement
{
    public Vector3 tempPlayerPosition;

    public ChapterDataElement chapterData;
    public PhaseDataElement phaseData;

    public List<RegionDataElement> regionDataList;

    #region DataElement
    public SelectionElement SelectionElement { get; set; }

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

    public IDataElement Clone()
    {
        var dataElement = new EditorWorldDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    new public virtual void Copy(IDataElement dataSource)
    {
        var worldDataSource = (GameWorldDataElement)dataSource;
    }
    #endregion
}
