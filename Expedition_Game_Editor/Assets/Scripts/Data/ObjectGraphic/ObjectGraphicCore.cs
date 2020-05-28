using UnityEngine;
using System.Collections;

public class ObjectGraphicCore : GeneralData
{
    private int iconId;

    private string name;
    private string path;

    private float height;
    private float width;
    private float depth;

    public bool Changed { get { return false; } }

    #region Properties
    public int IconId
    {
        get { return iconId; }
        set { iconId = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Path
    {
        get { return path; }
        set { path = value; }
    }

    public float Height
    {
        get { return height; }
        set { height = value; }
    }

    public float Width
    {
        get { return width; }
        set { width = value; }
    }

    public float Depth
    {
        get { return depth; }
        set { depth = value; }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update() { }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues() { }

    public void GetOriginalValues() { }

    public virtual void ClearChanges()
    {
        GetOriginalValues();
    }

    public void Delete() { }
    #endregion

    new public virtual void Copy(IDataElement dataSource)
    {
        var objectGraphicDataSource = (ObjectGraphicDataElement)dataSource;

        iconId = objectGraphicDataSource.iconId;

        name = objectGraphicDataSource.name;
        path = objectGraphicDataSource.path;

        height = objectGraphicDataSource.height;
        width = objectGraphicDataSource.width;
        depth = objectGraphicDataSource.depth;
    }
}
