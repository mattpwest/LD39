using UnityEngine;
using UnityEngine.UI;

public class ChargeStrategyConfig : MonoBehaviour
{
    public Text ValuePowerPercentage;
    public Text ValuePower;
    public Text ValueTime;
    public Slider SliderPower;
    public GameObject Dialog;

    private StarPower starPower;
    private ShipResources shipResources;
    private float chargePower;
    private float chargeTime;

    void Start () {
        
        this.SliderPower.onValueChanged.AddListener(this.SliderPowerChanged);
    }

    void Awake()
    {
        this.starPower = GameObject.FindObjectOfType<StarPower>();
        this.shipResources = GameObject.FindObjectOfType<ShipResources>();
    }
	
	void Update ()
	{
	    this.ValueTime.text = float.IsInfinity(this.chargeTime) ? "∞" : $"{Mathf.Round(chargeTime)}";
	    this.ValuePower.text = $"{Mathf.Round(this.chargePower)}";
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

    public void Execute()
    {
        var timePassed = 0;

        while (shipResources.CurrentPower < shipResources.MaxPower)
        {
            shipResources.ChargeShip(this.starPower, 1);

            timePassed++;
        }

        shipResources.TimePassed(timePassed);
    }
}
