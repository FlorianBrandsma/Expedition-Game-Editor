using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseAction : MonoBehaviour, IAction
{
    private ExButton button;

    private Texture2D playIcon;
    private Texture2D pauseIcon;

    public ActionProperties actionProperties;

    public void InitializeAction(Path path)
    {
        playIcon = Resources.Load<Texture2D>("Textures/Icons/UI/Play");
        pauseIcon = Resources.Load<Texture2D>("Textures/Icons/UI/Pause");
    }

    public void SetAction(Path path)
    {
        //if (GlobalManager.programType == GlobalManager.Scenes.Game) return;

        button = ActionManager.instance.AddButton(actionProperties);

        button.icon.texture = pauseIcon;

        button.Button.onClick.AddListener(delegate { PauseTime(!TimeManager.instance.Paused); });
    }

    public void PauseTime(bool pause)
    {
        TimeManager.instance.PauseTime(pause);
    }

    public void UpdateAction()
    {
        if (button == null) return;

        button.icon.texture = TimeManager.instance.Paused ? playIcon : pauseIcon;
    }

    public void CloseAction() { }
}
