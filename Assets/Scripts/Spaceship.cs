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

	private int tileX = 64;
	private int tileY = 64;

	int[,] shipMap = new int[,]
	{
		{0,4,0},
		{0,1,0},
		{0,1,0},
		{3,2,3},
		{0,1,0}
	};

	public int maxWidth;
	public int maxHeight;


	private CircuitBoard circuitBoardScript;

	// Use this for initialization
	void Start () {
		_speed = 150;
		_rotation_speed = 90;
		_maxSpeed = 200;

		maxWidth = shipMap.GetLength(0);
		maxHeight = shipMap.GetLength (1);

		componentArray = new GameObject[maxWidth, maxHeight];

		int xPosition = -200;
		int yPosition = -200;

		circuitBoardScript = GetComponent("CircuitBoard") as CircuitBoard;

		for(int i=shipMap.GetLength(0)-1; i >= 0; --i)
		{
			
			xPosition = -200;
			for(int j=shipMap.GetLength(1)-1; j >= 0; --j)
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
			yPosition += tileY;
		}

		/*
		for(int i = 0; i < componentArray.GetLength (0); ++i)
		{
			for(int j = 0; j < componentArray.GetLength(1); ++j)
			{
				if(componentArray[i,j] != null)
					Debug.Log (componentArray[i,j].name);
			}
		}*/

		circuitBoardScript.setGameObjectArray(ref shipMap);
	}
	
	// Update is called once per frame
	void Update () {



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
