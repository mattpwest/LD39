using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class ProductionStrategyConfiguration : MonoBehaviour
{
    public Text ValueAvailable;
    public Text ValueWorkers;
    public Text ValuePopulation;
    public Text ValuePower;
    public Text ValueTime;
    public Text ValueMetal;

    public Slider SliderPopulation;
    public Slider SliderWorkers;

    public GameObject Dialog;

    private ShipResources shipResources;
    // Use this for initialization
    void Start()
    {
        this.SliderPopulation.onValueChanged.AddListener(this.ValuePopulationChangeCheck);
        this.SliderWorkers.onValueChanged.AddListener(this.ValueWorkersChangeCheck);
    }

    void Awake()
    {
        this.shipResources = GameObject.FindObjectOfType<ShipResources>();
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

    public void Execute()
    {
        var populationToProduce = (int)this.SliderPopulation.value;
        var populationProduced = 0.0f;

        while(populationProduced < populationToProduce)
        {
            populationProduced += this.shipResources.ProducePopulation();
        }

        this.HideDialog();
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

        var timeCost = populationToProduce / this.shipResources.PopulationProductionPerTime;
        var powerCost = this.shipResources.CurrentPowerConsumption * timeCost;
        var metalCost = populationToProduce * this.shipResources.MetalCostPerPopulation;

        this.ValuePower.text = powerCost.IsInfinityOrNan() ? "∞" : $"{Math.Round(powerCost)}";
        this.ValueTime.text = timeCost.IsInfinityOrNan() ? "∞" : $"{Math.Round(timeCost)}";
        this.ValueMetal.text = metalCost.IsInfinityOrNan() ? "∞" : $"{Math.Round(metalCost)}";
    }
}
