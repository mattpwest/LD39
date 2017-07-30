using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

public class JumpStrategyConfig : MonoBehaviour, IEvent
{
    public Text ValueWorkers;
    public Text ValueTime;
    public Text ValuePower;

    public Slider SliderWorkers;

    public GameObject Dialog;

    private EventResult eventResult;
    private ShipResources shipResources;
    private float startPower;
    private int timeCost;
    private EventRunner eventRunner;

    // Use this for initialization
    void Start()
    {
        this.SliderWorkers.onValueChanged.AddListener(this.SliderWorkersChanged);

        this.eventResult = new EventResult
                           {
                               Cost1 = new EventResultItem { Name = "Time" },
                               Cost2 = new EventResultItem { Name = "Power" }
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

    public void HideDialog()
    {
        this.Dialog.SetActive(false);
    }

    public void ShowDialog()
    {
        this.Dialog.SetActive(true);

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

    public void ExecuteStep()
    {
        this.eventResult.Cost1.Value += 1;
        this.shipResources.PrepareForJump(1);
    }

    public EventResult GetResult(bool wasAttacked)
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
        }

        this.eventResult.Cost2.Value = (int)Mathf.Abs(this.startPower - this.shipResources.CurrentPower);

        return this.eventResult;
    }

    public bool IsBattle => false;
}
