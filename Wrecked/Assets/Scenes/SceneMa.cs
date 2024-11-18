using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager2 : MonoBehaviour
{
    public string SceneName;

    void Start()
    {
    }

    public void ChangeScene()
    {
       
        SceneManager.LoadScene(SceneName);
    }
}