﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class MiningStrategyConfig : MonoBehaviour
{
    public Text ValueAvailable;
    public Text ValueResource;
    public Text ValueWorkers;
    public Text ValueMetal;
    public Text ValuePower;
    public Text ValueTime;

    public Slider SliderResource;
    public Slider SliderWorkers;

    public GameObject Dialog;

    private ShipResources shipResources;
    public MinePlanet Planet { get; set; }

    private float metalMined;

    // Use this for initialization
	void Start ()
    {
        this.SliderResource.onValueChanged.AddListener(this.ValueResourceChangeCheck);
        this.SliderWorkers.onValueChanged.AddListener(this.ValueWorkersChangeCheck);
    }

    void Awake()
    {
	    this.shipResources = GameObject.FindObjectOfType<ShipResources>();
        this.SliderWorkers.maxValue = this.shipResources.WorkersAvailable;
    }

    // Update is called once per frame
    void Update ()
    {
        this.ValueAvailable.text = $"{this.shipResources.PopulationAvailable}";
        this.ValueResource.text = $"{Math.Round(this.SliderResource.value * 100.0f)}%";
        this.ValueWorkers.text = $"{this.shipResources.Workers}";

        if (this.Planet != null)
        {
            this.ValueMetal.text = $"{Math.Round(this.Planet.Metal * this.SliderResource.value)}";
        }
    }

    public void HideDialog()
    {
        this.Dialog.SetActive(false);
    }

    public void ShowDialog()
    {
        this.Dialog.SetActive(true);
        this.shipResources.ResetWorkers();
        this.SliderResource.value = 1.0f;
        this.UpdateCostValues();
    }

    private void ValueResourceChangeCheck(float value)
    {
        this.UpdateCostValues();
    }

    private void ValueWorkersChangeCheck(float value)
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

        this.metalMined = this.Planet.Metal * this.SliderResource.value;
        var timeCost =  this.metalMined / (this.Planet.MetalPerTimePerWorker * (Mathf.Log(this.shipResources.Workers + 2)));
        var powerCost = this.shipResources.CurrentPowerConsumption * timeCost;

        this.ValuePower.text = $"{Math.Round(powerCost)}";
        this.ValueTime.text = $"{Math.Round(timeCost)}";
    }

    public void Execute()
    {
        var targetMetal = shipResources.Metal + metalMined;

        while (shipResources.Metal < targetMetal)
        {
            shipResources.MineMetal(this.Planet, 1);
        }

        HideDialog();
    }
}