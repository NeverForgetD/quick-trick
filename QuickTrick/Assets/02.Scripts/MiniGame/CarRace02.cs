using DG.Tweening;
using Febucci.UI;
using System.Threading.Tasks;
using UnityEngine;

public class CarRace02 : MiniGameBase
{
    public override int miniGameIndex => 2;

    #region SerializeField
    [SerializeField] CanvasGroup test;
    [SerializeField] GameObject trigger;
    #endregion

    //test
    private void Start()
    {
        OnStandBy();

    }
    public override void OnLocalPlayerClicked()
    {
        throw new System.NotImplementedException();
    }

    public override void OnLocalPlayerLose()
    {
        throw new System.NotImplementedException();
    }

    public override void OnLocalPlayerWin()
    {
        throw new System.NotImplementedException();
    }

    public override async void OnStandBy()
    {
        trigger.transform.DOLocalMoveY(1000, 1).From();
        await Task.Delay(1500);
        ShowText();
        //trigger.transform.DOMoveY(0, 0);
    }

    public override void OnTriggerEvent()
    {
        throw new System.NotImplementedException();
    }


}
