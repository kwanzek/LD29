using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spaceship : MonoBehaviour {

	private List<GameObject> components = new List<GameObject>();

	private int _speed;
	private int _rotation_speed;
	private int _maxSpeed;

	// Use this for initialization
	void Start () {
		_speed = 150;
		_rotation_speed = 90;
		_maxSpeed = 200;
	}
	
	// Update is called once per frame
	void Update () {

		//Keep the ship on the screen for now

		//this.transform.position = new Vector2(this.transform.position.x + (_speed)*Time.deltaTime,
		//                                      this.transform.position.y + (_speed)*Time.deltaTime*0);

		//this.transform.Rotate(Vector3.forward * _rotation_speed*Time.deltaTime);

		float widthRel = (48*3) / (Screen.width); //relative width
		float heightRel= (32) /(Screen.height); //relative height
		
		Vector3 viewPos = Camera.main.WorldToViewportPoint (this.transform.position);
		viewPos.x = Mathf.Clamp(viewPos.x, widthRel, 1-widthRel);
		viewPos.y = Mathf.Clamp(viewPos.y, heightRel, 1-heightRel);
		this.transform.position = Camera.main.ViewportToWorldPoint (viewPos);




	}
}
