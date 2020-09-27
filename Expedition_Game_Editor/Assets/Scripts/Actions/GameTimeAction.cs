using UnityEngine;
using UnityEngine.UI;

public class GameTimeAction : MonoBehaviour, IAction
{
    public ExDropdown Dropdown { get; set; }

    public ActionProperties actionProperties;

    public void InitializeAction(Path path) { }
    
    public void SetAction(Path path)
    {
        //if (GlobalManager.programType == GlobalManager.Scenes.Game) return;

        Dropdown = ActionManager.instance.AddDropdown(actionProperties);

        Dropdown.Dropdown.captionText.text = TimeManager.FormatTime(TimeManager.instance.ActiveTime);

        for (int hour = 0; hour < TimeManager.hoursInDay; hour++)
        {
            Dropdown.Dropdown.options.Add(new Dropdown.OptionData(TimeManager.FormatTime(hour * TimeManager.secondsInHour)));
        }

        Dropdown.Dropdown.value = Mathf.FloorToInt(TimeManager.instance.ActiveTime / TimeManager.secondsInHour);

        Dropdown.Dropdown.onValueChanged.AddListener(delegate { SetTime(Dropdown.Dropdown.value); });
    }

    public void UpdateAction()
    {
        if (Dropdown == null) return;

        Dropdown.Dropdown.captionText.text = TimeManager.FormatTime(TimeManager.instance.ActiveTime);

        Dropdown.Dropdown.value = TimeManager.instance.ActiveTime / TimeManager.secondsInHour;
    }

    public void SetTime(int selectedHour)
    {
        if (Dropdown == null) return;
        
        //TimeManager.instance.ActiveTime = (selectedHour * TimeManager.secondsInHour);
    }

    public void CloseAction() { }
}