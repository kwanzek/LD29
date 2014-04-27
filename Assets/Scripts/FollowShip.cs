using UnityEngine;
using System.Collections;

//This script is essentially copied from the Unity2D tutorial
public class FollowShip : MonoBehaviour {
	
	
	private float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	private float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	private float xSmooth = 1f;		// How smoothly the camera catches up with it's target movement in the x axis.
	//private float ySmooth = 1f;		// How smoothly the camera catches up with it's target movement in the y axis.
	public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
	public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.
	
	public Transform player;		// Reference to the player's transform.
	public CircuitBoard circuitBoardScript;
	
	void Awake () {
	}

	bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}
	
	
	bool CheckYMargin()
	{
		Debug.Log (transform.position.y + " , " + player.position.y);
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
	}
	
	void FixedUpdate ()
	{
		if(player!=null)
		{
			//TrackPlayer();
		}
	}

	public void moveSelf(float x, float y)
	{
		this.transform.position = new Vector3(this.transform.position.x+x, this.transform.position.y+y, this.transform.position.z);
	}
	
	void TrackPlayer ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		float differenceX = targetX;
		float differenceY = targetY;


		//Debug.Log ("targetX: " +targetX + ", targetY: " + targetY + ", shipLoc: " + player.transform.position);
		// If the player has moved beyond the x margin...
		if(CheckXMargin())
		{
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
		}

		if(CheckYMargin())
		{
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetY = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
		}

		differenceX -= targetX;
		differenceY -= targetY;

		//circuitBoardScript.translateAll(differenceX, differenceY);

		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		//targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		//targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);
		

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
