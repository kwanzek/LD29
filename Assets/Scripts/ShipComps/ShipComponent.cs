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

	public GameObject parent;
	public GameObject powerSupply;

	public int index_X;
	public int index_Y;

	// Use this for initialization
	void Start () {
	
		_inputLevel = 0;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool equals(ShipComponent other)
	{
		if(this.index_X == other.index_X && this.index_Y == other.index_Y)
			return true;
		else return false;
	}
}
