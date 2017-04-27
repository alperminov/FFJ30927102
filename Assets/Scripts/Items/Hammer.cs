using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Item {

	public string spriteName = "Items/hammer";
	public string spriteInCellName = "Items/hammer_in_cell";

	public Hammer() {
		stackSize = 99;
		loadResources (spriteName, spriteInCellName);
	}

}
