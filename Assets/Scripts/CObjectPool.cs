using UnityEngine;
using System.Collections;

[System.Serializable]
public class CObjectPool
{
	[SerializeField]
	public	GameObject				prefab;
	private	GameObject[]			objects;
	private	FrameAmination[]		animations;
	private	RandomizePosition[]		randomizePos;

	private	int						count			= 5000;
	private	int						current_index	= 0;
	public	bool					forceY;
	public	float					forveYMin		= -128.0f;
	public	float					forveYMax		= 128.0f;
	
	public void Init( GameObject parent, int set_max_count )
	{
		count = set_max_count;
		Init( parent );
	}
	
	public void Init( GameObject parent )
	{
		int i;
		if ( prefab != null )
		{
			objects = new GameObject[ count ];
			animations = new FrameAmination[ count ];
			randomizePos = new RandomizePosition[ count ];
			for ( i = 0; i < count; i++ )
			{
				objects[ i ] = GameObject.Instantiate( prefab ) as GameObject;
				objects[ i ].transform.parent = parent.transform;
				objects[ i ].transform.localPosition = Vector3.zero;
				
				animations[ i ] = objects[ i ].GetComponent<FrameAmination>();
				randomizePos[ i ] = objects[ i ].GetComponent<RandomizePosition>();

				if ( animations[ i ] )
				{
					animations[ i ].SetupObjectPool( this, i );
				}				
				objects[ i ].SetActive( false );
			}
		}
	}
	
	public void SwitchOffAnimation( int num )
	{
		objects[ num ].SetActive( false );
	}

	public GameObject Show( bool badMovement, bool optimizedMovement, bool fastMovement )
	{
		if ( forceY )
		{
			Debug.Log( Mathf.Lerp( forveYMin, forveYMax, Random.value ) );
		}
		
		objects[ current_index ].SetActive( true );
		
		if ( animations[ current_index ] )
		{
			animations[ current_index ].Play();
		}

		if ( randomizePos[ current_index ] != null )
		{
			randomizePos[ current_index ].Randomize();

			randomizePos[ current_index ].badMovement = badMovement;
			randomizePos[ current_index ].optimizedMovement = optimizedMovement;
			randomizePos[ current_index ].fastMovement = fastMovement;
		}
		
		int return_number = current_index;
		
		current_index++;
		if ( current_index >= count )
		{
			current_index = 0;
		}
		
		if ( objects.Length > 0 && objects[ return_number ] != null )
		{
			return objects[ return_number ];
		}
		else
		{
			Debug.Log( prefab.name + "number: " + return_number );
			return null;
		}
	}
}