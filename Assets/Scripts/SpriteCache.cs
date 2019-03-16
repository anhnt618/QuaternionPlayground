using UnityEngine;
using System.Collections.Generic;

public struct KeyFrame
{
	public Sprite sprite;
	public IList<System.Action> events;
}

public class SpriteAnimation {

	public const int ANIMATION_LOOPING = 0;
	public const int ANIMATION_NONLOOPING = 1;
	public static float ANIMATION_FPS = 1/60;	// reproduces at 1/60 fps
	
	public KeyFrame[] keyFrames;
	public float frameDuration;
	public float speed;

	public short actionFrame;

	public SpriteAnimation(Sprite[] keyFrames, float speed = 1.0f, float fps = 60, short actionFrame = -1){
		this.keyFrames = new KeyFrame[keyFrames.Length];

		for(int i = 0; i < keyFrames.Length; i++)
		{
			this.keyFrames[i] = new KeyFrame();
			this.keyFrames[i].sprite = keyFrames[i];
		}
//		this.keyFrames = keyFrames;
		this.frameDuration = 1/fps;
		this.speed = speed;
		this.actionFrame = actionFrame;
	}

	public void setEventOnKeyFrame(int frame, System.Action keyEvent)
	{
		if(keyFrames[frame].events == null)
			keyFrames[frame].events = new List<System.Action>();

		keyFrames[frame].events.Add(keyEvent);
	}

	public KeyFrame getKeyFrame(float stateTime, int mode) {
		return keyFrames[getFrameNumber(stateTime, mode)];
	}
	
	public KeyFrame getKeyFrame(int frame) {
		return keyFrames[frame];
	}

	public int getFrameNumber(float stateTime, int mode) {
		int frameNumber = (int)(stateTime / (frameDuration* (1/speed)) );
		
		if(mode == ANIMATION_NONLOOPING) {
			frameNumber = Mathf.Min(keyFrames.Length-1, frameNumber);            
		} else {
			frameNumber = frameNumber % keyFrames.Length;
		}
		return frameNumber;
	}

	public bool isAtFrame(short targetFrame, float stateTime, int mode) {
		return getFrameNumber(stateTime, mode) == targetFrame;
	}

	public float getDurationUntil(short frame){
//		Debug.Log("DurationUntilFrame:: fDuration " + frameDuration + " frame " + frame + " speed " + (1/speed) );
		return (frameDuration * frame) * 1/speed;
	}

	public float getDuration(){
		return (frameDuration * keyFrames.Length)* 1/speed;
	}

	public bool isFinish(float stateTime){
		//Debug.Log ("SpriteAnim::isFinish " + stateTime + " DurTOTAL " + getDuration());
		return stateTime >= getDuration();
	}

	public float getSpeed(){
		return speed;
	}

	public void setSpeed(float speed){
		if(speed <= 0){
			speed = 0.0001f;
		}
		this.speed = speed;
	}
}


public class SpriteCache {

	private static SpriteCache instance;
	private Dictionary <string, Sprite[]> dic = new Dictionary<string, Sprite[]>();

	private SpriteCache() {}

	public static SpriteCache Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new SpriteCache();
			}
			return instance;
		}
	}
	
	public void loadSprites(string pListPath, string animName){

		Sprite[] sprites =  Resources.LoadAll<Sprite>(pListPath);
		List<Sprite> animSprites = new List<Sprite>();
		//Debug.Log ("spritesc " + sprites.Length);
		for(int i = 0; i < sprites.Length; i++){
			if(sprites[i].name.Contains(animName)){
				animSprites.Add(sprites[i]);
			}
		}

		if(!dic.ContainsKey(animName)){
			dic.Add(animName, animSprites.ToArray());
		}else {
			dic[animName] = animSprites.ToArray();
		}
	}

	public Sprite[] getSprites(string animName){
		if(dic.ContainsKey(animName))
			return dic[animName];
		else 
			return null;	// at this moment better to return null
			//throw new UnityException("Sprites[] of the "+ animName + " are not loaded to SpriteCache");
	}
}



/*
public class SpriteAnimation {
	
	public const int ANIMATION_LOOPING = 0;
	public const int ANIMATION_NONLOOPING = 1;
	
	public Sprite[] keyFrames;
	public float frameDuration;
	
	public SpriteAnimation(Sprite[] keyFrames, float frameDuration){
		this.keyFrames = keyFrames;
		this.frameDuration = frameDuration;
	}
	
	public Sprite getKeyFrame(float stateTime, int mode) {
		int frameNumber = (int)(stateTime / frameDuration);
		
		if(mode == ANIMATION_NONLOOPING) {
			frameNumber = Mathf.Min(keyFrames.Length-1, frameNumber);            
		} else {
			frameNumber = frameNumber % keyFrames.Length;
		}
		return keyFrames[frameNumber];
	}
	
	public int getFrameNumber(float stateTime, int mode) {
		int frameNumber = (int)(stateTime / frameDuration);
		
		if(mode == ANIMATION_NONLOOPING) {
			frameNumber = Mathf.Min(keyFrames.Length-1, frameNumber);            
		} else {
			frameNumber = frameNumber % keyFrames.Length;
		}
		return frameNumber;
	}
	
	public bool isAtFrame(short targetFrame, float stateTime, int mode) {
		return getFrameNumber(stateTime, mode) == targetFrame;
	}
	
	public float getDurationUntil(short frame){
		return frameDuration * frame;
	}
	
	public float getDuration(){
		return frameDuration * keyFrames.Length;
	}
	
	public bool isFinish(float stateTime){
		return stateTime >= getDuration();
	}
	
	public float getSpeed(){
		return keyFrames.Length / 60;
	}
	
}
*/
