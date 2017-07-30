using System;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

public class ChargeStrategyConfig : MonoBehaviour, IEvent
{
    public Text ValuePowerPercentage;
    public Text ValuePower;
    public Text ValueTime;
    public Slider SliderPower;
    public GameObject Dialog;

    private StarPower starPower;
    private ShipResources shipResources;
    private EventRunner eventRunner;
    private float chargePower;
    private float chargeTime;
    private float startPower;
    private EventResult eventResult;
    

    void Start ()
    {
        this.SliderPower.onValueChanged.AddListener(this.SliderPowerChanged);

        this.eventResult = new EventResult
        {
            Cost1 = new EventResultItem { Name = "Time" },
            Gain1 = new EventResultItem { Name = "Power" }
        };

    }

    void Awake()
    {
        this.starPower = GameObject.FindObjectOfType<StarPower>();
        this.shipResources = GameObject.FindObjectOfType<ShipResources>();
        this.eventRunner = GameObject.FindObjectOfType<EventRunner>();
    }
	
	void Update ()
	{
	    this.ValueTime.text = float.IsInfinity(this.chargeTime) ? "∞" : $"{Mathf.Round(chargeTime)}";
	    this.ValuePower.text = $"{Mathf.Round(this.chargePower - this.shipResources.CurrentPower)}";
	    this.ValuePowerPercentage.text = $"{Mathf.Round(this.SliderPower.value * 100)}%";
    }

    public void HideDialog()
    {
        this.Dialog.SetActive(false);
    }

    public void ShowDialog()
    {
        this.Dialog.SetActive(true);

        this.SliderPower.minValue = this.shipResources.CurrentPowerPercentage;
        this.SliderPower.value = this.SliderPower.maxValue;

        this.chargePower = this.SliderPower.value * this.shipResources.MaxPower;
        this.chargeTime = this.shipResources.ChargeShipTime(starPower, this.chargePower);
    }
    
    private void SliderPowerChanged(float value)
    {
        this.chargePower = value * this.shipResources.MaxPower;
        this.chargeTime = this.shipResources.ChargeShipTime(starPower, this.chargePower);
    }

    public void ExecuteStep()
    {
        this.eventResult.Cost1.Value += 1;
        shipResources.ChargeShip(this.starPower, 1);
    }

    public void Execute()
    {
        this.startPower = this.shipResources.CurrentPower;
        this.eventResult.Cost1.Value = 0;
        this.eventResult.Gain1.Value = 0;
        this.eventRunner.AddEvents(this, (int) this.chargeTime);
    }

    public EventResult GetResult(bool wasAttacked)
    {
        if (wasAttacked)
        {
            eventResult.Title = "Charging interrupted!";
            eventResult.FlavourText = "They've found us - we are under attack! Quickly - retract the solar collectors!";
        }
        else
        {
            eventResult.Title = "Charging completed";
            eventResult.FlavourText = "Charging has been completed.";
        }

        eventResult.Gain1.Value = (int) (this.shipResources.CurrentPower - startPower);

        return eventResult;
    }

    public bool IsBattle()
    {
        return false;
    }
}
