using UnityEngine;
using UnityEngine.UI;

public class StrategyConfiguration : MonoBehaviour
{
    public Text ValueAvailable;
    public Text ValueWorkers;
    public Text ValueFighers;
    public Text ValueMetal;
    public Text ValuePower;
    public Text ValueTime;

    public Slider SliderWorkers;
    public Slider SliderFighters;

    public GameObject Dialog;

    private ShipResources shipResources;
    public MinePlanet Planet { get; set; }

    // Use this for initialization
	void Start ()
    {
        this.SliderWorkers.onValueChanged.AddListener(this.ValueWorkersChangeCheck);
        this.SliderFighters.onValueChanged.AddListener(this.ValueFightersChangeCheck);
    }

    void Awake()
    {
	    this.shipResources = GameObject.FindObjectOfType<ShipResources>();
    }

    // Update is called once per frame
    void Update ()
    {
        this.ValueAvailable.text = $"{this.shipResources.PopulationAvailable}";
        this.ValueWorkers.text = $"{this.shipResources.Workers}";
        this.ValueFighers.text = $"{this.shipResources.Fighters}";

        if (this.Planet != null)
        {
            this.ValueMetal.text = $"{this.Planet.Metal}";
        }
    }

    public void HideDialog()
    {
        this.Dialog.SetActive(false);
    }

    public void ShowDialog()
    {
        this.Dialog.SetActive(true);
        this.shipResources.ResetPopulation();
        this.UpdateCostValues();
    }

    private void ValueWorkersChangeCheck(float value)
    {
        this.shipResources.SetActiveWorkers((int)value);

        this.UpdateSlidersMaxValue();
        this.UpdateCostValues();
    }

    private void ValueFightersChangeCheck(float value)
    {
        this.shipResources.SetActiveFighters((int)value);

        this.UpdateSlidersMaxValue();
        this.UpdateCostValues();
    }

    private void UpdateSlidersMaxValue()
    {
        this.SliderWorkers.maxValue = this.shipResources.WorkersAvailable;
        this.SliderFighters.maxValue = this.shipResources.FightersAvailable;
    }

    private void UpdateCostValues()
    {
        if(this.shipResources.Workers == 0)
        {
            this.ValuePower.text = "∞";
            this.ValueTime.text = "∞";
            return;
        }

        var timeCost = this.Planet.Metal / (this.Planet.MetalPerTimePerWorker * this.shipResources.Workers);
        var powerCost = this.shipResources.CurrentPowerConsumption * timeCost;

        this.ValuePower.text = $"{powerCost}";
        this.ValueTime.text = $"{timeCost}";
    }
}
