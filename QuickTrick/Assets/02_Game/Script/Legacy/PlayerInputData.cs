using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerInputData : INetworkInput
{
    public NetworkBool buttonPressed; // ��ư �Է� ����
    public float pressTime; // ��ư ���� �ð�
}