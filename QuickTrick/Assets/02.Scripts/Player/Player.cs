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

    private int playerID;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            ResetTrigger();
            playerID = Runner.LocalPlayer.PlayerId;
            Debug.Log($"�� ��ǻ�Ϳ����� �����÷��̾� ID�� {Runner.LocalPlayer.PlayerId}");
        }
    }

    private void Update()
    {
        // triggerOn�� true�� �� ������ ���
        if (Object.HasInputAuthority && triggerOn && triggerStartTime == 0f)
        {
            triggerStartTime = Time.time;
        }

        // Ʈ���� ���� �ƴ� �� Ȯ��, �׸��� ��ư ������ ���� �� ���� ����
        if (isReady)
        {
            if (Object.HasInputAuthority && Input.GetMouseButtonDown(0))
            {
                HandlePlayerClick();

                ResetTrigger();
            }
            // ���� ���ÿ��� Ʈ���Ű� ������ n�� �Ŀ� �ڵ����� ��ȣ�� ������ ������ ����!
            // �׽�Ʈ �ϰ� �����ϴ� ������
        }
    }


    private void HandlePlayerClick()
    {
        MiniGameManager.Instance.GameDone();

        if (triggerOn) //Ʈ���� ON
        {
            responseTime = Time.time - triggerStartTime;
            MiniGameManager.Instance._miniGameInstance.OnLocalPlayerClicked();
            RPC_SendResponseToServer(Runner.LocalPlayer, responseTime);

            Debug.Log($"{Runner.LocalPlayer} : {responseTime}");
        }
        else if (!triggerOn) //Ʈ���� OFF_�������
        {
            MiniGameManager.Instance._miniGameInstance.OnLocalPlayerLose();
            RPC_SendResponseToServer(Runner.LocalPlayer, responseTime, false);
        }
    }

    private void ResetTrigger()
    {
        triggerStartTime = 0f;
        responseTime = 0f;
        //canvas.alpha = 0f;
    }

    #region RPC
    /// <summary>
    /// ���� �÷��̾ �������� ���� ������ ������.
    /// </summary>
    /// <param name="playerRef"></param>
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
    #endregion
}
