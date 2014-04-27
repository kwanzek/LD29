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
		usage = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool addNode(GameObject node)
	{

		ShipComponent shipcomp = node.GetComponent("ShipComponent") as ShipComponent;
		Debug.Log (shipcomp.index_Row + ", " + shipcomp.index_Column);


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

		Debug.Log ("FUCKING: "+ node.name);
		//ShipComponent shipcomp = node.GetComponent("ShipComponent") as ShipComponent;
		//Debug.Log (shipcomp.index_Row + ", " + shipcomp.index_Column);


		if(usage > 0)
		{
			bool bbb = suppliedNodes.Remove(node);
			Debug.Log ("WHAT?: "+ suppliedNodes.Count + ", " + bbb);
			Debug.Log ("HIIII: "+ suppliedNodes.Contains(node));
			usage-=1;
			return true;
		}
		return false;
	}
}
