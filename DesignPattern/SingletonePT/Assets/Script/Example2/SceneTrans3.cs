using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrans3 : MonoBehaviour
{
    public void AddScene()
    {
        SceneManager.LoadScene("Scene3-2", LoadSceneMode.Additive);
    }

    public void RemoveScene()
    {
        SceneManager.UnloadSceneAsync("Scene3-2");
    }
}
