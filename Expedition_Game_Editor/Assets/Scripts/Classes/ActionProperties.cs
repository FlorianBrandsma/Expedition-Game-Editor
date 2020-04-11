[System.Serializable]
public class ActionProperties
{
    public enum Anchor
    {
        Main,
        Left,
        Right,
    }

    public Anchor anchor;
    public int width;
}
