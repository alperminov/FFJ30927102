using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item  {

	protected Texture2D sprite;
	protected Sprite spriteInBlock; 

	public Texture2D getSprite () {
		return sprite;
	}

	public Sprite getSpriteInBlock() {
		return spriteInBlock;
	}
}
