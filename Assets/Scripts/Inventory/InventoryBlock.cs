using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBlock : MonoBehaviour {

	Item item;
	string defaultSprite = "Inventory/block";
	Image image;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
	}

	//Если в блоке нет ничего, грузится дефолтный спрайт, иначе спрайт предмета
	void Update () {
		if (item == null)
			image.sprite = Resources.Load<Sprite> (defaultSprite);
		else
			image.sprite = item.getSpriteInBlock ();
	}

	public void setItem(Item item) {
		this.item = item;
	}

	public Item getItem() {
		return item;
	}
}
