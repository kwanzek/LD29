using UnityEngine;
using System.Collections;

public class BubbleLife : MonoBehaviour {

	private float lifeTimer;

	private float currentSpeed;
	private Vector2 targetPoint;

	// Use this for initialization
	void Start () {
		lifeTimer = Random.Range(.66f, 2.0f);
	}
	
	// Update is called once per frame
	void Update () {

		lifeTimer -= Time.deltaTime;
		
		if(lifeTimer <= 0.0f)
		{
			Destroy(this.gameObject);
		}
	
	}
}
