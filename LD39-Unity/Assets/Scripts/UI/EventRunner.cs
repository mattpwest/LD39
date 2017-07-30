using System.Collections.Generic;
using Assets.Scripts.UI;
using UnityEngine;

public class EventRunner : MonoBehaviour
{
    public int InitialEnemies = 10;
    public float EnemyGrowthFactor = 2.5f;

    public RectTransform battleDialog;
    public RectTransform resultsDialog;

    private int Enemies;
    private List<IEvent> events = new List<IEvent>();
    private ShipResources shipResources;

    void Start()
    {
        this.Enemies = InitialEnemies;
        this.shipResources = GameObject.FindObjectOfType<ShipResources>();
    }

    public void AddEvents(IEvent theEvent, int time)
    {
        for (var i = 0; i < time; i++)
        {
            this.events.Add(theEvent);
        }

        theEvent.HideDialog();

        this.ExecuteEvents();
    }

    public void ExecuteEvents()
    {
        bool found = false;
        IEvent lastEvent = null;
        foreach (var theEvent in events)
        {
            theEvent.ExecuteStep();
            lastEvent = theEvent;

            if (!lastEvent.IsBattle())
            {
                if (shipResources.WasFound)
                {
                    Debug.Log("We were found!");
                    found = true;
                    break;
                }
            }
        }

        events.Clear();

        var eventResult = lastEvent.GetResult(found);
        var dialog = resultsDialog.GetComponent<ResultsPanel>();
        dialog.Results = eventResult;

        if (found)
        {
            var battle = battleDialog.GetComponentInChildren<BattleStrategyConfig>();
            battle.Enemies = this.Enemies;
            this.Enemies = (int) (this.Enemies * this.EnemyGrowthFactor);
            dialog.NextEvent = battle;
        }

        dialog.gameObject.SetActive(true);
    }
}
