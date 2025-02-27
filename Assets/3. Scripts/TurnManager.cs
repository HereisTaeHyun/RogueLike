using UnityEngine;

public class TurnManager
{
    private int turnCount;

    public TurnManager()
    {
        turnCount = 1;
    }

    public void Tick()
    {
        turnCount += 1;
        Debug.Log($"Current Turn : {turnCount}");
    }
}
