public class GeneralElementData : GeneralData, IElementData
{
    public DataElement DataElement { get; set; }

    public GeneralElementData() : base()
    {
        DataType = Enums.DataType.None;
    }

    public void Update() { }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public void SetOriginalValues() { }

    public void GetOriginalValues() { }

    public void ClearChanges() { }

    public bool Changed { get { return false; } }

    public IElementData Clone()
    {
        var elementData = new GeneralElementData();

        CloneGeneralData(elementData);
        
        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        SetOriginalValues();
    }
}
