using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item {
	
	public string spriteName = "Items/axe";
	public string spriteInCellName = "Items/axe_in_cell";

	public Axe() {
		stackSize = 99;
		loadResources (spriteName, spriteInCellName);
	}
}
