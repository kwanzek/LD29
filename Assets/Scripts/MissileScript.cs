using UnityEngine;
using System.Collections;

public class MissileScript : MonoBehaviour {

	public int speed;
	public Vector3 angleVector;

	private float lifeTimer = 10.0f;

	// Use this for initialization
	void Start () {
		speed = 300;

	}
	
	// Update is called once per frame
	void Update () {
		if(angleVector != null)
		{
			float angle = angleVector.z*Mathf.Deg2Rad+Mathf.PI/2;

			this.transform.position = new Vector2(this.transform.position.x + Mathf.Cos(angle)*Time.deltaTime*speed,
			                                      this.transform.position.y + Mathf.Sin (angle) * Time.deltaTime * speed);

			//this.transform.Translate(new Vector3(Mathf.Cos(angle)*Time.deltaTime,  Mathf.Sin (angle)*Time.deltaTime,    0));
		}

		lifeTimer -= Time.deltaTime;

		if(lifeTimer <= 0.0f)
		{
			Destroy(this.gameObject);
		}
	}
}
