using System.Collections;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [Header("Managers")]
    public MapGenerator mapGenerator;
    public CoinManager coinManager;
    public AudioManager audioManager;
    public FXManager fxManager;

    
    [Header("Elements")]
    public Player player;
    public CameraHolder cameraHolder;
    public CarDetector carDetector;

    [Header("UI")]
    public FailUI failUI;
    public PlayerScoreUI playerScoreUI;
    public CoinUI coinUI;
    public MainMenu mainMenu;
    void Start()
    {
        mainMenu.RestartMainMenu();  
        mainMenu.Show();
    }

    public void UpdatePlayerScore(int playerScore)
    {
        playerScoreUI.UpdatePlayerScore(playerScore);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }

    }
    public void DeleteRow(int rowCount)
    {
        StartCoroutine(DeleteRowCoroutine(rowCount));
    }
    IEnumerator DeleteRowCoroutine(int rowCount)
    {
        for(int i = 0; i < rowCount; i++)
        {
            mapGenerator.DeleteRow();
            yield return new WaitForSeconds(.1f);
        }
    }
    public void ResetLevel()
    {
        player.ResetPlayer();
        cameraHolder.ResetCameraHolder();
        mapGenerator.DeleteMap();
        mapGenerator.AddNewRows(mapGenerator.mapZLength);
        failUI.RestartFailUI();
        coinUI.Show();
        coinUI.UpdateCoinCount();
        carDetector.gameObject.SetActive(true);
    }
}
