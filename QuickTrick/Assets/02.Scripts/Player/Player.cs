using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static event Action<int, float, bool> OnPlayerClicked;

    // miniGameStarted
    // triggerOn
    private bool canClick => MiniGameManager.Instance.miniGameStarted;
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

    public CanvasGroup canvas;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            ResetTrigger();
            playerID = Runner.LocalPlayer.PlayerId;
            Debug.Log($"�� ��ǻ�Ϳ����� �����÷��̾� ID�� {Runner.LocalPlayer.PlayerId}");
        }
    }

    bool enableClick_Client = true; // Ŭ���̾�Ʈ ������ ���

    private void Update()
    {
        // triggerOn�� true�� �� ������ ���
        if (Object.HasInputAuthority && triggerOn && triggerStartTime == 0f)
        {
            triggerStartTime = Time.time;
            canvas.alpha = 1f;
        }

        // Ʈ���� ���� �ƴ� �� Ȯ��, �׸��� ��ư ������ ���� �� ���� ����
        if (canClick)
        {
            if (Object.HasInputAuthority && Input.GetMouseButtonDown(0) && enableClick_Client)
            {
                if (triggerOn) // Ŭ�� ����, Ʈ���� ON
                {
                    responseTime = Time.time - triggerStartTime;
                    RPC_SendResponseTimeToServer(Runner.LocalPlayer, responseTime);
                }
                else if (!triggerOn) // Ŭ�� ����, Ʈ���� OFF_�������
                {
                    RPC_SendResponseTimeToServer(Runner.LocalPlayer, responseTime, false);
                }

                enableClick_Client = false;
                ResetTrigger();
            }
        }
    }

    private void ResetTrigger()
    {
        triggerStartTime = 0f;
        responseTime = 0f;

        canvas.alpha = 0f;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SendResponseTimeToServer(PlayerRef playerRef, float responseTime, bool isValid = true)
    {
        // �̺�Ʈ ȣ��� StateAuthority GameManager�� �˸�
        playerID = playerRef.PlayerId;
        //OnPlayerClicked?.Invoke(playerID , responseTime);
        OnPlayerClicked?.Invoke(playerID, responseTime, isValid);
    }
}
