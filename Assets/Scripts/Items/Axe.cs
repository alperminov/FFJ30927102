using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item {
	
	public string spriteName = "Items/axe";
	public string spriteInBlockName = "Items/axe_in_block";

	public Axe() {
		sprite = Resources.Load<Texture2D> (spriteName);
		spriteInBlock = Resources.Load<Sprite> (spriteInBlockName);
	}

}
