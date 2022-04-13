using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    private GameObject touchToStart;
    private GameObject swipeToDraw;
    private GameObject holdToDraw;
    private GameObject playerTorus;
    private GameObject finishTorus;
    public GameObject levelCompletedUI;
    public GameObject levelFailUI;
    public GameObject inGameUI;

    private void Awake()
    {
        EventManager.OnStateChanged += GameStateChanged;
        EventManager.OnSlaveFreed += DelayedUpdateSlaveCountUI;
    }

    private void OnDestroy()
    {
        EventManager.OnStateChanged -= GameStateChanged;
        EventManager.OnSlaveFreed -= DelayedUpdateSlaveCountUI;
    }


    void Start()
    {
        touchToStart = transform.GetChild(0).gameObject;
        swipeToDraw = transform.GetChild(1).gameObject;
        holdToDraw = transform.GetChild(2).gameObject;
        playerTorus = transform.GetChild(3).gameObject;
        finishTorus = transform.GetChild(4).gameObject;
        levelCompletedUI = transform.GetChild(5).gameObject;
        levelFailUI = transform.GetChild(6).gameObject;
        inGameUI = transform.GetChild(7).gameObject;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (GameManager.Instance.State)
            {
                case GameState.LevelStarted:
                    GameManager.Instance.UpdateGameState(GameState.PathDrawable);
                    break;
                case GameState.PathDrawable:
                    GameManager.Instance.UpdateGameState(GameState.InGame);
                    break;
                case GameState.PathCreated:
                    GameManager.Instance.UpdateGameState(GameState.PathCreatedMoving);
                    break;
                default:
                    break;

            }
        }

    }


    private void GameStateChanged(GameState state)
    {
        touchToStart.SetActive(state == GameState.LevelStarted);
        swipeToDraw.SetActive(state == GameState.PathDrawable);
        playerTorus.SetActive(state == GameState.PathDrawable || state == GameState.InGame);
        finishTorus.SetActive(state == GameState.InGameDrawing);
        holdToDraw.SetActive(state == GameState.PathCreated);
        levelCompletedUI.SetActive(state == GameState.LevelCompleted);
        levelFailUI.SetActive(state == GameState.LevelFailed);
        inGameUI.SetActive(state == GameState.PathDrawable || state == GameState.InGame || 
            state == GameState.InGameDrawing || state == GameState.PathCreated || 
            state == GameState.PathCreatedMoving);

        if (state.Equals(GameState.LevelStarted))
        {
            touchToStart.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = SceneManager.GetActiveScene().name;
            touchToStart.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = "x " + GameManager.TotalSavedNinja.ToString();

            inGameUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SceneManager.GetActiveScene().name;
            UpdateSlaveCountUI();
        }

        if (state.Equals(GameState.LevelCompleted))
        {
            levelCompletedUI.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = "x " + GameManager.Instance.GetSavedNinjaCount();
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % 2);
    }

    private void UpdateSlaveCountUI()
    {
        inGameUI.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "x " + GameManager.Instance.GetSavedNinjaCount();
    }

    private void DelayedUpdateSlaveCountUI()
    {
        Invoke("UpdateSlaveCountUI", 0.1f);
    }


}
