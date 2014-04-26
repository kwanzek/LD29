using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitConnection : MonoBehaviour {

	private List<GameObject> connectedPieces = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		foreach(GameObject obj in connectedPieces)
		{

		}

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
}
