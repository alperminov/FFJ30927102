using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Item {

	public string spriteName = "Items/hammer";
	public string spriteInBlockName = "Items/hammer_in_block";

	public Hammer() {
		sprite = Resources.Load<Texture2D> (spriteName);
		spriteInBlock = Resources.Load<Sprite> (spriteInBlockName);
	}

}
