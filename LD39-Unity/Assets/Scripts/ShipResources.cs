using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipResources : MonoBehaviour
{
    public float MaxPower = 200.0f;
    

    public float CurrentPower { get; private set; }
    public float TimeLeft { get; private set; }

    public ShipResources()
    {
        this.CurrentPower = 0.0f;
    }

    public void ChargeShip(StarPower star, int timeCharging)
    {
        var distanceVector = (Vector2)this.transform.position - star.Position;
        var distance = distanceVector.magnitude;

        if (distance > star.MaxDistance)
        {
            return;
        }

        var distanceMod = (star.MaxDistance - distance) / star.MaxDistance;

        this.CurrentPower += distanceMod * star.PowerPerTime * timeCharging;

        this.CurrentPower = Mathf.Min(this.CurrentPower, this.MaxPower);
    }

    public void TimePassed(float time)
    {
        this.TimeLeft -= time;
    }
}
