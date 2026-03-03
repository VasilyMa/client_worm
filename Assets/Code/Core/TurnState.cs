public class TurnState
{
    public int CurrentTurn;
    public int PlayerID;
    public bool IsMineTurn;

    public TurnState(bool IsMine, int playerID)
    {
        IsMineTurn = IsMine;
        CurrentTurn = 0;
        PlayerID = playerID;
    }

    public void SwitchTurn()
    {
        CurrentTurn++;

        IsMineTurn = !IsMineTurn;
    }
}
