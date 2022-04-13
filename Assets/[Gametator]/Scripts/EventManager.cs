using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<GameState> OnStateChanged;
    public static void _onStateChanged(GameState state)
    {
        OnStateChanged?.Invoke(state);
    }

    public static event Action OnCaught;
    public static void _onCaught()
    {
        OnCaught?.Invoke();
    }

    public static event Action OnAllOnBoard;
    public static void _onAllOnBoard()
    {
        OnAllOnBoard?.Invoke();
    }

    public static event Action OnSlaveFreed;
    public static void _onSlaveFreed()
    {
        OnSlaveFreed?.Invoke();
    }
}
