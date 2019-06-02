using UnityEngine;
using System.Collections;

public class ObjectGraphicCore : GeneralData
{
    private string name;
    private string path;
    private string icon;

    public string originalName;
    public string originalPath;
    public string originalIcon;

    private bool changedName;
    private bool changedPath;
    private bool changedIcon;

    public bool Changed
    {
        get
        {
            return changedName || changedPath || changedIcon;
        }
    }

    #region Properties

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

    public string Icon
    {
        get { return icon; }
        set
        {
            if (value == icon) return;

            changedIcon = (value != originalIcon);

            icon = value;
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

    public void SetOriginalValues()
    {
        originalName = name;
        originalPath = path;
        originalIcon = icon;

        ClearChanges();
    }

    public void GetOriginalValues()
    {
        name = originalName;
        path = originalPath;
        icon = originalIcon;
    }

    public void ClearChanges()
    {
        GetOriginalValues();

        changedName = false;
        changedPath = false;
        changedIcon = false;
    }

    public void Delete()
    {

    }

    #endregion
}
