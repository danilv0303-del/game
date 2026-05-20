using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeToMenu : MonoBehaviour
{
    [SerializeField] private string menuSceneName = "Menu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
        }
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // —брасываем врем€, если была пауза
        SceneManager.LoadScene(menuSceneName);
    }
}