using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechargeEvent
{
    private readonly StarPower star;
    private readonly ShipResources ship;

    public RechargeEvent(StarPower star, ShipResources ship)
    {
        this.star = star;
        this.ship = ship;
    }


    public void Execute()
    {
        var timePassed = 0;

        while (ship.CurrentPower < ship.MaxPower)
        {
            ship.ChargeShip(this.star, 1);

            timePassed++;
        }

        ship.TimePassed(timePassed);
    }
}
