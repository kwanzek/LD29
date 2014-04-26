using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour {


	public State _menuState = State.MainMenu;
	public enum State {MainMenu, Credits}


	private List<GameObject> mainMenuObjectList = new List<GameObject>();
	private List<GameObject> creditsMenuObjectList = new List<GameObject>();

	// Use this for initialization
	void Start () {
		_menuState = State.MainMenu;
		GameObject[] objectArray = GameObject.FindGameObjectsWithTag("Credits");

		foreach(GameObject obj in objectArray)
		{
			creditsMenuObjectList.Add(obj);
			Material mat = obj.transform.renderer.material;
			mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0);
		}

		objectArray = GameObject.FindGameObjectsWithTag("MainMenu");

		foreach(GameObject obj in objectArray)
		{
			mainMenuObjectList.Add(obj);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(_menuState == State.MainMenu)
		{
			Debug.Log ("MainMenu: " + mainMenuObjectList.Count);


			foreach(GameObject obj in creditsMenuObjectList)
			{
				Material mat = obj.transform.renderer.material;
				mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0);
			}
			foreach(GameObject obj in mainMenuObjectList)
			{
				Material mat = obj.transform.renderer.material;
				mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);
			}

		}
		else if(_menuState == State.Credits)
		{
			Debug.Log ("Credits: " + creditsMenuObjectList.Count);

			foreach(GameObject obj in mainMenuObjectList)
			{
				Material mat = obj.transform.renderer.material;
				mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0);
			}
			foreach(GameObject obj in creditsMenuObjectList)
			{
				Material mat = obj.transform.renderer.material;
				mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);
			}
		}
	}

	void setState(int input)
	{
		_menuState = (State)input;
	}
}
