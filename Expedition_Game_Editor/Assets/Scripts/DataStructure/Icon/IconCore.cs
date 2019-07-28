using UnityEngine;
using System.Collections;

public class IconCore : GeneralData
{
    private string path;

    public int originalIndex;
    public string originalPath;

    private bool changedIndex;
    private bool changedPath;

    public bool Changed
    {
        get
        {
            return changedPath;
        }
    }

    #region Properties

    public int Id { get { return id; } }

    public int Index
    {
        get { return index; }
        set
        {
            if (value == index) return;

            changedIndex = true;

            index = value;
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

    #endregion

    #region Methods

    public void Create()
    {

    }

    public void Update()
    {
        if (!Changed) return;

        SetOriginalValues();
    }

    public void UpdateSearch() { }

    public void SetOriginalValues()
    {
        originalPath = path;

        ClearChanges();
    }

    public void GetOriginalValues()
    {
        path = originalPath;
    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changedIndex = false;
        changedPath = false;
    }

    public void Delete()
    {

    }

    #endregion
}
