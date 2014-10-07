using UnityEngine;
using System.Collections;

public class OptimizationManager : MonoBehaviour {

	public static OptimizationManager instance;
	public	CObjectPool	TestAnimation;

	public	int			createCount			= 1000;
	public	bool		createPool			= false;
	public	bool		createInstance		= false;

	public	bool		badMovement			= false;
	public	bool		optimizedMovement	= false;
	public	bool		fastMovement		= false;

	public static float dt;

	void Awake() {
		instance = this;
	}

	void Start () {
		TestAnimation.Init( gameObject );
	}
	
	// Update is called once per frame
	void Update () {
		dt = Time.deltaTime;

		int i;
		if ( createPool )
		{
			createPool = false;

			for ( i = 0; i < createCount; i++ )
			{
				TestAnimation.Show( badMovement, optimizedMovement, fastMovement );
			}
		}

		if ( createInstance )
		{
			createInstance = false;
			
			for ( i = 0; i < createCount; i++ )
			{
				GameObject anim = Instantiate( TestAnimation.prefab ) as GameObject;
				FrameAmination fa = anim.GetComponent<FrameAmination>();
				fa.autoDestroy = true;
				fa.Play();

				RandomizePosition rp = anim.GetComponent<RandomizePosition>();
				rp.badMovement = badMovement;
				rp.optimizedMovement = optimizedMovement;
				rp.fastMovement = fastMovement;
			}
		}
	}
}