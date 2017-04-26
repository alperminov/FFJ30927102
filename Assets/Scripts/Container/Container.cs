using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container: Transform {

	public List<GameObject> containerCells { get; }

	private static Item draggedItem; //Предмет, который игрок перетаскивает
	private static ContainerCell previousInventoryCell; //Блок, из которого игрок начал тащить предмет. Необходим для свопа обьектов в инвентаре
	private int freeSpace;

	public Container (int cellCount, int columnCount, string containerName, GameObject cellPrefab) {
		
		containerCells = new List<GameObject> ();
		GameObject panel = GameObject.Find(containerName);
		RectTransform cellRectTransform = cellPrefab.GetComponent<RectTransform> ();
		RectTransform containerRectTransform = panel.GetComponent<RectTransform>();
		freeSpace = cellCount;

		float width = containerRectTransform.rect.width / columnCount;
		float ratio = width / cellRectTransform.rect.width;
		float height = cellRectTransform.rect.height * ratio;
		//Генерация сетки инвентаря
		int j = 0;
		for (int i = 0; i < cellCount; i++) {

			if ( i % columnCount == 0)
				j++;
				
			GameObject newCell = Instantiate (cellPrefab) as GameObject;
			newCell.name = panel.name + " cell at (" + i + "," + j + ")";
			newCell.transform.SetParent (panel.transform);

			//Преобразование ячейки до нужного размера
			RectTransform rectTransform = newCell.GetComponent<RectTransform> ();

			float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
			float y = containerRectTransform.rect.height / 2 - height * j;
			rectTransform.offsetMin = new Vector2 (x, y);

			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height;
			rectTransform.offsetMax = new Vector2 (x, y);

			containerCells.Add (newCell);
		}
	}
		
	public static void processMClick(float mouseX, float mouseY, List<GameObject> containerCells) {
		foreach (GameObject cell in containerCells) {

			ContainerCell containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();

			if (CursorIsInBCell(rectTransform, mouseX, mouseY)) {
				//this.addItem (new Axe ());
				moveItem (new Axe (30), null, containerCell, containerCells);
			}
		}

	}

	public static void processRClick(float mouseX, float mouseY, List<GameObject> containerCells) {
		foreach (GameObject cell in containerCells) {

			ContainerCell containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();

			if (CursorIsInBCell(rectTransform, mouseX, mouseY)) {
				moveItem (new Hammer (210), null, containerCell, containerCells);
			}
		}

	}

	//Реализация Drag
	public static void processKeyHold(float mouseX, float mouseY, List<GameObject> containerCells) {

		foreach (GameObject cell in containerCells) {
			ContainerCell containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();
			Item item = containerCell.item;
			if (CursorIsInBCell(rectTransform, mouseX, mouseY)) {
				if (item != null) {
					previousInventoryCell = containerCell;
					Cursor.SetCursor(item.sprite, Vector2.zero, CursorMode.Auto);
					draggedItem = item;
					containerCell.item = null;
					break;
				}
			}
		}
	}

	//Реализация Drop
	public static void processKeyRelease(float mouseX, float mouseY, List<GameObject> containerCells) {
		
		ContainerCell containerCell = null;
		foreach (GameObject cell in containerCells) {
			
			containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();

			if (CursorIsInBCell (rectTransform, mouseX, mouseY)) {
				if (draggedItem != null)
					moveItem (draggedItem, previousInventoryCell, containerCell, containerCells);
				break;
			}
		}

		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		draggedItem = null;
	}

	//Проверка нахождения курсора внутри какой либо из ячеек инвентаря
	static bool CursorIsInBCell(RectTransform cellRectTransf, float mouseX, float mouseY) {
		
		if ((mouseX >= (cellRectTransf.position.x - cellRectTransf.rect.width / 2)) && (mouseX <= (cellRectTransf.position.x + cellRectTransf.rect.width / 2)) &&
			(mouseY >= (cellRectTransf.position.y - cellRectTransf.rect.height / 2)) && (mouseY <= (cellRectTransf.position.y + cellRectTransf.rect.height / 2))) {
			return true;
		}
		return false;
	}

	public Item addItem(Item item) {
		
		bool itemsJoined = false;

		if (freeSpace > 0) {
			ContainerCell firstEmptyCell = null;
			foreach (GameObject cell in containerCells) {

				ContainerCell containerCell = cell.GetComponent<ContainerCell> ();

				if (containerCell.item == null) {
					if (firstEmptyCell == null)
						firstEmptyCell = containerCell;
				}

				else if (containerCell.item.joinItems (item) == null) {
					break;
				}
			}
		}
		return item;
	}

	//Возвращает объекты которые не удалось добавить
	public List<Item> addItems(List<Item> itemList) {
		int i;
		int itemCount = itemList.Count;
		for (i = 0; i < itemCount; i++)
			if (addItem (itemList [i]).itemCount == 0)
				break;
		
		if (i < itemCount)
			return itemList.GetRange (i, itemCount - i);
		else
			return null;

	}

	public static Item moveItem(Item item, ContainerCell fromCell, ContainerCell toCell, List<GameObject> containerCells) {
		Item result = moveItem (item, toCell, containerCells);
		if (fromCell != null) {
			fromCell.item = result;
			return null;
		}
		return result;
	}

	static Item moveItem(Item item, ContainerCell toCell, List<GameObject> containerCells) {
		Item result = item;

		if (toCell != null) {
			if (toCell.item == null)
				toCell.item = item.getStack ();
			result = toCell.item.joinItems (item);
		}
		else {
			ContainerCell emptyCell = findEmptyCell (containerCells);

			if (emptyCell == null)
				return item;
			
			emptyCell.item = toCell.item.joinItems (item);
		}

		if (result.itemCount == 0)
			return result;
		else {
			result = moveItem (item, null, containerCells);
			return result;
		}
			
	}

	static ContainerCell findEmptyCell(List<GameObject> containerCells) {
		foreach (GameObject cell in containerCells) {
			ContainerCell containerCell = cell.GetComponent<ContainerCell> ();
			if (containerCell.item == null)
				return containerCell;
		}
		return null;
	}
}
