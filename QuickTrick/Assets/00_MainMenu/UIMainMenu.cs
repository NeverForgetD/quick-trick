using UnityEngine;
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

    }


}
