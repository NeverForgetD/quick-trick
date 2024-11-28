using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Splash
{
    public class UISplash : MonoBehaviour
    {
        public void OnEnable()
        {
            // ���� ���� �� ���� �� ��...��) PC�� ��� Ŀ�� ���̰�/�Ⱥ��̰�
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
