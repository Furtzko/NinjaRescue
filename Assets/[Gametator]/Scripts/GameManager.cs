using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static int TotalSavedNinja = 0; //Oyun boyunca kurtarılan sayısı
    public int SavedNinjaCount = 1; //Bölüm sonu gemiye binen sayısı

    private void Awake()
    {
        Instance = this;
        EventManager.OnSlaveFreed += SlaveFreed;
    }

    private void OnDestroy()
    {
        EventManager.OnSlaveFreed -= SlaveFreed;
    }

    private void Start()
    {
        UpdateGameState(GameState.LevelStarted);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        EventManager._onStateChanged(newState);
    }

    public void IncreaseNinjaCount()
    {
        SavedNinjaCount++;
        TotalSavedNinja++;
    }

    public void DecreaseNinjaCount()
    {
        SavedNinjaCount--;
    }

    private void SlaveFreed()
    {
        IncreaseNinjaCount();
    }

    public string GetSavedNinjaCount()
    {
        string countStr = (SavedNinjaCount - 1f).ToString();
        return countStr;
    }

}

public enum GameState
{
    LevelStarted, //tap to start ekranı
    PathDrawable, //swipe to draw ekranı
    InGame, //çizim ekranı- çizmiyor
    InGameDrawing, //çizim ekranı - çiziyor 
    PathCreated, //path çizim tamamlandı - yürümüyor
    PathCreatedMoving,//path çizim tamamlandı - yürüyor
    LevelCompleted, //player finishe ulaştı
    LevelFailed //yakalandı
}