using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class TutorialStartScript : MonoBehaviour
{
    public Button ChargeExecuteButton;
    public Button ProduceExecuteButton;
    public AudioSource AudioSource;
    public AudioClip MiningClip;
    public AudioClip ProductionClip;
    public AudioClip JumpClip;
    public GameObject Planet;
    public GameObject ChargeButton;
    public GameObject ProduceButton;
    public GameObject JumpButton;
    public StarProximityNotifier StarProximityNotifier;
    public MinePlanet MinePlanet;

	// Use this for initialization
	void Start ()
	{
	    this.Planet.SetActive(false);
	    this.ChargeButton.SetActive(false);
	    this.ProduceButton.SetActive(false);
	    this.JumpButton.SetActive(false);
        this.ChargeExecuteButton.onClick.AddListener(this.OnChargeExecuteClick);
        this.StarProximityNotifier.OnProximityEntered.AddListener(this.OnStarProximityEntered);
        this.MinePlanet.OnDialogCreated.AddListener(this.OnPlanetDialogCreated);
        this.ProduceExecuteButton.onClick.AddListener(this.OnProduceExecuteClick);
	}
	
    private void OnChargeExecuteClick()
    {
        this.Planet.SetActive(true);

        if(this.AudioSource.isPlaying)
        {
            this.AudioSource.Stop();
        }

        this.AudioSource.PlayOneShot(this.MiningClip);

        this.ChargeExecuteButton.onClick.RemoveListener(this.OnChargeExecuteClick);
    }

    private void OnStarProximityEntered()
    {
        this.ChargeButton.SetActive(true);

        this.StarProximityNotifier.OnProximityEntered.RemoveListener(this.OnStarProximityEntered);
    }

    private void OnPlanetDialogCreated(RectTransform dialog)
    {
        var dialogButtons = dialog.GetComponentsInChildren<Button>();

        var button = dialogButtons.Single(x => x.name == "ButtonExecute");

        OnButtonClickPlayAudioClipOnce.PlayAudioOnce(this.AudioSource, this.ProductionClip, this.ProduceButton, button);
    }

    private void OnProduceExecuteClick()
    {
        this.JumpButton.SetActive(true);

        if(this.AudioSource.isPlaying)
        {
            this.AudioSource.Stop();
        }

        this.AudioSource.PlayOneShot(this.JumpClip);

        this.ProduceExecuteButton.onClick.RemoveListener(this.OnProduceExecuteClick);
    }
}