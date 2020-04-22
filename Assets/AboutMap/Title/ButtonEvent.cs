using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    public AudioSource Push;

    // Start is called before the first frame update
    void Start()
    {
        Push.Stop();
    }
    
    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene("Loading");
    }

    public void ChangeGameScene()
    {
        Push.Play();
        StartCoroutine(ChangeScene());
    }

    public void GameExit()
    {
        Push.Play();
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
