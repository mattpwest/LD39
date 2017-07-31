using System;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Strategies;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class ProductionStrategyConfig : AbstractEvent
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

    private ShipResources shipResources;
    private EventRunner eventRunner;

    private float timeCost;
    private EventResult eventResult;
    private float startPopulation;
    private float startPower;
    private float startMetal;

    // Use this for initialization
    protected override void Start()
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

    protected override void Awake()
    {
        base.Awake();

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

    public override bool HasDialog()
    {
        return true;
    }

    public override void HideDialog()
    {
        this.Dialog.SetActive(false);
    }

    public override void ShowDialog()
    {
        base.ShowDialog();

        this.SliderWorkers.maxValue = this.shipResources.WorkersAvailable;
        this.SliderPopulation.maxValue = this.shipResources.MaxProducablePopulation;

        this.UpdateCostValues();
    }

    public override void ExecuteStep()
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

    public override EventResult GetResult(bool wasAttacked)
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

    public override bool IsBattle => false;
}
