using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrategyConfiguration : MonoBehaviour
{
    public Text ValueAvailable;
    public Text ValueWorkers;
    public Text ValueFighers;

    public Slider SliderWorkers;
    public Slider SliderFighters;

    private ShipResources shipResources;

	// Use this for initialization
	void Start () {
	    this.shipResources = GameObject.FindObjectOfType<ShipResources>();

        this.SliderWorkers.onValueChanged.AddListener(this.ValueWorkersChangeCheck);
        this.SliderFighters.onValueChanged.AddListener(this.ValueFightersChangeCheck);
    }

    // Update is called once per frame
    void Update ()
    {
        this.ValueAvailable.text = $"{this.shipResources.PopulationAvailable}";
        this.ValueWorkers.text = $"{this.shipResources.Workers}";
        this.ValueFighers.text = $"{this.shipResources.Fighters}";

    }

    private void ValueWorkersChangeCheck(float value)
    {
        this.shipResources.SetActiveWorkers((int)value);

        this.UpdateSlidersMaxValue();
    }

    private void ValueFightersChangeCheck(float value)
    {
        this.shipResources.SetActiveFighters((int)value);

        this.UpdateSlidersMaxValue();
    }

    private void UpdateSlidersMaxValue()
    {
        this.SliderWorkers.maxValue = this.shipResources.WorkersAvailable;
        this.SliderFighters.maxValue = this.shipResources.FightersAvailable;
    }
}
