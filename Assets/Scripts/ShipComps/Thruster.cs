using UnityEngine;
using System.Collections;

public class Thruster : ShipComponent {

	public int speed;

	public AudioClip bubbles;

	float timer = 1.5f;

	// Use this for initialization
	void Start () {
		speed = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if(_powerLevel > 0)
		{
			if(timer <= 0.0)
			{
				AudioSource.PlayClipAtPoint(bubbles, Camera.main.transform.position);
				timer = 1.5f;
			}
		}
		timer-=Time.deltaTime;

	}
}
