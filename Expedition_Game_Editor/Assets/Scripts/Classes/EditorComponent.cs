[System.Serializable]
public class EditorComponent
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
