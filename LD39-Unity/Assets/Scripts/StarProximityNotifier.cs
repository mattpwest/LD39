using UnityEngine;
using UnityEngine.Events;

public class StarProximityNotifier : MonoBehaviour
{
    public UnityEvent OnProximityEntered { get; }

    public StarProximityNotifier()
    {
        this.OnProximityEntered = new UnityEvent();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        this.OnProximityEntered.Invoke();
    }
}
