using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    public void OnPlayButtonClick()
    {
        // play버튼 비황성화 및 효과
        SceneManager.LoadScene(2);
    }

}