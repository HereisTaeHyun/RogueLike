using UnityEngine;

public class TurnManager
{
    private int turnCount;

    public event System.Action OnTick;
    public TurnManager()
    {
        turnCount = 1;
    }

    public void Tick()
    {
        OnTick?.Invoke();
        turnCount += 1;
        Debug.Log($"Current Turn : {turnCount}");
    }
}
