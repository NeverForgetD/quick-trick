using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum GameMode // 미니 게임 종류들
    {
        COLORSHIFT,
        GUNFIGHT,
        CARDGAME,
        MaxCount, // 연산 편리상 넣은 것...실제로 게임 X
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
