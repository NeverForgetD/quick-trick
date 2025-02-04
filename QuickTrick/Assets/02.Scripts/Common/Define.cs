using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum GameMode // �̴� ���� ������
    {
        //Default00,
        QuickDraw01,
        CarRace02,
        Card03,
        MaxCount, // ���� ���� ���� ��...������ ���� X
    }

    public enum RunnerStatus
    {
        TITLE,
        CONNECTINGSERVER,
        WAITING,
        TIMEOUT,
        DISCONNECTING,
        GAME,
        NONE,
    }

    public enum GameState
    {
        ENTER,
        STARTGAME,
        STARTROUND,
    }
}
