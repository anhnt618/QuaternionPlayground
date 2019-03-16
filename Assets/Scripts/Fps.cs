using UnityEngine;
using System.Collections;

public class Fps : MonoBehaviour {

	double updateInterval = 0.5;
	
	private double accum = 0.0; 	// FPS accumulated over the interval
	private double frames = 0; 	// Frames drawn over the interval
	private double timeleft;	 // Left time for current interval
	private string fps = "";
	private Rect rect = new Rect(300, 10 ,150,20);
	void OnGUI(){

		GUI.Box(rect, fps + " FPS");
	}

	void Start()
	{
		timeleft = updateInterval;  
		Application.targetFrameRate = 300;
	}
	void Update(){ 
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0 )
		{

			fps = (accum/frames).ToString("f0");
			timeleft = updateInterval;
			accum = 0.0;
			frames = 0;
		}
	}

}
