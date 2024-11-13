using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : NetworkBehaviour
{
    // 클릭 허용 타이머와 관련된 변수들
    [Networked] private float waitingTime { get; set; } // 대기 시간
    [Networked] private bool isClickAllowed { get; set; } // 클릭 가능 여부
    private float elapsedTime;

    // 게임 초기화
    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            StartCoroutine(InitGame());
        }
        //base.Spawned();
    }

    // 게임을 초기화하는 코루틴
    private IEnumerator InitGame()
    {
        // 랜덤 대기 시간 설정
        waitingTime = Random.Range(5.0f, 10.0f);   // 이건 Random 안 써도 되고 연구해보고 좋은 수치와 방법을 찾아보자
        elapsedTime = 0;
        isClickAllowed = false;


        // 대기 시간 동안 기다림
        yield return new WaitForSeconds(waitingTime);

        // 클릭 가능 상태로 전환
        RPC_SetClickAlloewd();
    }

    // 서버에서 클릭 가능 여부를 설정하는 메서드
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetClickAlloewd()
    {
        isClickAllowed = true;
        Debug.Log("클릭 가능");
    }

    // 클라이언트에서 클릭 감지 및 반응 속도 기록
    private void Update()
    {
        if (isClickAllowed && Input.GetMouseButtonDown(0))
        {
            float reationTime = elapsedTime - waitingTime;
            isClickAllowed= false;
            // 클릭 이후 추가 처리 (승자 판단)
        }

        if (!isClickAllowed)
        {
            elapsedTime += Time.deltaTime;
        }
    }
}
