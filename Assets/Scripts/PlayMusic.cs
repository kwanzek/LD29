using UnityEngine;
using System.Collections;

public class PlayMusic : MonoBehaviour {

	public AudioSource ambientMusic;

	// Use this for initialization
	void Start () {
	
		ambientMusic.Play();
		ambientMusic.loop = true;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
