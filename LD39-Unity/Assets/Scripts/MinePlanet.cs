using UnityEngine;

public class MinePlanet : MonoBehaviour
{
    public RectTransform dialog;
    public float Metal = 100.0f;
    public float MetalPerTimePerWorker = 0.5f;
    private RectTransform mineEventPanel;

	void Start () {
	    var parent = GameObject.Find("Main Canvas");
	    this.mineEventPanel = Instantiate(dialog, parent.transform);
        this.mineEventPanel.gameObject.SetActive(false);
	    this.mineEventPanel.GetComponentInChildren<StrategyConfiguration>().Planet = this;
	}
	
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        this.mineEventPanel.gameObject.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        this.mineEventPanel.gameObject.SetActive(false);
    }
}
