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

    void Start () {
        
        this.SliderPower.onValueChanged.AddListener(this.SliderPowerChanged);
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
        shipResources.ChargeShip(this.starPower, 1);
    }

    public void Execute()
    {
        Debug.Log((int) this.chargeTime);
        this.eventRunner.AddEvents(this, (int) this.chargeTime);
    }
}
