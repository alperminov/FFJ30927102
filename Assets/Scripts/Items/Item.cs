using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item  {

	public Texture2D sprite { get; set; }
	public Sprite spriteInCell { get; set; }
	public int itemCount { get; set; }
	protected int maxItemCount;

	protected void loadResources(string spriteName, string spriteInCellName) {
		sprite = Resources.Load<Texture2D> (spriteName);
		spriteInCell = Resources.Load<Sprite> (spriteInCellName);
	}

	public bool joinItems(Item joiningItem) {
		if (this.GetType () == joiningItem.GetType ()) {
			if ((this.itemCount + joiningItem.itemCount) <= this.maxItemCount) {
				this.itemCount += joiningItem.itemCount;
				return true;
			}				
		}
		return false;
	}

	public int getMaxItemCount() {
		return maxItemCount;
	}
}
