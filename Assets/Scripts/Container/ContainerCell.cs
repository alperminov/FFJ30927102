using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerCell : MonoBehaviour {

	public Item item { get; set; }
	string defaultSprite = "Container/Cell";
	Image itemImage;
	Text itemCountText;

	// Use this for initialization
	void Start () {
		itemCountText = transform.Find("ItemCount").GetComponent<Text> ();
		itemImage = GetComponent<Image> ();
	}

	//Если в блоке нет ничего, грузится дефолтный спрайт, иначе спрайт предмета
	void Update () {
		if (item == null) {
			itemCountText.text = "";
			itemImage.sprite = Resources.Load<Sprite> (defaultSprite);
		}
		else {
			int itemCount = item.itemCount;
			itemImage.sprite = item.spriteInCell;
			if (itemCount > 1)
				itemCountText.text = itemCount.ToString ();
			else if (itemCount == 0) {
				item = null;
				itemCountText.text = "";
				itemImage.sprite = Resources.Load<Sprite> (defaultSprite);
			}
			else
				itemCountText.text = "";
		}
	}

}
