using UnityEngine;
using UnityEngine.Events;

public class GameSequenceHandler : MonoBehaviour
{
    public UnityEvent OnGameStart;

    private void Awake()
    {
        OnGameStart ??= new UnityEvent();
    }
    
    private void Start()
    {
        OnGameStart?.Invoke();
    }
}
