using SBR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndButtons : MonoBehaviour {
    public void NextLevel() {
        var ctrl = FindObjectOfType<RandomEnemySpawnController>();
        ctrl.Restart();
        Pause.paused = false;
    }

    public void BackToBase() {
        SceneManager.LoadScene("ShopScene");
        Pause.paused = false;
    }
}
