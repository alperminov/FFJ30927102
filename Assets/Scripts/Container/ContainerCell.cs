using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerCell : MonoBehaviour {

	public List<Item> items { get; set; }
	string defaultSprite = "Container/Cell";
	Image itemImage;
	Text itemCountText;
	int itemCount;

	// Use this for initialization
	void Start () {
		itemCount = 0;
		items = new List<Item> ();
		itemCountText = transform.Find("ItemCount").GetComponent<Text> ();
		itemImage = GetComponent<Image> ();
	}

	//Если в блоке нет ничего, грузится дефолтный спрайт, иначе спрайт предмета
	void Update () {
		if (items.Count == 0) {
			itemCountText.text = "";
			itemImage.sprite = Resources.Load<Sprite> (defaultSprite);
		}
		else {
			itemCount = items.Count;
			itemImage.sprite = items[0].spriteInCell;
			if (itemCount > 1)
				itemCountText.text = itemCount.ToString ();
			else
				itemCountText.text = "";
		}
	}

	public Item addItem(Item item) {
		
		if ((items.Count == 0) || ((item.GetType () == items[0].GetType()) && (items.Count < items[0].getStackSize ()))) {
			items.Add (item);
			return null;
		}
		return item;
	}

	public List<Item> addItems(List<Item> items) {
		
		List<Item> rejectedItems = new List<Item> ();

		foreach (Item item in items) 
			if (addItem (item) != null) 
				rejectedItems.Add (item);

		return rejectedItems;
	}
			
	public static List<Item> getItemStack(List<Item> items) {
		List<Item> resultStack;
		if (items.Count > 0) {
			if (items.Count <= items[0].getStackSize ()) {
				resultStack = items.GetRange (0, items.Count);
				items.Clear();
			} else {
				resultStack = items.GetRange (0, items[0].getStackSize ());
				items.RemoveRange (0, items[0].getStackSize ());
			}
		}
		return items;
	}

	public int getFreeSpace() {
		return items.Count > 0 ? items[0].getStackSize () - items.Count : int.MaxValue;
	}

}
