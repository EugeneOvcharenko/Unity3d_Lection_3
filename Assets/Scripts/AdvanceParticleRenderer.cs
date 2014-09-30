using UnityEngine;
using System.Collections;

[System.Serializable]
public class CAnimState
{
	public	string	name	= "default";
	public	int		start	= 0;
	public	int		end		= 0;
	public	int		index	= 0;
	public	Vector3	pos		= Vector3.zero;
	public	float	time	= 0.025f;
	public	CTimer	timer;

	public void Init()
	{
		timer = new CTimer();
		timer.Start( time );
		timer.loop = true;
		index = start;
	}

	public void Update( float dt )
	{
		if ( timer.Update( dt ) )
		{
			index = ++index >= end ? start : index;
		}
	}
}

public class AdvanceParticleRenderer : MonoBehaviour
{
	private	ParticleSystem					particlesSystem;
	private	ParticleSystem.Particle[]		particles;
	public	int								frameCount;
	public	float							maxLife = 100f;
	public	int								particlesEmitCount = 10;
	private	float							particleDeltaLife;
	private	int								currentParticleIndex = 0;

	public	int								showState = 0;
	public	CAnimState[]					states;

	public void Awake()
	{
		gameObject.SetActive( true );
		
		particlesSystem = GetComponent<ParticleSystem>();
		particles = new ParticleSystem.Particle[ 100 ];
	}
	
	void Start () 
	{
		particlesSystem.startLifetime = maxLife;
		particlesSystem.Emit( particlesEmitCount );
		particlesSystem.GetParticles( particles );
		particleDeltaLife = maxLife / frameCount;

		SetParticlesCount( states.Length );
		for ( int i = 0; i < states.Length; i++ )
		{
			CAnimState s = states[ i ];
			s.Init();
		}
	}
	
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
	
	public void SetNextParticle( Vector3 position, int currentAnimIndex )
	{
		particles[ currentParticleIndex ].position = position;
		particles[ currentParticleIndex ].lifetime = maxLife - ( currentAnimIndex * particleDeltaLife );
		
		currentParticleIndex++;
	}
	
	public void SetParticlesArray()
	{
		particlesSystem.SetParticles( particles, currentParticleIndex );
	}

	void FixedUpdate()
	{
		GetParticlesArray();

		for ( int i = 0; i < states.Length; i++ )
		{
			CAnimState s = states[ i ];
			s.Update( Time.fixedDeltaTime );
			SetNextParticle( s.pos, s.index );
		}

		SetParticlesArray();
	}
}
