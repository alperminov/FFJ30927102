using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item  {

	public Texture2D sprite { get; set; }
	public Sprite spriteInCell { get; set; }
	protected int stackSize;

	protected void loadResources(string spriteName, string spriteInCellName) {
		sprite = Resources.Load<Texture2D> (spriteName);
		spriteInCell = Resources.Load<Sprite> (spriteInCellName);
	}

	public int getStackSize() {
		return stackSize;
	}
}
