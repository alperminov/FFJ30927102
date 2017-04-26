using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Item {

	public string spriteName = "Items/hammer";
	public string spriteInCellName = "Items/hammer_in_cell";

	public Hammer() {
		itemCount = 1;
		stackSize = 99;
		loadResources (spriteName, spriteInCellName);
	}

	public Hammer(int count) {
		itemCount = count;
		stackSize = 99;
		loadResources (spriteName, spriteInCellName);
	}
}
