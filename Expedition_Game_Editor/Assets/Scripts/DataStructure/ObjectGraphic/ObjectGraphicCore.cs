using UnityEngine;
using System.Collections;

public class ObjectGraphicCore : GeneralData
{
    private int iconId;
    private string name;
    private string path;

    public int originalIconId;
    public string originalName;
    public string originalPath;

    private bool changedIconId;
    private bool changedName;
    private bool changedPath;
    
    public bool Changed
    {
        get
        {
            return changedName || changedPath || changedIconId;
        }
    }

    #region Properties

    public int Id { get { return id; } }

    public string Name
    {
        get { return name; }
        set
        {
            if (value == name) return;

            changedName = (value != originalName);

            name = value;
        }
    }

    public string Path
    {
        get { return path; }
        set
        {
            if (value == path) return;

            changedPath = (value != originalPath);

            path = value;
        }
    }

    public int IconId
    {
        get { return iconId; }
        set
        {
            if (value == iconId) return;

            changedIconId = (value != originalIconId);

            iconId = value;
        }
    }

    #endregion

    #region Methods

    public void Create()
    {

    }

    public virtual void Update() { }

    public void UpdateSearch() { }

    public virtual void SetOriginalValues()
    {
        originalName = name;
        originalPath = path;
        originalIconId = iconId;
    }

    public void GetOriginalValues()
    {
        name = originalName;
        path = originalPath;
        iconId = originalIconId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedName = false;
        changedPath = false;
        changedIconId = false;
    }

    public void Delete()
    {

    }

    #endregion
}
