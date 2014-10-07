using UnityEngine;
using System.Collections;

public class RandomizePosition : MonoBehaviour {

	public	bool		badMovement			= false;
	public	bool		optimizedMovement	= false;
	public	bool		fastMovement		= false;

	Transform	t;
	Vector3		lastPos;

	void Start () {
		t = transform;
		Randomize();
	}

	public void Randomize () {
		Vector3 pos = new Vector3();

		pos.x = Random.Range( 0, Camera.main.pixelWidth ) - Camera.main.pixelWidth / 2f;
		pos.y = Random.Range( 0, Camera.main.pixelHeight ) - Camera.main.pixelHeight / 2f;
		transform.localPosition = pos;
		lastPos = pos;
	}	

	public void Update()
	{
		if ( badMovement )
		{
			transform.localPosition = Vector3.Lerp( transform.localPosition, Vector3.zero, Time.deltaTime / 5 );
		}

		if ( optimizedMovement )
		{
			lastPos = Vector3.Lerp( lastPos, Vector3.zero, Time.deltaTime / 5 );
			t.localPosition = lastPos;
		}

		if ( fastMovement )
		{
			lastPos = Vector3.Lerp( lastPos, Vector3.zero, OptimizationManager.dt / 5 );
			t.localPosition = lastPos;
		}
	}
}