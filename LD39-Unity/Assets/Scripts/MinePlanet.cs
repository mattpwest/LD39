using UnityEngine;

public class MinePlanet : MonoBehaviour
{
    public RectTransform dialog;
    public float Metal = 100.0f;
    public float MetalPerTimePerWorker = 0.5f;

    private MiningStrategyConfig miningStrategyConfig;

	void Start () {
	    var parent = GameObject.Find("Main Canvas");
	    var panel = Instantiate(dialog, parent.transform);
	    this.miningStrategyConfig = panel.GetComponentInChildren<MiningStrategyConfig>();
	    this.miningStrategyConfig.Planet = this;
        this.miningStrategyConfig.HideDialog();
	}
	
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        this.miningStrategyConfig.ShowDialog();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        this.miningStrategyConfig.HideDialog();
    }
}
