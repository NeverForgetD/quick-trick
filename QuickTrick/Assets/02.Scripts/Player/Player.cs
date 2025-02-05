using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static event Action<int, float, bool> OnPlayerClicked;

    private bool isReady => MiniGameManager.Instance.miniGameReady;
    private bool triggerOn => MiniGameManager.Instance.triggerOn;

    /// <summary>
    /// 트리거 이벤트가 발생한 시간_클라이언트에서 저장
    /// </summary>
    private float triggerStartTime;

    /// <summary>
    /// 플레이어의 반응속도
    /// </summary>
    private float responseTime;

    /// <summary>
    /// 플레이어 아이디 번호_호스트1 클라이언트2
    /// </summary>
    private int localPlayerID;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            ResetTrigger();
            localPlayerID = Runner.LocalPlayer.PlayerId;
            Debug.Log("works");
            MiniGameManager.Instance.SetPlayerID(localPlayerID);
        }
    }

    #region Update
    private void Update()
    {
        updateTriggerOn();

        // 스탠바이 끝났는지, 그리고 버튼 누르면 누를 수 없게 방지
        if (isReady)
        {
            if (Object.HasInputAuthority && Input.GetMouseButtonDown(0))
            {
                HandlePlayerClick();
                ResetTrigger();
            }
        }
    }

    /// <summary>
    /// triggerOn이 true가 된 시점을 기록
    /// </summary>
    private void updateTriggerOn()
    {
        if (Object.HasInputAuthority && triggerOn && triggerStartTime == 0f)
        {
            triggerStartTime = Time.time;
        }
    }

    private void HandlePlayerClick()
    {
        MiniGameManager.Instance.GameDone();

        if (triggerOn) //트리거 ON
        {
            responseTime = Time.time - triggerStartTime;
            MiniGameManager.Instance._miniGameInstance.OnLocalPlayerClicked(responseTime);
            RPC_SendResponseToServer(responseTime);

            Debug.Log($"{Runner.LocalPlayer} : {responseTime}");
        }
        else if (!triggerOn) //트리거 OFF_부정출발
        {
            MiniGameManager.Instance._miniGameInstance.OnLocalPlayerClicked(-1);
            //MiniGameManager.Instance._miniGameInstance.OnLocalPlayerLose();
            RPC_SendResponseToServer(-1, false);
        }
    }

    private void ResetTrigger()
    {
        triggerStartTime = 0f;
        responseTime = 0f;
    }

    #endregion


    #region RPC
    /// <summary>
    /// </summary>
    /// <param name="playerRef"></param>
    /// 로컬 플레이어가 서버에게 반응 정보를 보낸다.
    /// <param name="responseTime"></param>
    /// <param name="isValid"></param>
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SendResponseToServer(float responseTime, bool isValid = true)
    {
        // 이벤트 호출로 StateAuthority GameManager에 알림

        //playerID = playerRef.PlayerId;

        //OnPlayerClicked?.Invoke(playerID , responseTime);
        OnPlayerClicked?.Invoke(localPlayerID, responseTime, isValid);
        Debug.Log($"{localPlayerID} id is sent to server");
    }


    /*
         /// <summary>
    /// </summary>
    /// <param name="playerRef"></param>
    /// 로컬 플레이어가 서버에게 반응 정보를 보낸다.
    /// <param name="responseTime"></param>
    /// <param name="isValid"></param>
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SendResponseToServer(PlayerRef playerRef, float responseTime, bool isValid = true)
    {
        // 이벤트 호출로 StateAuthority GameManager에 알림
        playerID = playerRef.PlayerId;
        //OnPlayerClicked?.Invoke(playerID , responseTime);
        OnPlayerClicked?.Invoke(playerID, responseTime, isValid);
    }
     */
    #endregion
}
