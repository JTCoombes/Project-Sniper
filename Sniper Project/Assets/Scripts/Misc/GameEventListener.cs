using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> {  }

public class GameEventListener : MonoBehaviour
{
    public GameEvent GameEvent;

    public CustomGameEvent response;

    private void OnEnable()
    {
        GameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        GameEvent.UnregisterListener(this);
    }

    public void OneventRaised(Component sender, object data)
    {
        response.Invoke(sender, data);
    }
}
