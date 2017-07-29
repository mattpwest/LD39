using UnityEngine;

public class MinePlanet : MonoBehaviour
{
    public RectTransform dialog;
    public float Metal = 100.0f;
    public float MetalPerTimePerWorker = 0.5f;

    private StrategyConfiguration mineStrategyConfiguration;

	void Start () {
	    var parent = GameObject.Find("Main Canvas");
	    var panel = Instantiate(dialog, parent.transform);
	    this.mineStrategyConfiguration = panel.GetComponentInChildren<StrategyConfiguration>();
	    this.mineStrategyConfiguration.Planet = this;
        this.mineStrategyConfiguration.HideDialog();
	}
	
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        this.mineStrategyConfiguration.ShowDialog();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        this.mineStrategyConfiguration.HideDialog();
    }
}
