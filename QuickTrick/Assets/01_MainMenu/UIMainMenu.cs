using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

namespace MainMenu
{
    public class UIMainMenu : MonoBehaviour
    {

        private void Start()
        { 
            //PlayerRef
            //NetworkRunner.StartGame();
            
        }
        /*
        public async Task StartPlayer(NetworkRunner runner)
        {
            var result = ArrayWithOffset runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.Shared,
            });

            if (result.OK)
            {
                // all good
            }
            else
            {
                Debug.Log($"Failed to start: {result.ShutdonwReason}");
            }
        }
        */
    }
}

