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
}
