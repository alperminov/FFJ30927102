using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Item {

	public string spriteName = "Items/hammer";
	public string spriteInCellName = "Items/hammer_in_cell";

	public Hammer() {
		sprite = Resources.Load<Texture2D> (spriteName);
		spriteInCell = Resources.Load<Sprite> (spriteInCellName);
	}

}
