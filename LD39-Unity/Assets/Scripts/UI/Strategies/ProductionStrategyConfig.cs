using System;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class ProductionStrategyConfig : MonoBehaviour, IEvent
{
    public Text ValueAvailable;
    public Text ValueWorkers;
    public Text ValuePopulation;
    public Text ValuePower;
    public Text ValueTime;
    public Text ValueMetal;
    public Text ValuePopulationGain;

    public Slider SliderPopulation;
    public Slider SliderWorkers;

    public GameObject Dialog;

    private ShipResources shipResources;
    private EventRunner eventRunner;

    private float timeCost;
    private EventResult eventResult;
    private float startPopulation;
    private float startPower;
    private float startMetal;

    // Use this for initialization
    void Start()
    {
        this.SliderPopulation.onValueChanged.AddListener(this.ValuePopulationChangeCheck);
        this.SliderWorkers.onValueChanged.AddListener(this.ValueWorkersChangeCheck);

        this.eventResult = new EventResult
        {
            Cost1 = new EventResultItem { Name = "Power" },
            Cost2 = new EventResultItem { Name = "Time" },
            Cost3 = new EventResultItem { Name = "Metal" },
            Gain1 = new EventResultItem { Name = "Population" }
        };
    }

    void Awake()
    {
        this.shipResources = GameObject.FindObjectOfType<ShipResources>();
        this.eventRunner = GameObject.FindObjectOfType<EventRunner>();
        this.SliderWorkers.maxValue = this.shipResources.WorkersAvailable;
        this.SliderPopulation.maxValue = this.shipResources.MaxProducablePopulation;
        this.UpdateCostValues();
    }

    // Update is called once per frame
    void Update()
    {
        this.ValueAvailable.text = $"{this.shipResources.PopulationAvailable}";
        this.ValueWorkers.text = $"{this.shipResources.Workers}";
        this.ValuePopulation.text = $"{this.SliderPopulation.value}";
    }

    public bool HasDialog()
    {
        return true;
    }

    public void HideDialog()
    {
        this.Dialog.SetActive(false);
    }

    public void ShowDialog()
    {
        this.Dialog.SetActive(true);

        this.SliderWorkers.maxValue = this.shipResources.WorkersAvailable;
        this.SliderPopulation.maxValue = this.shipResources.MaxProducablePopulation;

        this.UpdateCostValues();
    }

    public void ExecuteStep()
    {
        this.eventResult.Cost2.Value += 1;
        this.shipResources.ProducePopulation();
    }

    public void Execute()
    {
        this.eventResult.Cost2.Value = 0;

        this.startPower = this.shipResources.CurrentPower;
        this.startMetal = this.shipResources.Metal;
        this.startPopulation = this.shipResources.Population;

        this.eventRunner.AddEvents(this, (int) this.timeCost);
    }

    private void ValuePopulationChangeCheck(float value)
    {
        this.SliderPopulation.maxValue = this.shipResources.MaxProducablePopulation;

        this.UpdateCostValues();
    }

    private void ValueWorkersChangeCheck(float value)
    {
        this.shipResources.SetActiveWorkers((int)value);

        this.SliderWorkers.maxValue = this.shipResources.WorkersAvailable;

        this.UpdateCostValues();
    }

    private void UpdateCostValues()
    {
        var populationToProduce = (int)this.SliderPopulation.value;

        this.timeCost = populationToProduce / this.shipResources.PopulationProductionPerTime;
        var powerCost = this.shipResources.CurrentPowerConsumption * timeCost;
        var metalCost = populationToProduce * this.shipResources.MetalCostPerPopulation;

        this.ValuePower.text = powerCost.IsInfinityOrNan() ? "∞" : $"{Math.Round(powerCost)}";
        this.ValueTime.text = timeCost.IsInfinityOrNan() ? "∞" : $"{Math.Round(timeCost)}";
        this.ValueMetal.text = metalCost.IsInfinityOrNan() ? "∞" : $"{Math.Round(metalCost)}";
        this.ValuePopulationGain.text = $"{populationToProduce}";
    }

    public EventResult GetResult(bool wasAttacked)
    {
        if (wasAttacked)
        {
            this.eventResult.Title = "Production interrupted!";
            this.eventResult.FlavourText = "They've found us - we are under attack! Cancel current production run and switch to combat configurations!";
        }
        else
        {
            this.eventResult.Title = "Production completed";
            this.eventResult.FlavourText = "Production run of new robots has been completed.";
        }

        this.eventResult.Cost1.Value = (int) Math.Abs(this.startPower - this.shipResources.CurrentPower);
        this.eventResult.Cost3.Value = (int) (this.startMetal - this.shipResources.Metal);
        this.eventResult.Gain1.Value = (int) (this.shipResources.Population - this.startPopulation);

        return this.eventResult;
    }

    public bool IsBattle()
    {
        return false;
    }
}
