using Assets.Scripts.UI;
using Assets.Scripts.UI.Strategies;
using UnityEngine;
using UnityEngine.UI;

public class ChargeStrategyConfig : AbstractEvent
{
    public Text ValuePowerPercentage;
    public Text ValuePower;
    public Text ValueTime;
    public Slider SliderPower;

    private ShipResources shipResources;
    private EventRunner eventRunner;
    private float chargePower;
    private float chargeTime;
    private float startPower;
    private EventResult eventResult;
    
    protected override void Start ()
    {
        this.SliderPower.onValueChanged.AddListener(this.SliderPowerChanged);

        this.eventResult = new EventResult
        {
            Cost1 = new EventResultItem { Name = "Time" },
            Gain1 = new EventResultItem { Name = "Power" }
        };

    }

    protected override void Awake()
    {
        base.Awake();

        this.shipResources = GameObject.FindObjectOfType<ShipResources>();
        this.eventRunner = GameObject.FindObjectOfType<EventRunner>();
    }
	
	private void Update ()
	{
	    this.ValueTime.text = float.IsInfinity(this.chargeTime) ? "∞" : $"{Mathf.Round(chargeTime)}";
	    this.ValuePower.text = $"{Mathf.Round(this.chargePower - this.shipResources.CurrentPower)}";
	    this.ValuePowerPercentage.text = $"{Mathf.Round(this.SliderPower.value * 100)}%";
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

        this.SliderPower.minValue = this.shipResources.CurrentPowerPercentage;
        this.SliderPower.value = this.SliderPower.maxValue;

        this.chargePower = this.SliderPower.value * this.shipResources.MaxPower;
        this.chargeTime = this.shipResources.ChargeShipTime(this.chargePower);
    }
    
    private void SliderPowerChanged(float value)
    {
        this.chargePower = value * this.shipResources.MaxPower;
        this.chargeTime = this.shipResources.ChargeShipTime(this.chargePower);
    }

    public override void ExecuteStep()
    {
        this.eventResult.Cost1.Value += 1;
        shipResources.ChargeShip(1);
    }

    public void Execute()
    {
        this.startPower = this.shipResources.CurrentPower;
        this.eventResult.Cost1.Value = 0;
        this.eventResult.Gain1.Value = 0;
        this.eventRunner.AddEvents(this, (int) this.chargeTime);
    }

    public override EventResult GetResult(bool wasAttacked)
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

    public override bool IsBattle => false;
}
