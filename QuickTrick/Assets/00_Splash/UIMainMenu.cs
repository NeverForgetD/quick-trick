using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class UIMainMenu : MonoBehaviour
    {
        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void QuitGame()
        {
            Application.Quit();

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
            #endif
        }

        public void OnEnable()
        {
            // 게임 시작 시 적용 할 것...예) PC의 경우 커서 보이게/안보이게
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
                LoadScene(1);
        }

        private bool IsPointerOverUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }


}
