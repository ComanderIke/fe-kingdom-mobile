using System;

public interface IBattleSuccessRenderer
{
    public void Show();
    void Hide();
    event Action OnFinished;
}