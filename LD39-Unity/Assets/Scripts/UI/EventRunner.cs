using System;
using System.Collections.Generic;
using Assets.Scripts.UI;
using UnityEngine;

public class EventRunner : MonoBehaviour
{
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
        foreach (var theEvent in events)
        {
            theEvent.ExecuteStep();

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

        // TODO: Pop results dialog


    }
}
