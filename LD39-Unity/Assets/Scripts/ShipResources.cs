﻿using System;
using UnityEngine;

public class ShipResources : MonoBehaviour
{
    public float MaxPower = 200.0f;
    public float WorkerPowerConsumption = 0.5f;
    public float FighterPowerConsumption = 0.7f;
    public float PopulationPerMetal = 10.0f;
    public float PopulationPerWorker = 2.0f;

    public float CurrentPower { get; private set; }
    public float TimeLeft { get; private set; }
    public int Population { get; private set; }
    public int Workers { get; private set; }
    public int Fighters { get; private set; }
    public int Metal { get; private set; }

    public int PopulationAvailable => this.Population - this.Workers - this.Fighters;
    public int WorkersAvailable => this.Population - this.Fighters;
    public int FightersAvailable => this.Population - this.Workers;

    public float CurrentPowerConsumption => this.CurrentWorkerPowerConsumption + this.CurrentFighterPowerConsumption;

    private float CurrentWorkerPowerConsumption => this.Workers * this.WorkerPowerConsumption;
    private float CurrentFighterPowerConsumption => this.Fighters * this.FighterPowerConsumption;
    public float CurrentPowerPercentage => this.CurrentPower / this.MaxPower;

    public ShipResources()
    {
        this.CurrentPower = 0.0f;
        this.TimeLeft = 1000.0f;
        this.Population = 100;
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

    public float ChargeShipTime(StarPower star, float targetPower)
    {
        var deltaPower = targetPower - this.CurrentPower;
        if (deltaPower < 0)
        {
            return 0.0f;
        }

        var distanceVector = (Vector2)this.transform.position - star.Position;
        var distance = distanceVector.magnitude;
        if (distance > star.MaxDistance)
        {
            return float.PositiveInfinity;
        }

        var distanceMod = (star.MaxDistance - distance) / star.MaxDistance;
        return deltaPower / (distanceMod * star.PowerPerTime);
    }

    public void TimePassed(float time)
    {
        this.TimeLeft -= time;
    }

    public void SetActiveWorkers(int count)
    {
        if (count > this.WorkersAvailable)
        {
            throw new ArgumentOutOfRangeException("Available population is less than the count to activate!");
        }

        this.Workers = count;
    }

    public void SetActiveFighters(int count)
    {
        if (count > this.FightersAvailable)
        {
            throw new ArgumentOutOfRangeException("Available population is less than the count to activate!");
        }

        this.Fighters = count;
    }

    public void ResetPopulation()
    {
        this.Workers = 0;
        this.Fighters = 0;
    }
}
