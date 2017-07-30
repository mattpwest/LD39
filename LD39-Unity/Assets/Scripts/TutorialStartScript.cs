using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    public BattleStrategyConfig BattleStrategyConfig;

    private Button mineExecuteButton;

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
        this.BattleStrategyConfig.OnBattleStarted.AddListener(this.OnBattleStarted);
	}

    private void OnBattleStarted()
    {
        this.BattleStrategyConfig.OnBattleStarted.RemoveListener(this.OnBattleStarted);

        if(this.AudioSource.isPlaying)
        {
            this.AudioSource.Stop();
        }

        this.Planet.SetActive(true);
        this.ChargeButton.SetActive(true);
        this.ProduceButton.SetActive(true);
        this.JumpButton.SetActive(true);
        this.ChargeExecuteButton.onClick.RemoveListener(this.OnChargeExecuteClick);
        this.StarProximityNotifier.OnProximityEntered.RemoveListener(this.OnStarProximityEntered);
        this.MinePlanet.OnDialogCreated.RemoveListener(this.OnPlanetDialogCreated);
        this.ProduceExecuteButton.onClick.RemoveListener(this.OnProduceExecuteClick);

        if (this.mineExecuteButton == null)
        {
            return;
        }

        this.mineExecuteButton.onClick.RemoveListener(this.OnMineExecuteClick);
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

        this.mineExecuteButton = dialogButtons.Single(x => x.name == "ButtonExecute");

        this.mineExecuteButton.onClick.AddListener(this.OnMineExecuteClick);
    }

    private void OnMineExecuteClick()
    {
        this.ProduceButton.SetActive(true);

        if(this.AudioSource.isPlaying)
        {
            this.AudioSource.Stop();
        }

        this.AudioSource.PlayOneShot(this.ProductionClip);

        if(this.mineExecuteButton == null)
        {
            return;
        }

        this.mineExecuteButton.onClick.RemoveListener(this.OnMineExecuteClick);
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