using UnityEngine;
using System.Collections;

public class AdvanceParticleRenderer : MonoBehaviour
{
	private	ParticleSystem					particlesSystem;
	private	ParticleSystem.Particle[]		particles;
	public	int								frameCount;
	public	float							maxLife = 100f;
	public	int								particlesEmitCount = 10;
	private	float							particleDeltaLife;
	private	int								currentParticleIndex = 0;
	
	private	CTimer							animDelay;
	
	public void Awake()
	{
		base.Awake();
		gameObject.SetActive( true );
		
		particlesSystem = GetComponent<ParticleSystem>();
		particles = new ParticleSystem.Particle[ 100 ];
		
		animDelay = new CTimer( delay, false );
	}
	
	void Start () 
	{
		particlesSystem.startLifetime = maxLife;
		particlesSystem.Emit( particlesEmitCount );
		particlesSystem.GetParticles( particles );
		particleDeltaLife = maxLife / frameCount;
		
		animDelay.Start( delay );
	}

	#if UNITY_EDITOR
	public void Update()
	{
		#if UNITY_EDITOR
		if ( autoCreateStates )
		{
			autoCreateStates = false;
			//CreateStates( simpleHolder );
		}
		#endif
		
		animDelay.Update( Find.dt );
	}
	
	public void FixedUpdate () 
	{
		
	}
	#endif
	
	public void SetParticlesCount( int count )
	{
		if( particlesEmitCount < count )
		{
			particlesEmitCount = count - particlesEmitCount + 5;
			particlesSystem.Emit( particlesEmitCount );
			particlesEmitCount = particlesSystem.particleCount;
		}
	}
	
	public void GetParticlesArray()
	{
		particlesSystem.GetParticles( particles );
		currentParticleIndex = 0;
	}
	
	public void SetNextParticle( Vector3 position, int animStateIndex, ref int currentAnimIndex )
	{
		particles[ currentParticleIndex ].position = position;
		currentAnimIndex++;
		if( currentAnimIndex > states[ animStateIndex ].end )
		{
			currentAnimIndex = states[ animStateIndex ].start;
		}
		particles[ currentParticleIndex ].lifetime = maxLife - ( currentAnimIndex * particleDeltaLife );
		
		animDelay.Start( delay );
		
		currentParticleIndex++;
	}
	
	public void SetParticlesArray()
	{
		particlesSystem.SetParticles( particles, currentParticleIndex );
	}
}
