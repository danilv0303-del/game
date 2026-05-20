using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonexit : MonoBehaviour
{
    public GameObject menu;
    public GameObject levels;

    public void OpenLevels()
    {
        menu.SetActive(false);
        levels.SetActive(true);
    }
}