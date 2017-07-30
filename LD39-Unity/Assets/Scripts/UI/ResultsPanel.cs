using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

public class ResultsPanel : MonoBehaviour
{
    public Text Title;
    public Text FlavourText;

    public Text Cost1Label;
    public Text Cost2Label;
    public Text Cost3Label;
    public Text Cost1Value;
    public Text Cost2Value;
    public Text Cost3Value;

    public Text Gain1Label;
    public Text Gain2Label;
    public Text Gain1Value;
    public Text Gain2Value;

    private EventResult _results;
    private EventRunner eventRunner;

    public EventResult Results
    {
        get { return _results; }
        set
        {
            _results = value;

            Title.text = _results.Title;
            FlavourText.text = _results.FlavourText;

            Cost1Label.text = "";
            Cost1Value.text = "";
            Cost2Label.text = "";
            Cost2Value.text = "";
            Cost3Label.text = "";
            Cost3Value.text = "";
            Gain1Label.text = "";
            Gain1Value.text = "";
            Gain2Label.text = "";
            Gain2Value.text = "";

            if (_results.Cost1 != null)
            {
                Cost1Label.text = _results.Cost1.Name;
                Cost1Value.text = $"{_results.Cost1.Value}";
            }

            if (_results.Cost2 != null)
            {
                Cost2Label.text = _results.Cost2.Name;
                Cost2Value.text = $"{_results.Cost2.Value}";
            }

            if (_results.Cost3 != null)
            {
                Cost3Label.text = _results.Cost3.Name;
                Cost3Value.text = $"{_results.Cost3.Value}";
            }

            if (_results.Gain1 != null)
            {
                Gain1Label.text = _results.Gain1.Name;
                Gain1Value.text = $"{_results.Gain1.Value}";
            }

            if (_results.Gain2 != null)
            {
                Gain2Label.text = _results.Gain2.Name;
                Gain2Value.text = $"{_results.Gain2.Value}";
            }
        }
    }

    void Start()
    {
        this.eventRunner = GameObject.FindObjectOfType<EventRunner>();
    }

    public void CloseDialog()
    {
        this.gameObject.SetActive(false);

        if (Results.OptionalNextEvent != null)
        {
            if (Results.OptionalNextEvent.HasDialog())
            {
                Results.OptionalNextEvent.ShowDialog();
            }
            else
            {
                this.eventRunner.AddEvents(Results.OptionalNextEvent, 1);
                this.eventRunner.ExecuteEvents();
            }
        }
    }
}
