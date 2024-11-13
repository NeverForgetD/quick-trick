using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            //Debug.Log("플레이어 has StateQuthority----------------------");
        }
    }
}
