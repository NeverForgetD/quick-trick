using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : NetworkBehaviour
{
    // Ŭ�� ��� Ÿ�̸ӿ� ���õ� ������
    [Networked] private float waitingTime { get; set; } // ��� �ð�
    [Networked] private bool isClickAllowed { get; set; } // Ŭ�� ���� ����
    private float elapsedTime;

    // ���� �ʱ�ȭ
    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            StartCoroutine(InitGame());
        }
        //base.Spawned();
    }

    // ������ �ʱ�ȭ�ϴ� �ڷ�ƾ
    private IEnumerator InitGame()
    {
        // ���� ��� �ð� ����
        waitingTime = Random.Range(5.0f, 10.0f);   // �̰� Random �� �ᵵ �ǰ� �����غ��� ���� ��ġ�� ����� ã�ƺ���
        elapsedTime = 0;
        isClickAllowed = false;


        // ��� �ð� ���� ��ٸ�
        yield return new WaitForSeconds(waitingTime);

        // Ŭ�� ���� ���·� ��ȯ
        RPC_SetClickAlloewd();
    }

    // �������� Ŭ�� ���� ���θ� �����ϴ� �޼���
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetClickAlloewd()
    {
        isClickAllowed = true;
        Debug.Log("Ŭ�� ����");
    }

    // Ŭ���̾�Ʈ���� Ŭ�� ���� �� ���� �ӵ� ���
    private void Update()
    {
        if (isClickAllowed && Input.GetMouseButtonDown(0))
        {
            float reationTime = elapsedTime - waitingTime;
            isClickAllowed= false;
            // Ŭ�� ���� �߰� ó�� (���� �Ǵ�)
        }

        if (!isClickAllowed)
        {
            elapsedTime += Time.deltaTime;
        }
    }
}
