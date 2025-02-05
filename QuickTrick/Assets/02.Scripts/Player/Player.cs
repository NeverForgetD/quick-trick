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
    /// Ʈ���� �̺�Ʈ�� �߻��� �ð�_Ŭ���̾�Ʈ���� ����
    /// </summary>
    private float triggerStartTime;

    /// <summary>
    /// �÷��̾��� �����ӵ�
    /// </summary>
    private float responseTime;

    /// <summary>
    /// �÷��̾� ���̵� ��ȣ_ȣ��Ʈ1 Ŭ���̾�Ʈ2
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

        // ���Ĺ��� ��������, �׸��� ��ư ������ ���� �� ���� ����
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
    /// triggerOn�� true�� �� ������ ���
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

        if (triggerOn) //Ʈ���� ON
        {
            responseTime = Time.time - triggerStartTime;
            MiniGameManager.Instance._miniGameInstance.OnLocalPlayerClicked(responseTime);
            RPC_SendResponseToServer(responseTime);

            Debug.Log($"{Runner.LocalPlayer} : {responseTime}");
        }
        else if (!triggerOn) //Ʈ���� OFF_�������
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
    /// ���� �÷��̾ �������� ���� ������ ������.
    /// <param name="responseTime"></param>
    /// <param name="isValid"></param>
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SendResponseToServer(float responseTime, bool isValid = true)
    {
        // �̺�Ʈ ȣ��� StateAuthority GameManager�� �˸�

        //playerID = playerRef.PlayerId;

        //OnPlayerClicked?.Invoke(playerID , responseTime);
        OnPlayerClicked?.Invoke(localPlayerID, responseTime, isValid);
        Debug.Log($"{localPlayerID} id is sent to server");
    }


    /*
         /// <summary>
    /// </summary>
    /// <param name="playerRef"></param>
    /// ���� �÷��̾ �������� ���� ������ ������.
    /// <param name="responseTime"></param>
    /// <param name="isValid"></param>
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SendResponseToServer(PlayerRef playerRef, float responseTime, bool isValid = true)
    {
        // �̺�Ʈ ȣ��� StateAuthority GameManager�� �˸�
        playerID = playerRef.PlayerId;
        //OnPlayerClicked?.Invoke(playerID , responseTime);
        OnPlayerClicked?.Invoke(playerID, responseTime, isValid);
    }
     */
    #endregion
}
