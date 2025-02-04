using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum GameMode // 미니 게임 종류들
    {
        //Default00,
        QuickDraw01,
        CarRace02,
        Card03,
        MaxCount, // 연산 편리상 넣은 것...실제로 게임 X
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
