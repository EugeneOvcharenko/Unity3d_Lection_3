//  --------------------------------------------------------------------------
//
//  CustomTimer.cs
//
//  iLogos internal library
//
//  Created by Eugene Ovcharenko on July 2013
//  email: over.contact@gmail.com
//
//  Copyright (c) 2013 iLogos, Ukraine. All rights reserved.
//
//  --------------------------------------------------------------------------


using UnityEngine;
using System.Collections;
using System;
//using System.Collections.Generic;

/// <summary>
/// Mono class template.
/// </summary>
public class CustomTimer : MonoBehaviour {}

[System.Serializable]
/// <summary>
/// CTimer class for easy timing and events firing.
/// </summary>
public class CTimer
{
	// DONE: add delegate callbacks if necessary
	public	bool	enabled;
	private	float	curTime;
	private	float	secondsTime;
	private	float	secondsCount;
	public	float	elapsedTime		{ get{ return curTime; } }
	public	float	elapsedSeconds	{ get{ return secondsCount; } }
	public	float	time;
	public	float	w;
	public	bool	loop	= false;
	public	bool	reverse	= false;
	
	private	Action			callback;//по завершении
	private	Action			eachSecCallback;//каждую сек
	private	Action<CTimer>	eachSecCallbackTimer;
	
	public CTimer()
	{
		enabled	= false;
	}
	
	public CTimer( Action set_callback )
	{
		callback = set_callback;
		enabled	= false;
	}

	public void AddEachSecondCallback( Action set_callback )
	{
		eachSecCallback = set_callback;
	}
	
	public void AddEachSecondCallback( Action<CTimer> set_callback )
	{
		eachSecCallbackTimer = set_callback;
	}
	
	/// <summary>
	/// Start the timer with specified time.
	/// </summary>
	public void Start( float set_time )
	{
		reverse	= false;
 		time	= set_time;
		enabled = time > 0;
		curTime = 0.0f;
		secondsTime = 0.0f;
		secondsCount = 0;
		eachSecCallback = null;
		eachSecCallbackTimer = null;
	}

	/// <summary>
	/// Start the timer with specified time. Computes reversed W value.
	/// </summary>
	public void StartReverse( float set_time )
	{
		Start( set_time );
		reverse = true;
	}
	
	/// <summary>
	/// Stop this timer. No callback is called.
	/// </summary>
	public void Stop()
	{
		enabled = false;
	}
	
	/// <summary>
	/// Stop this timer. Assigned callback is called.
	/// Warning: if you stop timer with callback, and then resume it, callback may be called twice.
	/// </summary>
	public void StopWithCallback()
	{
		enabled = false;
		
		if ( callback != null )
		{
			callback();
		}
	}
	
	/// <summary>
	/// Updates the timer with specified dt.
	/// </summary>
	/// <returns>
	/// True if time has finished timing, false otherwise.
	/// </returns>
	public bool Update( float dt )
	{
		if ( enabled )
		{			
			curTime += dt;
			w = curTime / time;
			
			if ( reverse ) w = 1.0f - w;
			if ( curTime >= time )
			{
				// we need to update w value before each sec calbacks
				w = 1.0f;
				if ( reverse ) w = 0.0f;
			}
			
			secondsTime += dt;
			if ( secondsTime > 1.0f )
			{
				secondsCount++;
				// calls callback each second
				secondsTime -= 1.0f;
				
				if ( eachSecCallback != null )		eachSecCallback();
				if ( eachSecCallbackTimer != null )	eachSecCallbackTimer( this );
			}
			
			if ( curTime >= time )
			{				
				curTime = 0.0f;
				enabled = loop;
				
				if ( callback != null )
				{
					callback();
				}
				return true;
			}
		}
		
		return false;
	}
}