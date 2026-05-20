using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endLevels : MonoBehaviour
{
    [SerializeField] private string menuSceneName = "Menu";
    private void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
