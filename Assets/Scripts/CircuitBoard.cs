using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitBoard : MonoBehaviour {

	private List<GameObject> shipComponents = new List<GameObject>();
	private List<CircuitConnection> circuitConnections = new List<CircuitConnection>();

	private GameObject currentConnection;
	private Vector2 initialClickLocation;

	public GameObject connectionPrefab;

	// Use this for initialization
	void Start () {
		GameObject[] objectArray = GameObject.FindGameObjectsWithTag("Thruster");
		
		foreach(GameObject obj in objectArray)
		{
			//ShipComponent shipComp = obj.GetComponent("ShipComponent") as ShipComponent;
			shipComponents.Add(obj);
		}

		objectArray = GameObject.FindGameObjectsWithTag("PowerSupply");
		
		foreach(GameObject obj in objectArray)
		{
			//ShipComponent shipComp = obj.GetComponent("ShipComponent") as ShipComponent;
			shipComponents.Add(obj);
		}

		currentConnection = null;

		Debug.Log (shipComponents.Count);
	}
	
	// Update is called once per frame
	void Update () {
	
		if(currentConnection!=null)
		{

			Vector3 curMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition );

			Vector2 differenceVector = new Vector2(curMouse.x - initialClickLocation.x, curMouse.y-initialClickLocation.y);


			//atan2 y/x
			//differenceVector = new Vector2(differenceVector.x/differenceVector.magnitude, differenceVector.y / differenceVector.magnitude);

			float angle = Mathf.Atan2(differenceVector.y , differenceVector.x);

			angle = angle*Mathf.Rad2Deg+90;

			
			Vector2 midpoint = new Vector2((curMouse.x+initialClickLocation.x) / 2, (curMouse.y+initialClickLocation.y)/2);
			
			currentConnection.transform.position = new Vector3(midpoint.x, midpoint.y, 0);

			//Magnitude of distance vector / height of sprite = scale


			if(differenceVector.magnitude >= 0.01f)
			{
				currentConnection.transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
				currentConnection.transform.localScale = new Vector3(currentConnection.transform.localScale.x, 
				                                                     differenceVector.magnitude / 64, 1);

			}
			else
			{
				currentConnection.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0);
				currentConnection.transform.localScale = new Vector3(currentConnection.transform.localScale.x, 0.01f, 1);

			}



			if(Input.GetMouseButtonUp(0))
			{
				CircuitConnection cirConnect = currentConnection.GetComponent("CircuitConnection") as CircuitConnection;

				Vector3 mouseUpLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition );
				Vector2 mouseUpVector2 = new Vector2(mouseUpLocation.x, mouseUpLocation.y);

				GameObject initialBlock=null;
				GameObject endBlock=null;

				foreach (GameObject obj in shipComponents)
				{
					Bounds objBoundingBox = obj.renderer.bounds;
					Vector3 objExtents = objBoundingBox.extents;
					
					Rect objBoundingRect = new Rect(objBoundingBox.center.x-objExtents.x, 
					                                objBoundingBox.center.y-objExtents.y, objExtents.x*2, objExtents.y*2);

					if(objBoundingRect.Contains(mouseUpLocation))
					{
						endBlock = obj;
					}
					if(objBoundingRect.Contains(initialClickLocation))
					{
						initialBlock = obj;
					}


				}


				if(initialBlock == null || endBlock == null)
				{
					GameObject.Destroy(currentConnection);
				}
				else
				{
					CircuitConnection cirConScript = currentConnection.GetComponent("CircuitConnection") as CircuitConnection;
					cirConScript.addConnectedPiece(endBlock);
					cirConScript.addConnectedPiece(initialBlock);
					Debug.Log ("Connection length: " +cirConScript.getConnectedPieces().Count);
				}

				currentConnection = null;
			}
		}

		else if(currentConnection == null)
		{
			if(Input.GetMouseButtonDown(0))
			{
				Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition );
				newPos.z = 0;
				currentConnection = (GameObject)Instantiate(connectionPrefab, newPos, Quaternion.identity);
				initialClickLocation = new Vector2(newPos.x, newPos.y);
			}


		}

	}
}
