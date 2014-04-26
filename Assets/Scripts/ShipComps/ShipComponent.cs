using UnityEngine;
using System.Collections;

public class ShipComponent : MonoBehaviour {

	//Sprite
	//What type of connection DC vs AC
	//What level of input
	//Position
	//Mass
	//Rotation
	//Damage level

	public int _inputLevel;
	public int _mass;
	public int _damageLevel;
	public int _powerType; 

	// Use this for initialization
	void Start () {
	
		_inputLevel = 0;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
