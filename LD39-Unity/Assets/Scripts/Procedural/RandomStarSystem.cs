using UnityEngine;

public class RandomStarSystem : MonoBehaviour
{

	public int Seed;
	public Transform StarTemplate;
	public Transform PlanetTemplate;
	
	void Start () {
		Random.InitState(Seed);

		GenerateStar();

		GeneratePlanets();
	}

	private void GenerateStar()
	{
		var star = Instantiate(StarTemplate, transform.position, Quaternion.identity, transform);
		var starPower = star.GetComponent<StarPower>();
		var starRenderer = star.GetComponent<SpriteRenderer>();

		var scale = 1.0f + Random.Range(0.0f, 2.0f);
		star.localScale = new Vector3(scale, scale, 1.0f);
		starRenderer.color = Random.ColorHSV();
		starPower.MaxDistance = 10 + Random.Range(0, 10);
		starPower.PowerPerTime = 5 + Random.Range(0, 5);
	}

	private void GeneratePlanets()
	{
	}
	
	void Update () {
	}
}
