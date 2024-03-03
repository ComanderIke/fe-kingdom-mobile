namespace Game.GameInput.Interfaces
{
    public interface IGridInputReceiver
    {
        void GridClicked(int x, int y);
        void GridClickedDown(int x, int y);
    }
}