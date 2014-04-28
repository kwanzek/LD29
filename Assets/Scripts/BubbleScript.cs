using UnityEngine;
using System.Collections;

public class BubbleScript : MonoBehaviour {

	public GameObject bubble1;
	public GameObject bubble2;
	public GameObject bubble3;
	public GameObject bubble4;

	private float timer;

	public float minNoise = -30.0f;
	public float maxNoise = 30.0f;

	// Use this for initialization
	void Start () {
	
		timer = 0.01f;

	}
	
	// Update is called once per frame
	void Update () {
	
		if(timer <= 0.0f)
		{
			timer=Random.Range (0.02f, 0.1f);

			for(int i = 0; i < Random.Range (1,3); ++i)
			{
				int bubbleRand = Random.Range (1,11);

				GameObject whatToSpawn = null;

				if(bubbleRand < 4)
				{
					whatToSpawn = bubble1;
				}
				else if(bubbleRand > 3 && bubbleRand < 7)
				{
					whatToSpawn = bubble2;
				}
				else if (bubbleRand > 6 && bubbleRand != 10)
				{
					whatToSpawn = bubble3;
				}
				else if (bubbleRand == 10)
				{
					whatToSpawn = bubble4;
				}
				else
				{
					whatToSpawn = bubble2;
				}



				Vector2 noise = new Vector2(this.gameObject.transform.position.x + Random.Range(minNoise, maxNoise),
				                            this.gameObject.transform.position.y + Random.Range(minNoise, maxNoise));


				Instantiate(whatToSpawn, 
				            new Vector3(noise.x, noise.y, 0), Quaternion.identity);
			}


		}

		timer-= Time.deltaTime;

	}
}
