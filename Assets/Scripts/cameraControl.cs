using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {

	private Vector2 velocity;
	public float smoothTimeY;
	public float smoothTimeX;

	public GameObject player;
	
	// Update is called once per frame
	void Update () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void FixedUpdate(){

		float posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
		float posY = Mathf.SmoothDamp (transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

		transform.position = new Vector3 (posX, posY, transform.position.z);
	}
}