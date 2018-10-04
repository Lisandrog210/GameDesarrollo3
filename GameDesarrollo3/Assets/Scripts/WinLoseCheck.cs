using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseCheck : MonoBehaviour {
        
    public int level;
    [SerializeField] private int lives = 3;
    CheckpointManager cm;

    private void Start()
    {
        cm = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.tag == "WinCheck" )
        {
           LevelSelectManager.instance.IsLevelWon(level);
           WinScene();
        }        

        if(collider.tag == "LoseCheck" && lives > 0) {
            //CARGAR PANEL DE VOLVER AL CHECKPOINT
            RemoveLife();
            MoveToCheckpoint();
        }
        else if(collider.tag == "LoseCheck" && lives == 0 )
            GameOverScene();
    }

    private void MoveToCheckpoint()
    {
        if (cm.lastActivated)
        {
            Debug.Log("MOVE THE BALL!!!");
            //aca tiene que ser el ultimo checkpoint activado
            this.transform.position = cm.lastActivated.transform.position;
        }else
            GameOverScene();

    }

    private void RemoveLife()
    {
        Debug.Log("LIFE --");
        lives--;
    }

    public void WinScene() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void GameOverScene() {
        SceneManager.LoadScene("GameOverMenu");
    }
  

}
