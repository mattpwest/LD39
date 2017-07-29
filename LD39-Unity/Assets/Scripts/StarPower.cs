using UnityEngine;

public class StarPower : MonoBehaviour
{
    public float PowerPerTime = 10.0f;
    public float MaxDistance = 20.0f;

    public Vector2 Position => this.transform.position;
}
