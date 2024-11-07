using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum GameMode
    {
        COLORSHIFT,
        GUNFIGHT,
        CARDGAME,
    }

    public enum MainMenuUI
    {
        NONE,
        CONNECTINGSERVER,
        WAITING,
        FINDROOM,
        TIMEOUT,

    }
}
