using UnityEngine;
using UnityEngine.UI;

public class ProductionStrategyConfiguration : MonoBehaviour
{
    public Text ValueAvailable;
    public Text ValueWorkers;
    public Text ValuePopulation;

    public Slider SliderPopulation;
    public Slider SliderWorkers;

    public GameObject Dialog;

    private ShipResources shipResources;
    // Use this for initialization
    void Start()
    {
        this.SliderPopulation.onValueChanged.AddListener(this.ValuePopulationChangeCheck);
        this.SliderWorkers.onValueChanged.AddListener(this.ValueWorkersChangeCheck);
    }

    void Awake()
    {
        this.shipResources = GameObject.FindObjectOfType<ShipResources>();
    }

    // Update is called once per frame
    void Update()
    {
        this.ValueAvailable.text = $"{this.shipResources.PopulationAvailable}";
        this.ValueWorkers.text = $"{this.shipResources.Workers}";
    }

    public void HideDialog()
    {
        this.Dialog.SetActive(false);
    }

    public void ShowDialog()
    {
        this.Dialog.SetActive(true);
    }

    private void ValuePopulationChangeCheck(float value)
    {
        
    }

    private void ValueWorkersChangeCheck(float value)
    {
        this.shipResources.SetActiveWorkers((int)value);

        this.SliderWorkers.maxValue = this.shipResources.WorkersAvailable;
    }

    private void UpdateCostValues()
    {
        if(this.shipResources.Metal == 0)
        {
            
        }

        var populationToProduce = this.shipResources.Metal / this.shipResources.PopulationPerMetal;
    }
}
