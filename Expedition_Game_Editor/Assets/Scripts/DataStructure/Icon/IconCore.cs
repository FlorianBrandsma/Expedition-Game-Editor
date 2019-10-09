using UnityEngine;
using System.Collections;

public class IconCore : GeneralData
{
    private string path;

    public string originalPath;

    private bool changedPath;

    public bool Changed
    {
        get
        {
            return changedPath;
        }
    }

    #region Properties

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

    #endregion

    #region Methods

    public void Create() { }

    public virtual void Update() { }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalPath = path;
    }

    public void GetOriginalValues()
    {
        path = originalPath;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedPath = false;
    }

    public void Delete() { }

    #endregion
}
