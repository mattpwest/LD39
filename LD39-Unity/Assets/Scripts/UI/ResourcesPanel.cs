using UnityEngine;
using UnityEngine.UI;

public class ResourcesPanel : MonoBehaviour
{
    public RectTransform ValuePower;
    public RectTransform ValueTime;

    public GameObject ChargeDialog;

    private ShipResources shipResources;
    private Text valuePower;
    private Text valueTime;

    private RechargeEvent rechargeEvent;

    void Start ()
    {
        this.shipResources = GameObject.FindObjectOfType<ShipResources>();
        var starPower = GameObject.FindObjectOfType<StarPower>();

        this.rechargeEvent = new RechargeEvent(starPower, shipResources);

        valuePower = ValuePower.GetComponent<Text>();
        valueTime = ValueTime.GetComponent<Text>();
    }
	
	void Update ()
	{
	    valuePower.text = shipResources.CurrentPower + "";
	    valueTime.text = shipResources.TimeLeft + "";
	}

    public void ExecuteChargeEvent()
    {
        ChargeDialog.GetComponentInChildren<ChargeStrategyConfig>().ShowDialog();
    }
}
