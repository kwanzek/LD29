using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerSupply : ShipComponent {

	public int capacity;
	public int usage;
	public List<GameObject> suppliedNodes = new List<GameObject>();

	// Use this for initialization
	void Start () {
		capacity = 2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool addNode(GameObject node)
	{
		if(usage < capacity && !suppliedNodes.Contains(node))
		{
			suppliedNodes.Add (node);
			usage+=1;
			return true;
		}
		else return false;
	}

	public bool removeNode(GameObject node)
	{
		if(usage > 0)
		{
			suppliedNodes.Remove(node);
			usage-=1;
			return true;
		}
		return false;
	}
}
