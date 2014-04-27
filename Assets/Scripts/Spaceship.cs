using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spaceship : MonoBehaviour {

	private int _speed;
	private int _rotation_speed;
	private int _maxSpeed;


	public enum ComponentEnum {Empty=0, Passthrough=1, PowerSupply=2, Thruster=3, Gun=4};

	public GameObject[,] componentArray;

	public GameObject passthroughPrefab;
	public GameObject powersupplyPrefab;
	public GameObject thrusterPrefab;
	public GameObject gunPrefab;

	public FollowShip followship;

	private int tileX = 64;
	private int tileY = 64;

	int[,] shipMap = new int[,]
	{
		{0,0,0,0,0},
		{0,0,4,0,0},
		{0,0,1,0,0},
		{0,0,1,0,0},
		{0,3,2,3,0},
		{0,0,1,0,0},
		{0,0,0,0,0},
	};

	public int maxWidth;
	public int maxHeight;


	private CircuitBoard circuitBoardScript;

	private int calculateMaxWidth()
	{
		int count =0;
		int maxCount = 0;
		for(int i = 0; i < shipMap.GetLength(0); ++i)
		{
			count = 0;
			for(int j = 0; j < shipMap.GetLength(1); ++j)
			{
				if(shipMap[i,j] != 0)
				{
					count++;
				}
			}
			if(count > maxCount)
				maxCount = count;
		}
		return maxCount;
	}

	private int calculateMaxHeight()
	{
		int count =0;
		int maxCount = 0;

		for(int i = 0; i < shipMap.GetLength(1); ++i)
		{
			count = 0;
			for(int j = 0; j < shipMap.GetLength(0); ++j)
			{
				if(shipMap[j,i] != 0)
				{
					count++;
				}
			}
			if(count > maxCount)
				maxCount = count;
		}
		return maxCount;
	}

	// Use this for initialization
	void Start () {
		_speed = 50;
		_rotation_speed = 90;
		_maxSpeed = 100;

		maxWidth = calculateMaxWidth();
		maxHeight = calculateMaxHeight();

		//Debug.Log ("ShipComponentLength0: "+ shipMap.GetLength(0) + ", shipcomponentLength1: "+ shipMap.GetLength(1));


		//Debug.Log ("MaxWidth: " + maxWidth + ", maxHeight: " + maxHeight);
	
		componentArray = new GameObject[7, 5];

		int xPosition = -300;
		int yPosition = 200;

		circuitBoardScript = GetComponent("CircuitBoard") as CircuitBoard;


		for(int i = 0; i < shipMap.GetLength(0); ++i)
		{
			xPosition = -300;
			for(int j = 0; j < shipMap.GetLength(1); ++j)
			{

				int val = shipMap[i,j];
				switch(val)
				{
				case 1:
				{
					componentArray[i,j] = (GameObject)Instantiate(passthroughPrefab, 
					                                              new Vector3(xPosition, yPosition, 0), Quaternion.identity);
					break;
				}
				case 2:
				{
					componentArray[i,j] = (GameObject)Instantiate(powersupplyPrefab, 
					                                              new Vector3(xPosition, yPosition, 0), Quaternion.identity);
					
					break;
				}
				case 3:
				{
					componentArray[i,j] = (GameObject)Instantiate(thrusterPrefab, 
					                                              new Vector3(xPosition, yPosition, 0), Quaternion.identity);
					
					break;
				}
				case 4:
				{
					componentArray[i,j] = (GameObject)Instantiate(gunPrefab, 
					                                              new Vector3(xPosition, yPosition, 0), Quaternion.identity);
					
					break;
				}
				default:
				{
					break;
				}
				}
				xPosition += tileX;
			}
			yPosition -= tileY;
		}

		circuitBoardScript.setGameObjectArray(ref shipMap, ref maxWidth, ref maxHeight);



		//this.transform.position = new Vector2(1.736f, -125.52f);
		followship = Camera.main.GetComponent("FollowShip") as FollowShip;
		followship.player = this.transform;
		followship.circuitBoardScript = circuitBoardScript;
		followship.enabled = true;
		//FollowShip temp = Camera.main.GetComponent("FollowShip") as FollowShip;
		//temp.player = this.transform;
		//temp.circuitBoardScript = circuitBoardScript;
		//temp.enabled = true;

	}
	
	// Update is called once per frame
	void Update () {

		GameObject[,] circuitArray = circuitBoardScript.componentArray;

		int numThrusters = 0;

		foreach(GameObject obj in circuitArray)
		{
			if(obj != null)
			{
				if(obj.name.Contains("powersupply"))
				{

				}
				else
				{
					ShipComponent compScript = obj.GetComponent("ShipComponent") as ShipComponent;
					if(compScript._powerLevel > 0)
					{
						if(obj.name.Contains("thruster"))
						{
							//Debug.Log ("MOVEFORWARD");
							numThrusters+=1;
						}
						else if(obj.name.Contains("gun"))
						{
							//Debug.Log ("SHOOT");
						}

					}
				}

			}
		}
		if(numThrusters > 0)
		{
			foreach(GameObject obj in componentArray)
			{
				if(obj != null)
				{
					obj.transform.position = new Vector2(obj.transform.position.x,
					                                      obj.transform.position.y +(_speed*numThrusters*Time.deltaTime));
				}
			}
			this.transform.position = new Vector2(this.transform.position.x,
			                                      this.transform.position.y +(_speed*numThrusters*Time.deltaTime));

			followship.moveSelf(0, _speed*numThrusters*Time.deltaTime);
			circuitBoardScript.translateAll(0, _speed*numThrusters*Time.deltaTime);

			//Debug.Log (this.transform.position);
		}

		//Keep the ship on the screen for now

		//this.transform.position = new Vector2(this.transform.position.x + (_speed)*Time.deltaTime,
		//                                      this.transform.position.y + (_speed)*Time.deltaTime*0);

		//this.transform.Rotate(Vector3.forward * _rotation_speed*Time.deltaTime);


		/*
		float widthRel = (48*3) / (Screen.width); //relative width
		float heightRel= (32) /(Screen.height); //relative height
		
		Vector3 viewPos = Camera.main.WorldToViewportPoint (this.transform.position);
		viewPos.x = Mathf.Clamp(viewPos.x, widthRel, 1-widthRel);
		viewPos.y = Mathf.Clamp(viewPos.y, heightRel, 1-heightRel);
		this.transform.position = Camera.main.ViewportToWorldPoint (viewPos);

		*/


	}
}
