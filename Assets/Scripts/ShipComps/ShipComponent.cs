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

	public int _powerLevel;
	public int _mass;
	public int _damageLevel;
	public int _powerType;

	public GameObject parent;
	public GameObject powerSupply;

	public int index_Row;
	public int index_Column;

	// Use this for initialization
	void Start () {
	
		_powerLevel = 0;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool equals(ShipComponent other)
	{
		if(this.index_Row == other.index_Row && this.index_Column == other.index_Column)
			return true;
		else return false;
	}
}
