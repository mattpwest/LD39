using System;
using UnityEngine;

public class ShipResources : MonoBehaviour
{
    public float MaxPower = 200.0f;
    public float MaxMetal = 1000.0f;
    public float WorkerPowerConsumption = 0.5f;
    public float FighterPowerConsumption = 0.7f;
    public float MetalCostPerPopulation = 10.0f;
    public float PopulationPerTimePerWorker = 2.0f;
    public float HighRiskHours = 8.0f;
    public float LowRiskHours = 24.0f;

    public float CurrentPower { get; private set; }
    public float TimeLeft { get; private set; }
    public float Metal { get; private set; }
    public float Population { get; private set; }
    public int Workers { get; private set; }
    public int Fighters { get; private set; }

    public int PopulationAvailable => (int)this.Population - this.Workers - this.Fighters;
    public int WorkersAvailable => (int)this.Population - this.Fighters;
    public int FightersAvailable => (int)this.Population - this.Workers;

    public float CurrentPowerPercentage => this.CurrentPower / this.MaxPower;
    public int MaxProducablePopulation => (int)(this.Metal / this.MetalCostPerPopulation);
    public float WorkerProductionRate => Mathf.Log(this.Workers + 1);
    public float PopulationProductionPerTime => this.WorkerProductionRate * this.PopulationPerTimePerWorker;
    public bool WasFound {
        get {
            float roll = UnityEngine.Random.Range(0.0f, 1.0f);
            if (TimeLeft <= 0)
            {
                return roll <= 1.0f;
            }
            if (TimeLeft <= HighRiskHours * Risk)
            {
                return roll <= 0.3f;
            }
            if (TimeLeft <= LowRiskHours * Risk)
            {
                return roll <= 0.05f;
            }
            return false;
        }
    }

    public float CurrentPowerConsumption => this.CurrentWorkerPowerConsumption + this.CurrentFighterPowerConsumption;

    private float CurrentWorkerPowerConsumption => this.Workers * this.WorkerPowerConsumption;
    private float CurrentFighterPowerConsumption => this.Fighters * this.FighterPowerConsumption;

    private float Risk = 1.0f;

    public ShipResources()
    {
        this.CurrentPower = MaxPower * 0.1f;
        this.Metal = MaxMetal * 0.1f;
        this.TimeLeft = 48.0f;
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

        this.TimePassed(timeCharging);
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

    public float ProducePopulation()
    {
        var populationProduced = this.PopulationProductionPerTime;

        this.Metal -= populationProduced * this.MetalCostPerPopulation;

        this.Population += populationProduced;

        this.TimePassed(1.0f);

        this.ConsumePower();

        return populationProduced;
    }

    private void ConsumePower()
    {
        this.CurrentPower -= this.CurrentPowerConsumption;
    }

    public float MineMetal(MinePlanet planet, int timeMining)
    {
        var metalMined = planet.MetalPerTimePerWorker * timeMining * Mathf.Log(Workers + 2);
        this.Metal = Mathf.Min(this.Metal + metalMined, this.MaxMetal);
        this.TimePassed(timeMining);
        this.PowerConsumed(this.Workers * this.WorkerPowerConsumption);
        return metalMined;
    }

    private void PowerConsumed(float powerConsumed)
    {
        this.CurrentPower = Math.Max(0.0f, this.CurrentPower - powerConsumed);
    }

    public void TimePassed(float timePassed)
    {
        this.TimeLeft = Mathf.Max(0.0f, this.TimeLeft - timePassed);
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

    public void ResetWorkers()
    {
        this.Workers = 0;
    }

    public void ResetFighters()
    {
        this.Fighters = 0;
    }

    public void IncreaseRisk(float percentage)
    {
        this.Risk += this.Risk * percentage;
    }

    public void ResetRisk()
    {
        this.Risk = 1.0f;
    }
}
