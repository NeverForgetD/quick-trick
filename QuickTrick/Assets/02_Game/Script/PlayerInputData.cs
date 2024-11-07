using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerInputData : INetworkInput
{
    public NetworkBool buttonPressed; // 버튼 입력 상태
    public float pressTime; // 버튼 누른 시간
}