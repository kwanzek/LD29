using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitConnection : MonoBehaviour {

	private List<GameObject> connectedPieces = new List<GameObject>();

	public int timeStamp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	}


	public void addConnectedPiece(GameObject addedComponent)
	{
		connectedPieces.Add(addedComponent);
	}

	public void removeConnectedPiece(GameObject removedComponent)
	{
		connectedPieces.Remove(removedComponent);
	}

	public List<GameObject> getConnectedPieces()
	{
		return connectedPieces;
	}

	public void depower(GameObject importantPiece)
	{
		foreach(GameObject piece in connectedPieces)
		{
			if(piece!= null)
			{
				if(piece.name.Contains("powersupply"))
			 	{
					PowerSupply shipComp = piece.GetComponent("PowerSupply") as PowerSupply;
					shipComp.removeNode(importantPiece);

				}
				else
				{
					ShipComponent shipComp = piece.GetComponent("ShipComponent") as ShipComponent;
					shipComp._numConnections -= 1;
					shipComp._powerLevel -= 100;
				}
			}
		}
	}

}
