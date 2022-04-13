using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 activePosition; //Vector3(-3.15f, 1.4f, -2.4f);
    private Quaternion activeRotation; //Quaternion.Euler(-10f, 192f, 0f);

    private Vector3 levelStartPosition = new Vector3(0.1f, 0.8f, -2f);
    private Quaternion levelStartRotation = Quaternion.Euler(-3f, 180f, 0f);

    private Vector3 inGamePosition = new Vector3(0, 14.3f, -4f);
    private Quaternion inGameRotation = Quaternion.Euler(72.322f, 0f, 0f);

    private Vector3 levelCompletePosition = new Vector3(2.8f, 2.3f, 0f);
    private Quaternion levelCompleteRotation = Quaternion.Euler(0.5f, 352f, 0f);

    private void Awake()
    {
        EventManager.OnStateChanged += GameStateChanged;
    }

    private void OnDestroy()
    {
        EventManager.OnStateChanged -= GameStateChanged;
    }

    private void Start()
    {
        StartCoroutine(InitPosition());
        
    }

    IEnumerator InitPosition()
    {
        yield return new WaitForSeconds(0.5f);

        activePosition = levelStartPosition;
        activeRotation = levelStartRotation;
    }

    void Update()
    {
        transform.position = Vector3.Slerp(transform.position, activePosition, 1.2f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, activeRotation, 1.2f * Time.deltaTime);

    }

    private void GameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.PathDrawable:
                activePosition = inGamePosition;
                activeRotation = inGameRotation;
                break;
            case GameState.LevelCompleted:
                activePosition = levelCompletePosition;
                activeRotation = levelCompleteRotation;
                break;
            default:
                break;
        }
    }
}
