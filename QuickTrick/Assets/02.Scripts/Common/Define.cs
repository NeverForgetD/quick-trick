using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum GameMode // �̴� ���� ������
    {
        //Default00,
        //QuickDraw01,
        CarRace02,
        MaxCount, // ���� ���� ���� ��...������ ���� X
    }

    public enum RunnerStatus
    {
        NONE,
        CONNECTINGSERVER,
        WAITING,
        TIMEOUT,
        DISCONNECTING,
        GAME,
    }

    public enum GameState
    {
        ENTER,
        STARTGAME,
        STARTROUND,
    }
}
