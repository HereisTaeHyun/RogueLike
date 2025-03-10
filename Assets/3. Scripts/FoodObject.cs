using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : CellObject
{
    public int AmountGranted = 10;
    public AudioClip clip;
    public override void PlayerEntered()
    {
        Destroy(gameObject);
        GameManager.Instance.ChangeFood(AmountGranted);
        GameManager.Instance.PlaySound(clip);
    }

    public override bool PlayerWantsToEnter()
    {
        return base.PlayerWantsToEnter();
    }
}
