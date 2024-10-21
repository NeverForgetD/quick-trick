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
            // ���� ���� �� ���� �� ��...��) PC�� ��� Ŀ�� ���̰�/�Ⱥ��̰�
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
