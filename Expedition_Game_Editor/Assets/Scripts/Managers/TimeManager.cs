using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public enum Time
    {
        Day,
        Night,
    }

    private float dot;

    public Color baseColor;
    public Gradient nightDayColor;
    public Gradient ambientColor;

    public Gradient nightDayFogColor;
    public AnimationCurve fogDensityCurve;
    public float fogScale = 1f;

    static public Time defaultTime = Time.Day;
    static public Time activeTime;
    
    public void GetTimes()
    {
        activeTime = defaultTime;
    }
    
    public void SetTime(Time time)
    {
        activeTime = time;

        SetLighting(activeTime);

        EditorManager.editorManager.ResetEditor();
    }

    public void SetLighting()
    {
        SetLighting(activeTime);
    }

    public void ResetLighting(Light light)
    {
        SetLighting(defaultTime);

        light.color = baseColor;
    }

    public void SetLighting(Time time)
    {
        dot = Mathf.Clamp01((int)time + 0.5f);

        RenderSettings.ambientLight = ambientColor.Evaluate(dot);

        RenderSettings.fogColor = nightDayFogColor.Evaluate(dot);
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;
    }

    public void SetCameraLight(Light light)
    {
        light.color = nightDayColor.Evaluate(dot);
    }
}
