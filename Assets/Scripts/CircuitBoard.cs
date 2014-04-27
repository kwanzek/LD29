using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitBoard : MonoBehaviour {

	public GameObject[,] componentArray;
	private int[,] shipMap;
	public List<CircuitConnection> circuitConnections = new List<CircuitConnection>();

	private GameObject currentConnection;
	private Vector2 initialClickLocation;

	public GameObject connectionPrefab;
	public GameObject circuitboardPrefab;

	public GameObject circuitPassthroughPrefab;
	public GameObject circuitGunPrefab;
	public GameObject circuitPowerSupplyPrefab;
	public GameObject circuitThrusterPrefab;

	private int circuitBoardWidth = 300;
	private int circuitBoardHeight = 720;
	private int screenWidth = 1280;
	private int screenHeight = 720;

	private int maxWidth;
	private int maxHeight;

	private int tileX = 64;
	private int tileY = 64;

	// Use this for initialization
	void Start () {

		/*
		GameObject[] objectArray = GameObject.FindGameObjectsWithTag("Thruster");
		
		foreach(GameObject obj in objectArray)
		{
			//ShipComponent shipComp = obj.GetComponent("ShipComponent") as ShipComponent;
			//shipComponents.Add(obj);
		}

		objectArray = GameObject.FindGameObjectsWithTag("PowerSupply");
		
		foreach(GameObject obj in objectArray)
		{
			//ShipComponent shipComp = obj.GetComponent("ShipComponent") as ShipComponent;
			//shipComponents.Add(obj);
		}

		currentConnection = null;*/

		GameObject  q = (GameObject) Instantiate(circuitboardPrefab, new Vector3(screenWidth/2-circuitBoardWidth/2,
		                                                                         0, 0f), Quaternion.identity);

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

				foreach (GameObject obj in componentArray)
				{
					if(obj == null)
						continue;
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

				//is there enough power?
				//do you have power?

				if(initialBlock == null || endBlock == null || (initialBlock == endBlock))
				{
					GameObject.Destroy(currentConnection);
				}
				else
				{
					//breadth first search of space to construct connections along grid
					//CircuitConnection cirConScript = currentConnection.GetComponent("CircuitConnection") as CircuitConnection;
					//cirConScript.addConnectedPiece(endBlock);
					//cirConScript.addConnectedPiece(initialBlock);
					GameObject.Destroy(currentConnection);
					breadthFirstPath(initialBlock, endBlock);
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

	public void setGameObjectArray(ref int[,] map, ref int maxWidth, ref int maxHeight)
	{
		shipMap = map;
		setupCircuitBoard(maxWidth, maxHeight);
	}

	private void breadthFirstPath(GameObject beginningObject, GameObject endingObject)
	{
		Queue<GameObject> Q = new Queue<GameObject>();
		List<GameObject> V = new List<GameObject>();


		Q.Enqueue(beginningObject);
		V.Add(beginningObject);

		while(Q.Count > 0)
		{
			GameObject expandedNode = Q.Dequeue();
			ShipComponent script = expandedNode.GetComponent("ShipComponent") as ShipComponent;

			if(expandedNode == endingObject)
			{
				GameObject tempNode = endingObject;

				ShipComponent nodeScript = tempNode.GetComponent("ShipComponent") as ShipComponent;


				//IMPORTANT STUFF HERE

				if(beginningObject.name.Contains("powersupply"))
				{
					PowerSupply powerScript = beginningObject.GetComponent("PowerSupply") as PowerSupply;
					if(powerScript.addNode(endingObject))
					{
						nodeScript.powerSupply =beginningObject;
						nodeScript._powerLevel = 100;
						Debug.Log ("LINK CREATED");
					}
					else
					{
						Debug.Log ("THIS NODE IS OVERCAPACITY, CANNOT CREATE LINK");
					}

				}
				//ending
				while(tempNode != beginningObject)
				{
					//create connection
					ShipComponent nodeScript2 = tempNode.GetComponent("ShipComponent") as ShipComponent;
					tempNode = nodeScript2.parent;
				}
			}

			//get neighbors
			List<GameObject> neighbors = getNeighborsOfNode(script.index_Row, script.index_Column);
			//Debug.Log ("Count: " + neighbors.Count + " index_row: " + script.index_Row + ", index_col: " + script.index_Column);
			foreach(GameObject neighbor in neighbors)
			{
				//Debug.Log ("Nothing");
				if(!V.Contains(neighbor) && neighbor != null)
				{
					ShipComponent neighborScript = neighbor.GetComponent("ShipComponent") as ShipComponent;
					neighborScript.parent = expandedNode;
					V.Add(neighbor);
					Q.Enqueue(neighbor);
				}
			}
		}
	}

	private List<GameObject> getNeighborsOfNode(int x, int y)
	{
		List<GameObject> neighborList = new List<GameObject>();
		int xMinus1 = x-1;
		int yMinus1 = y-1;
		int xPlus1 = x+1;
		int yPlus1 = y+1;

		if(inBounds(xMinus1, y))
			neighborList.Add (componentArray[xMinus1, y]);
		if(inBounds(xPlus1, y))
			neighborList.Add (componentArray[xPlus1, y]);
		if(inBounds(x, yPlus1))
			neighborList.Add(componentArray[x, yPlus1]);
		if(inBounds(x, yMinus1))
			neighborList.Add (componentArray[x,yMinus1]);

		return neighborList;
	}

	private bool inBounds(int x, int y)
	{
		if(x>=0 && x < 7 && y >= 0 && y < 5)
		{
			return true;
		}
		else
			return false;
	}





	private void setupCircuitBoard(int maxWidth, int maxHeight)
	{
		
		componentArray = new GameObject[7, 5];


		int tempTileWidth = (280 / maxWidth);
		int tempTileHeight = tempTileWidth;

		float tempTileScaleY = (float)tempTileHeight / (tileY+8);
		float tempTileScaleX = (float)(tempTileWidth) / (tileX+8);

		//Debug.Log ("TempTileWidth: " + tempTileWidth + ", TempTileHeight: " + tempTileHeight);

		int xOffset = (int)Mathf.Ceil(5/2.0f)-1;
		int yOffset = (int)Mathf.Ceil(7/2.0f)-1;

		//Debug.Log ("xOffset: " + xOffset + ", yOffset: " + yOffset);
		int baseXPosition = 490-(xOffset*tempTileWidth);
		if(maxWidth % 2 == 0)
			baseXPosition-=(tempTileWidth/2);

		int xPosition = baseXPosition;

		int yPosition = yOffset*tempTileHeight;

		//Debug.Log ("xOffset: " + xOffset + ", yOffset: " + yOffset);

						
		for(int i = 0; i < shipMap.GetLength(0); ++i)
		{
			xPosition = baseXPosition;
			
			bool containsVal = false;
			for(int j = 0; j < shipMap.GetLength(1); ++j)
			{
				int val = shipMap[i,j];
				switch(val)
				{
				case 1:
				{
					componentArray[i,j] = (GameObject)Instantiate(circuitPassthroughPrefab, 
					                                              new Vector3(xPosition, yPosition, 0), Quaternion.identity);
					break;
				}
				case 2:
				{
					componentArray[i,j] = (GameObject)Instantiate(circuitPowerSupplyPrefab, 
					                                              new Vector3(xPosition, yPosition, 0), Quaternion.identity);
					
					break;
				}
				case 3:
				{
					componentArray[i,j] = (GameObject)Instantiate(circuitThrusterPrefab, 
					                                              new Vector3(xPosition, yPosition, 0), Quaternion.identity);
					
					break;
				}
				case 4:
				{
					componentArray[i,j] = (GameObject)Instantiate(circuitGunPrefab, 
					                                              new Vector3(xPosition, yPosition, 0), Quaternion.identity);
					
					break;
				}
				default:
				{
					break;
				}
				}
				if(val!=0)
					containsVal = true;
				//if(val!=0)
				xPosition += tempTileWidth;
				if(val!=0)
				{
					componentArray[i,j].transform.localScale = new Vector3(tempTileScaleX, tempTileScaleX, 1);
					ShipComponent compScript = componentArray[i,j].GetComponent("ShipComponent") as ShipComponent;
					compScript.index_Row = i;
					compScript.index_Column = j;
				}
			}
			//if(containsVal)
			yPosition -= tempTileHeight;
		}
	}
}
