using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item  {

	public Texture2D sprite { get; set; }
	public Sprite spriteInCell { get; set; }
	public int itemCount { get; set; }
	protected int stackSize;

	protected void loadResources(string spriteName, string spriteInCellName) {
		sprite = Resources.Load<Texture2D> (spriteName);
		spriteInCell = Resources.Load<Sprite> (spriteInCellName);
	}

	public Item joinItems(Item joiningItem) {
		if (this.GetType () == joiningItem.GetType ()) {
			if ((this.itemCount + joiningItem.itemCount) <= this.stackSize) {
				this.itemCount += joiningItem.itemCount;
				joiningItem.itemCount = 0;
			} else {
				joiningItem.itemCount = this.itemCount + joiningItem.itemCount - this.stackSize;
				this.itemCount = this.stackSize;
			}
		}
		return joiningItem;
	}

	public int getMaxItemCount() {
		return stackSize;
	}

	public Item getStack() {
		Item stack = (
			(Item)this.MemberwiseClone ();
		if (this.itemCount > stackSize) {
			stack.itemCount = stackSize;
			this.itemCount -= stackSize;
			return stack;
		}
		return this;
	}

}
