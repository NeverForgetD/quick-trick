using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Splash
{
    public class UISplash : MonoBehaviour
    {
        public void OnEnable()
        {
            // 게임 시작 시 적용 할 것...예) PC의 경우 커서 보이게/안보이게
        }
        public void OnExitButtonCLick()
        {
            QuitGame();
        }

        public void OnScreenClick()
        {
            LoadScene(1);
        }

        void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        void QuitGame()
        {
            Application.Quit();

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
            #endif
        }

        /*
        private bool IsPointerOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
        */
    }
}
