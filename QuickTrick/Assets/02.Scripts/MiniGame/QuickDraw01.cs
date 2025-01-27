using DG.Tweening;
using UnityEngine;

public class QuickDraw01 : MiniGameBase
{
    public override int miniGameIndex => 1;

    #region SerializeField
    [SerializeField] SpriteRenderer cutIn;

    [SerializeField] SpriteRenderer playerIdle;
    [SerializeField] SpriteRenderer playerWin;
    [SerializeField] SpriteRenderer playerLose;

    [SerializeField] SpriteRenderer enemyIdle;
    [SerializeField] SpriteRenderer enemyWin;
    [SerializeField] SpriteRenderer enemyLose;

    [SerializeField] SpriteRenderer SunFlash;

    [SerializeField] SerializeField tumbleWeed;
    #endregion

    public override void OnLocalPlayerClicked(float responseTime)
    {
        cutIn.transform.DOMoveX(10, 1).From();
    }

    public override void OnLocalPlayerLose(float opponentResponseTime)
    {
        throw new System.NotImplementedException();
    }

    public override void OnLocalPlayerWin(float opponentResponseTime)
    {
        throw new System.NotImplementedException();
    }

    public override void OnStandBy()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerEvent()
    {
        throw new System.NotImplementedException();
    }
}
