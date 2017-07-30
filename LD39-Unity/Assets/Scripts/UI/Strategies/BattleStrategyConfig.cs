using System;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;

public class BattleStrategyConfig : MonoBehaviour, IEvent
{
    public Text ValueEnemies;
    public Text ValuePopulationAvailable;
    public Text ValueFighters;
    public Text ValueWorkers;

    public Text ValueTime;
    public Text ValuePower;
    public Text ValuePopulation;
    public Text ValueGain;

    public Slider SliderFighters;
    public Slider SliderWorkers;

    public GameObject Dialog;

    public float Enemies { get; set; }

    private EventRunner eventRunner;
    private ShipResources shipResources;

    private float timeCost;
    private float startPower;
    private float startPopulation;
    private EventResult eventResult;

    // Use this for initialization
	void Start ()
    {
        this.SliderFighters.onValueChanged.AddListener(this.ValueFightersChangeCheck);
        this.SliderWorkers.onValueChanged.AddListener(this.ValueWorkersChangeCheck);

        this.eventResult = new EventResult
        {
            Cost1 = new EventResultItem {Name = "Time", Value = 0},
            Cost2 = new EventResultItem {Name = "Power", Value = 0 },
            Cost3 = new EventResultItem {Name = "Population", Value = 0 }
        };
    }

    void Awake()
    {
	    this.shipResources = GameObject.FindObjectOfType<ShipResources>();
        this.eventRunner = GameObject.FindObjectOfType<EventRunner>();
    }

    // Update is called once per frame
    void Update ()
    {
        this.ValueEnemies.text = $"{Math.Round(this.Enemies)}";
        this.ValuePopulationAvailable.text = $"{this.shipResources.PopulationAvailable}";
        this.ValueFighters.text = $"{this.shipResources.Fighters}";
        this.ValueWorkers.text = $"{this.shipResources.Workers}";
    }

    public void HideDialog()
    {
        this.Dialog.SetActive(false);
    }

    public void ShowDialog()
    {
        this.Dialog.SetActive(true);

        this.shipResources.ResetFighters();
        this.shipResources.ResetWorkers();

        this.shipResources.SetActiveFighters((int)Math.Min(Enemies, shipResources.FightersAvailable));
        this.shipResources.SetActiveWorkers(shipResources.WorkersAvailable);

        this.SliderFighters.maxValue = this.shipResources.Population;

        this.SliderFighters.value = shipResources.Fighters;

        this.SliderWorkers.maxValue = (int) this.shipResources.Population * 0.9f;
        
        this.SliderWorkers.value = shipResources.Workers;
        
        this.UpdateCostValues();
    }

    private void ValueFightersChangeCheck(float value)
    {
        this.shipResources.SetActiveFighters((int) value);
        this.SliderWorkers.maxValue = this.shipResources.WorkersAvailable;
        
        this.UpdateCostValues();
    }

    private void ValueWorkersChangeCheck(float value)
    {
        this.shipResources.SetActiveWorkers((int) value);
        this.SliderFighters.maxValue = this.shipResources.FightersAvailable;

        this.UpdateCostValues();
    }

    private void UpdateCostValues()
    {
        if(this.shipResources.Workers == 0)
        {
            this.ValueTime.text = "∞";
            return;
        }

        var ourCasualtyRate = this.Enemies / this.shipResources.Fighters;
        var theirCasualtyRate = this.shipResources.Fighters / this.Enemies;

        var timeToLoss = this.shipResources.Population / ourCasualtyRate;
        var timeToWin = this.Enemies / theirCasualtyRate;
        var timeToJump = this.shipResources.TimeLeftToCalculateJump;

        this.timeCost = Mathf.Floor(Math.Min(timeToJump, Math.Min(timeToWin, timeToLoss)) + 1);
        this.ValueTime.text = $"{Math.Round(timeCost)}";
        
        var powerCost = this.shipResources.CurrentPowerConsumption * timeCost;
        this.ValuePower.text = $"{Math.Round(powerCost)}";

        this.ValuePopulation.text = $"{Math.Round(Math.Min(ourCasualtyRate * timeCost, shipResources.Population))}";

        if (timeToJump <= timeToWin && timeToJump <= timeToLoss)
        {
            this.ValueGain.text = "Escape";
        }
        else if (timeToWin <= timeToJump && timeToWin <= timeToLoss)
        {
            this.ValueGain.text = "Victory";
        }
        else if (timeToLoss <= timeToJump && timeToLoss <= timeToWin)
        {
            this.ValueGain.text = "Defeat";
        }
        else
        {
            this.ValueGain.text = "Uncertain";
        }
    }

    public void Execute()
    {
        this.eventResult.Cost1.Value = 0;
        this.eventResult.Cost2.Value = 0;
        this.eventResult.Cost3.Value = 0;

        this.startPopulation = this.shipResources.Metal;
        this.startPower = this.shipResources.CurrentPower;

        this.eventRunner.AddEvents(this, (int) Math.Ceiling(this.timeCost));
    }

    public void ExecuteStep()
    {
        this.eventResult.Cost1.Value += 1;
        this.Enemies = this.shipResources.Battle(Enemies, 1);
    }

    public EventResult GetResult(bool wasAttacked)
    {
        if (this.shipResources.Lost)
        {
            this.eventResult.Title = "Defeat!";
            this.eventResult.FlavourText = "They have destroyed us all...";
        }
        else if (this.Enemies < 1.0f)
        {
            this.eventResult.Title = "Victory!";
            this.eventResult.FlavourText = "We have defeated this terran fleet and live to fight another day!";
        }
        else
        {
            this.eventResult.Title = "Escape!";
            this.eventResult.FlavourText = "We have jumped away! It should take the terrans some time find us again...";
        }

        this.eventResult.Cost2.Value = (int) Math.Abs(this.startPower - this.shipResources.CurrentPower);
        this.eventResult.Cost3.Value = (int) (this.startPopulation - this.shipResources.Population);

        return this.eventResult;
    }

    public bool IsBattle()
    {
        return true;
    }
}
