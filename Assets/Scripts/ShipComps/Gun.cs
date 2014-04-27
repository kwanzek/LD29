using UnityEngine;
using System.Collections;

public class Gun : ShipComponent {

	public float interval;
	public float intervalBase = 2.0f;

	// Use this for initialization
	void Start () {
		interval = intervalBase;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(_powerLevel);


		if (_powerLevel <= 0)
		{
			if(interval < intervalBase)
				interval -= Time.deltaTime;
			if(interval <= 0.0)
				interval = intervalBase;
		}
	}
}
