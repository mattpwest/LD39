using UnityEngine;
using UnityEngine.UI;

public class ResourcesPanel : MonoBehaviour
{
    public Text ValuePower;
    public Text ValueTime;
    public Text ValuePopulation;
    public Text ValueMetal;

    public GameObject ProductionDialog;

    public GameObject ChargeDialog;

    public GameObject JumpDialog;

    private ShipResources shipResources;

    void Start ()
    {
        this.shipResources = GameObject.FindObjectOfType<ShipResources>();
    }
	
	void Update ()
	{
	    ValuePower.text = $"{Mathf.Round(shipResources.CurrentPower)}";
	    ValuePopulation.text = $"{Mathf.Round(shipResources.Population)}";
	    ValueTime.text = $"{Mathf.Round(shipResources.TimeLeft)}";
	    ValueMetal.text = $"{Mathf.Round(shipResources.Metal)}";
	}

    public void ExecuteChargeEvent()
    {
        ChargeDialog.GetComponentInChildren<ChargeStrategyConfig>().ShowDialog();
    }

    public void ShowProduceDialog()
    {
        this.ProductionDialog.GetComponentInChildren<ProductionStrategyConfig>().ShowDialog();
    }

    public void ShowJumpDialog()
    {
        this.JumpDialog.GetComponentInChildren<JumpStrategyConfig>().ShowDialog();
    }
}
