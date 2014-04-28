using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spaceship : MonoBehaviour {

	private int _speed;
	private int _rotation_speed;
	private int _maxSpeed;

	public AudioClip missileShot;

	public enum ComponentEnum {Empty=0, Passthrough=1, PowerSupply=2, Thruster=3, Gun=4};

	public GameObject[,] componentArray;

	public GameObject passthroughPrefab;
	public GameObject powersupplyPrefab;
	public GameObject thrusterPrefab;
	public GameObject gunPrefab;
	public GameObject missilePrefab;


	private FollowShip followship;

	private int tileX = 32;
	private int tileY = 32;

	public Vector2 centerofMass;
	private int totalMass;

	private Vector2 shipVelocity;
	private float acceleration = 2.0f;
	private float shipAngle;
	private float shipAngularVelocity = 15.0f;

	private float coeffKineticFriction = 0.995f;

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
		//_speed = 50;
		//_rotation_speed = 90;
		_maxSpeed = 3;

		maxWidth = calculateMaxWidth();
		maxHeight = calculateMaxHeight();


		shipVelocity = new Vector2(0,0);
		shipAngle = 0;
		//Debug.Log ("ShipComponentLength0: "+ shipMap.GetLength(0) + ", shipcomponentLength1: "+ shipMap.GetLength(1));


		//Debug.Log ("MaxWidth: " + maxWidth + ", maxHeight: " + maxHeight);
	
		componentArray = new GameObject[7, 5];

		int xPosition = -100;
		int yPosition = 100;

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

		centerofMass = computeCenterOfMass();


	}



	Vector2 computeCenterOfMass()
	{
		int massTotal = 0;
		Vector2 weightedMass = new Vector2(0,0);

		foreach(GameObject obj in componentArray)
		{
			if(obj != null)
			{
				ShipComponent shipComponent = obj.GetComponent("ShipComponent") as ShipComponent;
				massTotal += shipComponent._mass;
				weightedMass.x+=(obj.transform.position.x * shipComponent._mass);
				weightedMass.y+=(obj.transform.position.y * shipComponent._mass);
			}
		}

		weightedMass.x /= massTotal;
		weightedMass.y /= massTotal;

		totalMass = massTotal;

		//Debug.Log ("CenterofMass: " + weightedMass);

		return weightedMass;

	}

	Vector2 sumOfForces()
	{
		Vector2 forceSum = new Vector2(0,0);
		foreach(GameObject obj in circuitBoardScript.componentArray)
		{
			if(obj != null)
			{
				if(obj.name.Contains("thruster"))
				{



					Thruster circuitComponent = obj.GetComponent("Thruster") as Thruster;

					GameObject shipObj = componentArray[circuitComponent.index_Row, circuitComponent.index_Column];



					if(circuitComponent._powerLevel > 0)
					{
						float thrusterAngle = shipObj.transform.localEulerAngles.z-shipAngle;
						while(thrusterAngle < 0)
						{
							thrusterAngle+=360;
						}
						while(thrusterAngle > 360)
							thrusterAngle-= 360; 

						thrusterAngle=thrusterAngle*Mathf.Deg2Rad + Mathf.PI/2;
					
						Vector2 forceVector = new Vector2(circuitComponent.speed * Mathf.Cos(thrusterAngle), 
						                                  circuitComponent.speed * Mathf.Sin(thrusterAngle));
						forceSum.x += forceVector.x;
						forceSum.y += forceVector.y;

						GameObject mirrorVal = componentArray[circuitComponent.index_Row, circuitComponent.index_Column];

						BubbleScript bScript =  mirrorVal.GetComponent("BubbleScript") as BubbleScript;
						bScript.enabled = true;

						//massTotal += shipComponent._mass;
						//weightedMass.x+=(obj.transform.position.x * shipComponent._mass);
						//weightedMass.y+=(obj.transform.position.y * shipComponent._mass);
					}
					else
					{
						GameObject mirrorVal = componentArray[circuitComponent.index_Row, circuitComponent.index_Column];
						
						BubbleScript bScript =  mirrorVal.GetComponent("BubbleScript") as BubbleScript;
						bScript.enabled = false;
					}
				}
			}
		}

		//Friction

		Vector2 frictionForce = new Vector2(-1*coeffKineticFriction*shipVelocity.x, -1*coeffKineticFriction*shipVelocity.y);

		forceSum.x+=frictionForce.x;
		forceSum.y+=frictionForce.y;

		forceSum.x /= totalMass;
		forceSum.y /= totalMass;

		//Debug.Log (forceSum);

		return forceSum;
	}



	Vector2 rotateVector(Vector2 vector, float theta)
	{

		theta*=Mathf.Deg2Rad;
		vector.x = Mathf.Cos (theta)* vector.x-Mathf.Sin(theta) * vector.y;
		vector.y = Mathf.Sin (theta)*vector.x +Mathf.Cos (theta)*vector.y;

		return vector;

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


		//Movement


		sumOfForces();


		GameObject thruster1 = null;
		GameObject thruster2 = null;

		bool thruster1Active = false;
		bool thruster2Active = false;

		foreach(GameObject obj in circuitBoardScript.componentArray)
		{
			if(obj != null)
			{
				if(obj.name.Contains("thruster"))
				{
					if(thruster1 == null)
						thruster1 = obj;
					else if(thruster2 == null)
						thruster2 = obj;
					Thruster thrusterScript = obj.GetComponent("Thruster") as Thruster;
					if(thrusterScript._powerLevel > 0)
					{
						if(obj == thruster1)
							thruster1Active = true;
						else if(obj == thruster2)
							thruster2Active = true;




						
					}
				}
			}
		}

		centerofMass = computeCenterOfMass();

		if(thruster1Active && !thruster2Active)
		{
			//rotate right
			shipAngle -= shipAngularVelocity * Time.deltaTime;
		}
		else if(!thruster1Active && thruster2Active)
		{
			shipAngle += shipAngularVelocity * Time.deltaTime;
			//rotate left
		}

		foreach(GameObject compObj in componentArray)
		{
			if(compObj != null)
			{
				//Vector2 differenceVector = new Vector2( compObj.transform.position.x - centerofMass.x,
				//                                       compObj.transform.position.y - centerofMass.y);

				//Vector2 newVector = rotateVector(differenceVector, shipAngle);

				//float angleDiff = Mathf.Atan2(differenceVector.y, differenceVector.x);

				Vector2 blockVector = new Vector2( compObj.transform.position.x - centerofMass.x,
				                                    compObj.transform.position.y - centerofMass.y);


				//Debug.Log ("Block Vector: " + blockVector);

				float angleDifference = shipAngle-  compObj.transform.localEulerAngles.z;

				float tempAngle = (angleDifference * Mathf.Deg2Rad);

				compObj.transform.localEulerAngles = new Vector3(0,0, shipAngle);


				float rotatedX = (Mathf.Cos (tempAngle)*(blockVector.x)) -
					(Mathf.Sin (tempAngle) * (blockVector.y)) + centerofMass.x;

				float rotatedY = (Mathf.Sin (tempAngle)*(blockVector.x)) +
					(Mathf.Cos (tempAngle) * (blockVector.y)) + centerofMass.y;




				compObj.transform.position = new Vector2(rotatedX, rotatedY);

			}
		}


		if(thruster1Active && thruster2Active)
		{
			float tempAngle = (Mathf.Deg2Rad * shipAngle)+Mathf.PI/2;
			if(shipVelocity.magnitude < _maxSpeed)
			{
				shipVelocity.x = shipVelocity.x + Mathf.Cos (tempAngle)*acceleration*Time.deltaTime;
				shipVelocity.y = shipVelocity.y + Mathf.Sin (tempAngle)*acceleration*Time.deltaTime;
			}


		}



		shipVelocity.x *= coeffKineticFriction;
		shipVelocity.y *= coeffKineticFriction;




		//Check collisions

		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");




		//Bounds boundingBox = renderer.bounds;
		//Vector3 extents = boundingBox.extents;
		//Rect boundingRect = new Rect(boundingBox.center.x-extents.x, boundingBox.center.y-extents.y,
		 //                            extents.x*1.8f, extents.y*1.8f);

		//Debug.Log (enemies[0].name);

		foreach(GameObject enemy in enemies)
		{
			Bounds tileBoundingBox = enemy.renderer.bounds;
			Vector3 tileExtents = tileBoundingBox.extents;

			Rect tileBoundingRect = new Rect(tileBoundingBox.center.x-tileExtents.x,
			                                 tileBoundingBox.center.y-tileExtents.y, tileExtents.x*1.15f, tileExtents.y*1.35f);


			foreach(GameObject block in componentArray)
			{
				if (block != null)
				{

					Bounds blockBoundingBox = block.renderer.bounds;
					Vector3 blockExtents = blockBoundingBox.extents;
					
					Rect blockBoundingRect = new Rect(blockBoundingBox.center.x-blockExtents.x,
					                                  blockBoundingBox.center.y-blockExtents.y, 
					                                  blockExtents.x*1.6f, blockExtents.y*1.6f);

					bool isIntersecting = doesIntersect(blockBoundingRect,tileBoundingRect);

					if(isIntersecting)
					{
						Debug.Log ("COLLISION");
						shipVelocity.x *= .95f;
						shipVelocity.y *= .95f;
						EnemyBehavior enBehScript = enemy.GetComponent("EnemyBehavior") as EnemyBehavior;

						enBehScript.currentSpeed *= 0.9995f;
					}

				}
			}




		}






		foreach(GameObject compObj in componentArray)
		{
			if(compObj != null)
			{
				compObj.transform.position = new Vector2(compObj.transform.position.x+shipVelocity.x, 
				                                         compObj.transform.position.y+shipVelocity.y);
			}
		}
		
		Vector2 newLoc = new Vector2(shipVelocity.x, 
		                             shipVelocity.y);
		
		followship.moveSelf(newLoc.x, newLoc.y);
		circuitBoardScript.translateAll(newLoc.x, newLoc.y);

		GameObject console = GameObject.FindGameObjectWithTag("Console");

		console.transform.Translate(new Vector3(newLoc.x, newLoc.y, 0));

		this.transform.position = new Vector3(centerofMass.x, centerofMass.y, 0);
		//Debug.Log (shipVelocity);


		//shipVelocity.x -= (coeffKineticFriction * acceleration);
		//shipVelocity.y -= (coeffKineticFriction * acceleration);

		this.transform.localEulerAngles = new Vector3(0,0,shipAngle);



		//Debug.Log (this.transform.position);
		
		
		













		//Firing
		foreach(GameObject obj in circuitArray)
		{
			if(obj!=null)
			{
				if(obj.name.Contains("gun"))
				{
					Gun gunScript = obj.GetComponent("Gun") as Gun;
					if(gunScript._powerLevel > 0)
					{
				
						if(gunScript.interval <= 0.0)
						{

							GameObject mirror = componentArray[gunScript.index_Row, gunScript.index_Column];

							GameObject missile = (GameObject)Instantiate(missilePrefab, 
							                                             new Vector3(mirror.transform.position.x, 
							            									mirror.transform.position.y,
							            											0),
							                                             mirror.transform.rotation);

							MissileScript missileScript = missile.GetComponent("MissileScript") as MissileScript;
							missileScript.angleVector = missile.transform.localEulerAngles;
							
							gunScript.interval = gunScript.intervalBase;


							AudioSource.PlayClipAtPoint(missileShot, Camera.main.transform.position);
						}
						
						gunScript.interval -= Time.deltaTime;

					}
				}
			}
		}


	}



			
			bool doesIntersect(Rect player, Rect obj)
			{
				Vector2 playerTopLeft = new Vector2(player.center.x-player.width/2, player.center.y+player.height/2);
				Vector2 playerBottomRight = new Vector2(player.center.x+player.width/2, player.center.y-player.height/2);
				
				Vector2 objectTopLeft = new Vector2(obj.center.x-obj.width/2, obj.center.y+obj.height/2);
				Vector2 objectBottomRight = new Vector2(obj.center.x+obj.width/2, obj.center.y-obj.height/2);
				
				
				
				return (playerTopLeft.x < objectBottomRight.x && playerTopLeft.y > objectBottomRight.y) &&
					(playerBottomRight.x > objectTopLeft.x && playerBottomRight.y < objectTopLeft.y);
			}












}
