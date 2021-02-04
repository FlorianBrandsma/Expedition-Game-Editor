using UnityEngine;

public class SaveData : SaveBaseData
{
    public Enums.SaveType SaveType  { get; set; }

    public string ChapterName       { get; set; }
    public string LocationName      { get; set; }

    public string InteractableName  { get; set; }
    public string ModelIconPath     { get; set; }

    public string PhaseName         { get; set; }
    public string PhaseGameNotes    { get; set; }

    ////public string interactableName;

    //public DateTime SaveTime { get; set; }

    ////Testing purposes
    //public DateTime TestTime { get; set; }
    //public TimeSpan PassedTime { get { return SaveTime - TestTime; } }
    //public float PassedSeconds { get { return PassedTime.Seconds; } }
    ////----------------

    public override void GetOriginalValues(SaveData originalData)
    {
        SaveType            = originalData.SaveType;

        ChapterName         = originalData.ChapterName;
        LocationName        = originalData.LocationName;

        InteractableName    = originalData.InteractableName;
        ModelIconPath       = originalData.ModelIconPath;

        PhaseName           = originalData.PhaseName;
        PhaseGameNotes      = originalData.PhaseGameNotes;

        base.GetOriginalValues(originalData);
    }

    public SaveData Clone()
    {
        var data = new SaveData();

        data.SaveType           = SaveType;

        data.ChapterName        = ChapterName;
        data.LocationName       = LocationName;

        data.InteractableName   = InteractableName;
        data.ModelIconPath      = ModelIconPath;

        data.PhaseName          = PhaseName;
        data.PhaseGameNotes     = PhaseGameNotes;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(SaveElementData elementData)
    {
        elementData.SaveType            = SaveType;

        elementData.ChapterName         = ChapterName;
        elementData.LocationName        = LocationName;

        elementData.InteractableName    = InteractableName;
        elementData.ModelIconPath       = ModelIconPath;

        elementData.PhaseName           = PhaseName;
        elementData.PhaseGameNotes      = PhaseGameNotes;

        base.Clone(elementData);
    }
}
