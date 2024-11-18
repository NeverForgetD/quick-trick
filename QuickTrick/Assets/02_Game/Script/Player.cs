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
    private float triggerStartTime;
    private float responseTime;

    private int playerID;

    public TextMeshProUGUI text;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            ResetTrigger();
            text.text = Runner.LocalPlayer.PlayerId.ToString();
            playerID = Runner.LocalPlayer.PlayerId;
            Debug.Log($"이 컴퓨터에서는 로컬플레이어 ID가 {Runner.LocalPlayer.PlayerId}");
        }
    }

    bool enableClick_Client = true; // 클라이언트 측에서 허용

    private void Update()
    {
        // triggerOn이 true가 된 시점을 기록
        if (Object.HasInputAuthority && triggerOn && triggerStartTime == 0f)
        {
            triggerStartTime = Time.time;
        }

        // 트리거 시작 됐는 지 확인, 그리고 버튼 누르면 누를 수 없게 방지
        if (canClick)
        {
            if (Object.HasInputAuthority && Input.GetMouseButtonDown(0) && enableClick_Client)
            {
                if (triggerOn) // 클릭 가능, 트리거 ON
                {
                    responseTime = Time.time - triggerStartTime;
                    RPC_SendResponseTimeToServer(Runner.LocalPlayer, responseTime);
                }
                else if (!triggerOn) // 클릭 가능, 트리거 OFF_부정출발
                {
                    RPC_SendResponseTimeToServer(Runner.LocalPlayer, responseTime, false);
                }

                enableClick_Client = false;
                ResetTrigger();
            }
        }

        // canClick 아닌데 누르면, 에러 소리 나게 할까? 일단 중요도 하
    }

    private void ResetTrigger()
    {
        triggerStartTime = 0f;
        responseTime = 0f;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SendResponseTimeToServer(PlayerRef playerRef, float responseTime, bool isValid = true)
    {
        // 이벤트 호출로 StateAuthority GameManager에 알림
        playerID = playerRef.PlayerId;
        //OnPlayerClicked?.Invoke(playerID , responseTime);
        OnPlayerClicked?.Invoke(playerID, responseTime, isValid);
    }
}
