using UnityEngine;
using System.Collections.Generic;

public class Unit: MonoBehaviour {

	private static bool animLoaded = false;

	public enum Orientation
	{
		Or_N,			   // North (up)
		Or_NW,             // Nort-West (up-left)
		Or_W,              // West (left)
		Or_SW,             // South-West (down-left)
		Or_S,              // South (down)
		Or_SE,             // South-East (down-right)
		Or_E,              // East (right)
		Or_NE              // North-East (up-right)
	};

	static string[] unitAnims = new string[]
	{
		"walk",
	};

	private AnimatorSystem animSys;

	void Awake()
	{
		if(!animLoaded)
		{
			animLoaded = true;
			string path = "Sprites/Units/thug_all";
			Unit.load(path, "thug");
		}

		animSys = new AnimatorSystem(transform, this);
		loadAnimations("thug");
		setAnimAndOr("walk", Orientation.Or_E);
	}

	void Update()
	{
		float dt = Time.deltaTime;
		animSys.update(dt);
	}

	public void setAnimAndOr(string newAnim, Orientation or)
	{
		animSys.setAnim(newAnim, or);
	}

	//
	// Load walk, attack, etc... from the unitType (thug, giant, etc)
	//
	public static bool load(string path, string unitType){
		string unitFile;
		for(int i=0; i < unitAnims.Length; i++){
			unitFile = unitType + "_" + unitAnims[i] + "_n";
			SpriteCache.Instance.loadSprites(path, unitFile);

			unitFile = unitType + " " + unitAnims[i] + "_ne";
			SpriteCache.Instance.loadSprites(path, unitFile);

			unitFile = unitType + "_" + unitAnims[i] + "_e";
			SpriteCache.Instance.loadSprites(path, unitFile);

			unitFile = unitType + " " + unitAnims[i] + "_se";
			SpriteCache.Instance.loadSprites(path, unitFile);

			unitFile = unitType + "_" + unitAnims[i] + "_s";
			SpriteCache.Instance.loadSprites(path, unitFile);
		}

		return true;
	}
	
	public bool loadAnimations(string unitType){	
		Sprite[] sprites;
		string unitFile;

		// for every possible orientation (hard-coded, only 5 orientations defined, the other 3 are mirrored)
		for(int i=0; i < unitAnims.Length; i++){

			try
			{
				unitFile = unitType + "_" + unitAnims[i] + "_n";
				sprites  = SpriteCache.Instance.getSprites(unitFile);
				if(sprites != null)
					animSys.loadAnimation(unitAnims[i], Unit.Orientation.Or_N, sprites);

				unitFile = unitType + "_" + unitAnims[i] + "_n";//TODO _ne
				sprites  = SpriteCache.Instance.getSprites(unitFile);
				if(sprites != null)
					animSys.loadAnimation(unitAnims[i], Unit.Orientation.Or_NE, sprites);
				
				unitFile = unitType + "_" + unitAnims[i] + "_e";
				sprites  = SpriteCache.Instance.getSprites(unitFile);
				if(sprites != null)
					animSys.loadAnimation(unitAnims[i], Unit.Orientation.Or_E ,sprites);
				
				unitFile = unitType + "_" + unitAnims[i] + "_s";//TODO _se
				sprites  = SpriteCache.Instance.getSprites(unitFile);
				if(sprites != null)
					animSys.loadAnimation(unitAnims[i], Unit.Orientation.Or_SE, sprites);
				
				unitFile = unitType + "_" + unitAnims[i] + "_s";
				sprites  = SpriteCache.Instance.getSprites(unitFile);
				if(sprites != null)
					animSys.loadAnimation(unitAnims[i], Unit.Orientation.Or_S, sprites);

			}
			catch(UnityException e)
			{
				Debug.LogError (e);
			}
		}

		return true;
	}


}
