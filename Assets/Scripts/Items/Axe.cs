using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item {
	
	public string spriteName = "Items/axe";
	public string spriteInCellName = "Items/axe_in_cell";

	public Axe() {
		itemCount = 1;
		stackSize = 99;
		loadResources (spriteName, spriteInCellName);
	}

	public Axe(int count) {
		itemCount = count;
		stackSize = 99;
		loadResources (spriteName, spriteInCellName);
	}
}
