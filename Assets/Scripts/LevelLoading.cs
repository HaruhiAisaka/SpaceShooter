using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoading : MonoBehaviour
{
    [SerializeField] int delaySec = 1;
    public void LoadStartMenu(){
        SceneManager.LoadScene(0);
    }

    public void LoadGame(){
        SceneManager.LoadScene("GameScene");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOver(){
        StartCoroutine(WaitAndLoad(delaySec,"GameOverScene"));
    }

    public IEnumerator WaitAndLoad(int sec,string scene){
        yield return new WaitForSeconds(sec);
        SceneManager.LoadScene(scene);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
