using UnityEngine;
using System.Collections.Generic;



public delegate void ActionFrame();

public class AnimatorSystem 
{

	public event ActionFrame OnActionFrame;

	private static Quaternion flipLeft  = Quaternion.Euler(0,0,0);
	private static Quaternion flipRight = Quaternion.Euler(0,180,0);

	private float stateTime;     
	private SpriteRenderer spRenderer;
	private Unit owner;
	private Transform spriteTransform;
	private SpriteAnimation curAnim;
	private float speed;
	private float speedRecorded;
	private int animationMode;
	private KeyFrame curKeyFrame;


	private Dictionary<string, Dictionary<Unit.Orientation, SpriteAnimation>> animations = 
					new Dictionary<string, Dictionary<Unit.Orientation, SpriteAnimation>>();


	private System.Action animFinishHandler;
	private bool checkAnimFinish;
	private int curFrame;

	public AnimatorSystem(Transform spriteTransform, Unit owner){
		this.spriteTransform = spriteTransform;
		this.spRenderer = spriteTransform.GetComponent<SpriteRenderer>();
		this.owner = owner;
		this.stateTime = 0;
		this.speed = 1.0f;
		this.speedRecorded = 1.0f;
		animationMode = SpriteAnimation.ANIMATION_LOOPING;

	}

	public bool loadAnimation(string animStr, Unit.Orientation or, Sprite[] sprites){
		SpriteAnimation spAnimation = new SpriteAnimation(sprites, 0.2f);
		//Debug.Log ("sprites " + sprites.Length + " animStr " + animStr);
		if(animations.ContainsKey(animStr)){
			animations[animStr].Add(or, spAnimation);
		}else {
			Dictionary <Unit.Orientation, SpriteAnimation> dic = new Dictionary<Unit.Orientation, SpriteAnimation> ();
			dic.Add(or, spAnimation);
			animations.Add(animStr, dic);
		}

		return true;
	}

	public void setActionFrames(string animStr, Unit.Orientation or, int frame, IList<System.Action> actions)
	{
		animations[animStr][or].keyFrames[frame].events = actions;
	}

	public void update(float dt){
		stateTime += dt * speed;

		int newFrame = curAnim.getFrameNumber(stateTime, animationMode);
		if(newFrame != curFrame)
		{
			curFrame = newFrame;
			curKeyFrame = curAnim.getKeyFrame(stateTime, animationMode);
			spRenderer.sprite = curKeyFrame.sprite;
			if(curKeyFrame.events != null)
				fireEvents(curKeyFrame.events);
		}
	}

	private void fireEvents(IList<System.Action> events)
	{
		for(int i = 0; i < events.Count; i++)
		{
			events[i]();
		}
	}
	public void setAnim(string animName, Unit.Orientation or, int animationMode = SpriteAnimation.ANIMATION_LOOPING){
		 
		if(or == Unit.Orientation.Or_W)
		{
			spriteTransform.rotation = flipRight;
			or = Unit.Orientation.Or_E;
			
		}else if(or == Unit.Orientation.Or_E)
		{
			spriteTransform.rotation = flipLeft;
		}


		curAnim = animations[animName][or];

		if(this.animationMode != animationMode)
			this.animationMode = animationMode;
	}
	
	public bool isAnimFinish(){

		return stateTime >= curAnim.getDuration();
	}

	public float getAnimDurationSec(string animName, Unit.Orientation or){
		return animations[animName][or].getDuration() * 1/speed;
	}

	public float getAnimDurationUntilFrame(short frame, string animName, Unit.Orientation or){
		return animations[animName][or].getDurationUntil(frame) * 1/speed;
	}

	public float getAnimSpeed(string animName, Unit.Orientation or){
		return animations[animName][or].getSpeed();
	}

	public void setAnimSpeed(float speed, string animName, Unit.Orientation or){
		animations[animName][or].setSpeed(speed);
	}
	
	public void setSpeed(float speed){
		this.speed = speed;
	}

	public float getSpeed(){
		return speed;
	}

	public void setStateTime(float time){
		stateTime = time;
	}
	public bool isAtFrame(short frameNumber){
		return curAnim.isAtFrame(frameNumber, stateTime, SpriteAnimation.ANIMATION_LOOPING);
	}

	public void setSortingOrder(int order){
		spRenderer.sortingOrder = order;
	}

	public void setColor(Color c){
		spRenderer.color = c;
	}

	public void saveSpeed(){
		speedRecorded = speed;
	}

	public void restoreSpeed(){
		speed = speedRecorded;
	}

	public void callFuncWhenAnimIsFinish(System.Action animFinishHandler){
		this.animFinishHandler = animFinishHandler;
		checkAnimFinish = true;
	}

	public void cancelAnimFinishCheck(){
		checkAnimFinish = false;
	}

	public bool isAnimFinishCheckOn(){
		return checkAnimFinish;
	}
}

