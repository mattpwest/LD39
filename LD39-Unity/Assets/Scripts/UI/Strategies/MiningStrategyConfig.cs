using System;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

public class MiningStrategyConfig : MonoBehaviour, IEvent
{
    public Text ValueAvailable;
    public Text ValueResource;
    public Text ValueWorkers;
    public Text ValueMetal;
    public Text ValuePower;
    public Text ValueTime;

    public Slider SliderResource;
    public Slider SliderWorkers;

    public GameObject Dialog;

    private EventRunner eventRunner;
    private ShipResources shipResources;
    public MinePlanet Planet { get; set; }

    private float metalMined;
    private int timeCost;
    private float startPower;
    private float startMetal;
    private EventResult eventResult;

    // Use this for initialization
	void Start ()
    {
        this.SliderResource.onValueChanged.AddListener(this.ValueResourceChangeCheck);
        this.SliderWorkers.onValueChanged.AddListener(this.ValueWorkersChangeCheck);

        this.eventResult = new EventResult
        {
            Cost1 = new EventResultItem {Name = "Power"},
            Cost2 = new EventResultItem {Name = "Time"},
            Gain1 = new EventResultItem {Name = "Metal"}
        };
    }

    void Awake()
    {
	    this.shipResources = GameObject.FindObjectOfType<ShipResources>();
        this.SliderWorkers.maxValue = this.shipResources.WorkersAvailable;
        this.eventRunner = GameObject.FindObjectOfType<EventRunner>();
    }

    // Update is called once per frame
    void Update ()
    {
        this.ValueAvailable.text = $"{this.shipResources.PopulationAvailable}";
        this.ValueResource.text = $"{Math.Round(this.SliderResource.value * 100.0f)}%";
        this.ValueWorkers.text = $"{this.shipResources.Workers}";

        if (this.Planet != null)
        {
            this.ValueMetal.text = $"{Math.Round(this.Planet.Metal * this.SliderResource.value)}";
        }
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
        this.shipResources.ResetWorkers();
        this.SliderResource.value = 1.0f;
        this.UpdateCostValues();
    }

    private void ValueResourceChangeCheck(float value)
    {
        this.UpdateCostValues();
    }

    private void ValueWorkersChangeCheck(float value)
    {
        this.shipResources.SetActiveWorkers((int)value);

        this.UpdateCostValues();
    }

    private void UpdateCostValues()
    {
        if(this.shipResources.Workers == 0)
        {
            this.ValuePower.text = "∞";
            this.ValueTime.text = "∞";
            return;
        }

        this.metalMined = this.Planet.Metal * this.SliderResource.value;
        this.timeCost =  (int) (this.metalMined / (this.Planet.MetalPerTimePerWorker * (Mathf.Log(this.shipResources.Workers + 2))));
        var powerCost = this.shipResources.CurrentPowerConsumption * timeCost;

        this.ValuePower.text = $"{Math.Round(powerCost)}";
        this.ValueTime.text = $"{timeCost}";
    }

    public void Execute()
    {
        this.eventResult.Cost1.Value = 0;
        this.eventResult.Cost2.Value = 0;
        this.eventResult.Gain1.Value = 0;

        this.startMetal = this.shipResources.Metal;
        this.startPower = this.shipResources.CurrentPower;

        this.eventRunner.AddEvents(this, timeCost);
    }

    public void ExecuteStep()
    {
        this.eventResult.Cost2.Value += 1;
        shipResources.MineMetal(this.Planet, 1);
    }

    public EventResult GetResult(bool wasAttacked)
    {
        if (wasAttacked)
        {
            this.eventResult.Title = "Mining interrupted!";
            this.eventResult.FlavourText = "They've found us - we are under attack! Abandon mining operation and switch to combat configurations!";
        }
        else
        {
            this.eventResult.Title = "Mining completed";
            this.eventResult.FlavourText = "Mining operation has been completed.";
        }

        this.eventResult.Cost1.Value = (int) Math.Abs(this.startPower - this.shipResources.CurrentPower);
        this.eventResult.Gain1.Value = (int) (this.shipResources.Metal - this.startMetal);

        return this.eventResult;
    }

    public bool IsBattle => false;
}
