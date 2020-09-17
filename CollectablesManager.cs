using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
	public PlayerHealth playerHealth;
	public GameObject collectables;
	public int ammoboxcount;
	public float spawnTime = 1f;
	public Transform[] spawnPoints;
	float ammoTimer;

	void Start ()
	{
		ammoboxcount = 0;
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}
	void Update() {
		ammoTimer += Time.deltaTime;
		if (ammoTimer >= 300f) {
			ammoTimer -= 300f;
			ammoboxcount--;
		}
	}

	void Spawn ()
	{
		if(playerHealth.currentHealth <= 0f || ammoboxcount>=5)
		{
			return;
		}

		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		Debug.Log ("Ammo spawn!");
		GameObject ammobox = Instantiate (collectables, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		ammoboxcount++;
		Destroy (ammobox, 300f);
	}
}
