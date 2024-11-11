using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum GameMode // �̴� ���� ������
    {
        COLORSHIFT,
        GUNFIGHT,
        CARDGAME,
        MaxCount, // ���� ���� ���� ��...������ ���� X
    }

    public enum MainMenuUI
    {
        NONE,
        CONNECTINGSERVER,
        WAITING,
        FINDROOM,
        TIMEOUT,

    }

    public enum RunnerStatus
    {
        NONE,
        CONNECTINGSERVER,
        WAITING,
        FINDROOM,
        TIMEOUT,
        DISCONNECTING,
    }
}
