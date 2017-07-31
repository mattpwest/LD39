using Assets.Scripts.UI;
using Assets.Scripts.UI.Strategies;
using UI.Events;
using UnityEngine;
using UnityEngine.UI;

public class JumpStrategyConfig : AbstractEvent
{
    public Text ValueWorkers;
    public Text ValueTime;
    public Text ValuePower;

    public Slider SliderWorkers;

    private EventResult eventResult;
    private ShipResources shipResources;
    private float startPower;
    private int timeCost;
    private EventRunner eventRunner;

    // Use this for initialization
    protected override void Start()
    {
        this.SliderWorkers.onValueChanged.AddListener(this.SliderWorkersChanged);

        this.eventResult = new EventResult
                           {
                               Cost1 = new EventResultItem { Name = "Time" },
                               Cost2 = new EventResultItem { Name = "Power" }
                           };
    }

    protected override void Awake()
    {
        base.Awake();

        this.shipResources = GameObject.FindObjectOfType<ShipResources>();
        this.SliderWorkers.maxValue = this.shipResources.WorkersAvailable;
        this.eventRunner = GameObject.FindObjectOfType<EventRunner>();
    }

    // Update is called once per frame
    void Update ()
	{
	    this.ValueWorkers.text = $"{this.shipResources.Workers}";
	}

    private void SliderWorkersChanged(float value)
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

        this.timeCost = (int)this.shipResources.TimeLeftToCalculateJump;
        var powerCost = this.shipResources.CurrentPowerConsumption * this.timeCost;

        this.ValueTime.text = $"{this.timeCost}";
        this.ValuePower.text = $"{Mathf.Round(powerCost)}";
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
        this.shipResources.ResetWorkers();
    }

    public void Execute()
    {
        this.eventResult.Cost1.Value = 0;
        this.eventResult.Cost2.Value = 0;

        this.startPower = this.shipResources.CurrentPower;

        this.eventRunner.AddEvents(this, this.timeCost);
    }

    public override void ExecuteStep()
    {
        this.eventResult.Cost1.Value += 1;
        this.shipResources.PrepareForJump(1);
    }

    public override EventResult GetResult(bool wasAttacked)
    {
        if(wasAttacked)
        {
            this.eventResult.Title = "Jump calculation interrupted!";
            this.eventResult.FlavourText =
                "They've found us - we are under attack! Abandon jump calculation operation and switch to combat configurations!";
        }
        else
        {
            this.eventResult.Title = "Jump successful!";
            this.eventResult.FlavourText = "We have jumped to the next system! It should take the terrans some while longer to find us...";
            this.eventResult.OptionalNextEvent = new JumpEvent(
                shipResources,
                "Epsilon Eridani",
                "We have arrived in the Epsilon Eridani system. It will take the terrans some time to search all the " +
                "systems we may have jumped to, which gives us some time to prepare for our next jump..."
            );
        }

        this.eventResult.Cost2.Value = (int)Mathf.Abs(this.startPower - this.shipResources.CurrentPower);

        return this.eventResult;
    }

    public override bool IsBattle => false;
}
