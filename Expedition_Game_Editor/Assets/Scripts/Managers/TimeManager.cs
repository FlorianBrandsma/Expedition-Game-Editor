using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public enum Time
    {
        Day,
        Night,
    }

    static public Time default_time = Time.Day;
    static public Time active_time;

    static public void GetTimes()
    {
        active_time = default_time;
    }

    static public void SetTime(int new_time)
    {
        active_time = (Time)new_time;

        EditorManager.editorManager.ResetEditor();
    }
}
