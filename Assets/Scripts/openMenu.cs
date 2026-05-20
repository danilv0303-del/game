using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class openMenu : MonoBehaviour
{
    [SerializeField] private string menuSceneName = "Menu";
    public void OpenMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
