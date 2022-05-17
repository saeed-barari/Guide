using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused;
    
    PlayerController plr;
    
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject Player;

    void Start() {
        plr = Player.GetComponent<PlayerController>();
    }
    public void Pause() {
        isPaused = true;
        plr.cameraLock = true;
        plr.cameraLock = true;

        Time.timeScale = 0f;

        pauseUI.SetActive(true);
        hud.SetActive(false);
    }

    public void Resume() {
        isPaused = false;
        plr.cameraLock = false;
        plr.cameraLock = false;

        Time.timeScale = 1f;

        pauseUI.SetActive(false);
        hud.SetActive(true);
    }
}
