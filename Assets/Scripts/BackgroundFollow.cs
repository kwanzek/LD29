using UnityEngine;
using System.Collections;

public class BackgroundFollow : MonoBehaviour {

	public GameObject currentBackground;
	public GameObject otherBackground1;
	public GameObject otherBackground2;
	public GameObject otherBackground3;

	public GameObject ship;

	int width = 2442;
	int height = 2160; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		Spaceship spaceShip = ship.GetComponent("Spaceship") as Spaceship;

		Vector2 curShipLoc = spaceShip.centerofMass;

		if(curShipLoc.x > currentBackground.transform.position.x)
		{

			otherBackground1.transform.position = new Vector2(currentBackground.transform.position.x + width,
			                                                  currentBackground.transform.position.y);
		}
		else
		{
			otherBackground1.transform.position = new Vector2(currentBackground.transform.position.x - width,
			                                                  currentBackground.transform.position.y);
		}

		if(curShipLoc.y > currentBackground.transform.position.y)
		{
			otherBackground2.transform.position = new Vector2(currentBackground.transform.position.x,
			                                                  currentBackground.transform.position.y+height);
		}
		else
		{

			otherBackground2.transform.position = new Vector2(currentBackground.transform.position.x,
			                                                  currentBackground.transform.position.y-height);

		}


		//top right
		if(curShipLoc.x > currentBackground.transform.position.x && curShipLoc.y > currentBackground.transform.position.y)
		{
			otherBackground3.transform.position = new Vector2(currentBackground.transform.position.x+width,
			                                                  currentBackground.transform.position.y+height);


		}


		//top left
		else if(curShipLoc.x < currentBackground.transform.position.x && curShipLoc.y > currentBackground.transform.position.y)
		{
			otherBackground3.transform.position = new Vector2(currentBackground.transform.position.x-width,
			                                                  currentBackground.transform.position.y+height);
		}

		//bottom left
		else if(curShipLoc.x < currentBackground.transform.position.x && curShipLoc.y < currentBackground.transform.position.y)
		{
			otherBackground3.transform.position = new Vector2(currentBackground.transform.position.x-width,
			                                                  currentBackground.transform.position.y-height);
		}
		else
		{
			otherBackground3.transform.position = new Vector2(currentBackground.transform.position.x+width,
			                                                  currentBackground.transform.position.y-height);
		}


		Bounds other1 = otherBackground1.renderer.bounds;
		Vector3 other1Extents = other1.extents;
		
		Rect other1BoundingRect = new Rect(other1.center.x-other1Extents.x,
		                                   other1.center.y-other1Extents.y, other1Extents.x*2f, other1Extents.y*2f);

		if(other1BoundingRect.Contains(new Vector3(spaceShip.centerofMass.x, spaceShip.centerofMass.y, 0)))
		{
			GameObject temp = currentBackground;
			currentBackground = otherBackground1;
			otherBackground1 = temp;

		}




		Bounds other2 = otherBackground2.renderer.bounds;
		Vector3 other2Extents = other2.extents;
		
		Rect other2BoundingRect = new Rect(other2.center.x-other2Extents.x,
		                                   other2.center.y-other2Extents.y, other2Extents.x*2f, other2Extents.y*2f);
		
		if(other2BoundingRect.Contains(new Vector3(spaceShip.centerofMass.x, spaceShip.centerofMass.y, 0)))
		{
			GameObject temp = currentBackground;
			currentBackground = otherBackground2;
			otherBackground2 = temp;
			
		}






		Bounds other3 = otherBackground1.renderer.bounds;
		Vector3 other3Extents = other3.extents;
		
		Rect other3BoundingRect = new Rect(other3.center.x-other3Extents.x,
		                                   other3.center.y-other3Extents.y, other3Extents.x*2f, other3Extents.y*2f);
		
		if(other3BoundingRect.Contains(new Vector3(spaceShip.centerofMass.x, spaceShip.centerofMass.y, 0)))
		{
			GameObject temp = currentBackground;
			currentBackground = otherBackground3;
			otherBackground3 = temp;
			
		}









	}
}
