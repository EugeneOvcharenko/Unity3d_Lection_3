using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameRadar : MonoBehaviour 
{
	private	AdvanceParticleRenderer particleRenderer;
	
	public	List<RadarObject> objects;
	
	void Awake()
	{
		particleRenderer = GetComponent<AdvanceParticleRenderer>();
		objects = new List<RadarObject>();
	}
	
	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}
	
	void FixedUpdate()
	{
		UpdateRadar();
	}
		
	void UpdateRadar()
	{
		particleRenderer.GetParticlesArray();
		for( int i = 0; i < objects.Count; ++i )
		{
			if( objects[i].currentObject.activeSelf == true )
			{
				if( ( objects[i].objectLifeHolder != null ) && ( objects[i].objectLifeHolder.life > 0 ) )
				{
					if( Mathf.Abs( objects[i].objectTransform.position.x ) < 2300 )
					{
						Vector3 tPos = transform.position;
						tPos.x = (300f / 2300f) * objects[i].objectTransform.position.x;
						tPos.y = -2300f;
						particleRenderer.SetNextParticle( tPos, objects[i].animStateIndex, ref objects[i].currentAnimIndex );
					}
				}
			}
		}
		particleRenderer.SetParticlesArray();
	}
	
	public void RegisterNewObject( GameObject obj )
	{
		if( obj == null )
		{
			return;
		}
		
		RadarObject robj = new RadarObject();
		
		if( obj.GetComponent<AIBoat>() != null )
		{
			robj.objectType = RadarObjectsTypes.boat;
			robj.animStateIndex = particleRenderer.GetStateNum( "circle" );
		}
		else if( obj.GetComponent<AIhelic>() != null )
		{
			robj.objectType = RadarObjectsTypes.helicopter;
			robj.animStateIndex = particleRenderer.GetStateNum( "trianle" );
		}
		else if( obj.GetComponent<TankAI>() != null )
		{
			robj.objectType = RadarObjectsTypes.tank;
			robj.animStateIndex = particleRenderer.GetStateNum( "square" );
		}
		else if( obj.GetComponent<AIdot>() != null )
		{
			robj.objectType = RadarObjectsTypes.dot;
			robj.animStateIndex = particleRenderer.GetStateNum( "health" );
		}
		else if( obj.GetComponent<AIMagnetMine>() != null )
		{
			robj.objectType = RadarObjectsTypes.magnetMine;
			robj.animStateIndex = particleRenderer.GetStateNum( "mine" );
		}
		else
		{
			return;
		}
		robj.objectName			= robj.objectType.ToString();
		robj.currentAnimIndex	= particleRenderer.states[ robj.animStateIndex ].start;
		
		robj.objectTransform	= obj.transform;
		robj.currentObject		= obj;
		LifeHolder lh = obj.GetComponent<LifeHolder>();
		if( lh != null )
		{
			robj.objectLifeHolder = lh;
		}

		objects.Add( robj );
		if( particleRenderer.particlesEmitCount < objects.Count )
		{
			particleRenderer.SetParticlesCount( objects.Count );
		}
	}
}

public	enum RadarObjectsTypes { none, helicopter, tank, dot, boat, magnetMine }

[System.Serializable]
public class RadarObject
{
	[HideInInspector]
	public	string				objectName;
	public	GameObject			currentObject;
	public	Transform			objectTransform;
	public	RadarObjectsTypes	objectType;
	public	int					animStateIndex;
	//[HideInInspector]
	public	int					currentAnimIndex;
}
