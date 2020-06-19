public interface IAction
{
    void InitializeAction(Path path);
    void SetAction(Path path);
    void CloseAction();
}
