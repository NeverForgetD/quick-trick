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
        // play��ư ��Ȳ��ȭ �� ȿ��
        SceneManager.LoadScene(2);
    }

}