using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {

	public GameObject squidPrefab;
	public GameObject stingrayPrefab;



	private float timer;
	// Use this for initialization
	void Start () {
		timer = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;

		if(timer < 0.0f)
		{
			int rand = Random.Range(0,2);
			if(rand == 0)
			{
				Instantiate(squidPrefab, 
				                                            new Vector3(Random.Range(-3000,3000),Random.Range(-3000,3000),0), 
				                                            Quaternion.identity);

			}
			else
			{
				Instantiate(stingrayPrefab, 
				                                            new Vector3(Random.Range(-3000,3000),Random.Range(-3000,3000),0), 
				                                            Quaternion.identity);
			}

			timer = Random.Range(3.0f, 10.0f);
		}
	}
}
