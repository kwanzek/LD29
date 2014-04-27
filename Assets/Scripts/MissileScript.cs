using UnityEngine;
using System.Collections;

public class MissileScript : MonoBehaviour {

	public int speed;
	public Vector3 angleVector;

	// Use this for initialization
	void Start () {
		speed = 300;
	}
	
	// Update is called once per frame
	void Update () {
		if(angleVector != null)
		{
			float angle = angleVector.z+Mathf.PI/2;
			this.transform.Translate(new Vector3(Mathf.Cos(angle)*speed*Time.deltaTime,  Mathf.Sin (angle)*speed*Time.deltaTime,0));
		}
	}
}
