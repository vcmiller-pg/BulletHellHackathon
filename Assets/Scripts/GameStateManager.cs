using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager inst { get; private set; }
    public float loseTime = 2;

    public int shotsHit { get; set; }
    public int shotsTaken { get; set; }
    public int enemiesDestroyed { get; set; }
    public int enemiesEscaped { get; set; }
    public float damageTaken { get; set; }

    private void Awake() {
        inst = this;
    }

    public void WinGame() {
        var screen = MessageManager.inst.ShowScreen(2);
        var text = screen.transform.GetChild(1).GetComponent<Text>();
        PopulateStats(text);
    }

    private void PopulateStats(Text text) {
        text.text = 
            $@"Enemies Destroyed: {enemiesDestroyed}
Enemies Escaped: {enemiesEscaped}
Accuracy: {Mathf.RoundToInt(shotsHit * 100.0f / shotsTaken)}%
Damage Taken: {Mathf.RoundToInt(damageTaken)}";
    }

    public void LoseGame() {
        Invoke("RestartLevel", loseTime);
        MessageManager.inst.ShowScreen(3);
    }

    private void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
