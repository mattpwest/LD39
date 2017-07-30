using System;
using System.Collections.Generic;
using Assets.Scripts.UI;
using UnityEngine;

public class EventRunner : MonoBehaviour
{
    public RectTransform resultsDialog;
    private List<IEvent> events = new List<IEvent>();
    private ShipResources shipResources;

    void Start()
    {
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

    private void ExecuteEvents()
    {
        bool found = false;
        IEvent lastEvent = null;
        foreach (var theEvent in events)
        {
            theEvent.ExecuteStep();
            lastEvent = theEvent;

            if (shipResources.WasFound)
            {
                Debug.Log("We were found!");
                found = true;
                break;
            }
        }

        if (found)
        {
            events.Clear();

            // TODO: Add combat event
        }

        var eventResult = lastEvent.GetResult(found);
        resultsDialog.GetComponent<ResultsPanel>().Results = eventResult;
        resultsDialog.gameObject.SetActive(true);


    }
}
