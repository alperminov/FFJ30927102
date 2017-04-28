using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container: Transform {

	public List<GameObject> containerCells { get; }
	private static List<Item> draggedItems; //Стак предметов, который игрок перетаскивает
	private static ContainerCell previousInventoryCell; //Блок, из которого игрок начал тащить предмет. Необходим для свопа обьектов в инвентаре
	private int freeSpace;

	public Container (int cellCount, int columnCount, string containerTag, GameObject cellPrefab) {
		
		GameObject[] panels = GameObject.FindGameObjectsWithTag(containerTag);
		RectTransform cellRectTransform = cellPrefab.GetComponent<RectTransform> ();
		freeSpace = cellCount;
		containerCells = new List<GameObject> ();
		draggedItems = new List<Item> ();

		foreach (GameObject panel in panels) {
		
			RectTransform containerRectTransform = panel.GetComponent<RectTransform> ();
			float width = containerRectTransform.rect.width / columnCount;
			float ratio = width / cellRectTransform.rect.width;
			float height = cellRectTransform.rect.height * ratio;
			//Генерация сетки инвентаря
			int j = 0;
			for (int i = 0; i < cellCount; i++) {

				if (i % columnCount == 0)
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
	}
		
	public void processMClick(float mouseX, float mouseY) {
		foreach (GameObject cell in containerCells) {

			ContainerCell containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();

			if (CursorIsInBCell(rectTransform, mouseX, mouseY)) {
				List<Item> axes = new List<Item> ();
				for (int i = 0; i < 40; i++)
					axes.Add (new Axe ());
				containerCell.addItems(axes);
			}
		}

	}

	public void processRClick(float mouseX, float mouseY) {
		foreach (GameObject cell in containerCells) {

			ContainerCell containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();

			if (CursorIsInBCell(rectTransform, mouseX, mouseY)) {
				List<Item> hammers = new List<Item> ();
				for (int i = 0; i < 40; i++)
					hammers.Add (new Hammer ());
				containerCell.addItems(hammers);
			}
		}

	}

	//Реализация Drag
	public void processKeyHold(float mouseX, float mouseY) {

		foreach (GameObject cell in containerCells) {
			ContainerCell containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();
			List<Item> items = containerCell.items;
			if (CursorIsInBCell(rectTransform, mouseX, mouseY)) {
				if (items.Count > 0) {
					previousInventoryCell = containerCell;
					Cursor.SetCursor(items[0].sprite, Vector2.zero, CursorMode.Auto);
					draggedItems.AddRange(items);
					containerCell.items.Clear();
					break;
				}
			}
		}
	}

	//Реализация Drop
	public void processKeyRelease(float mouseX, float mouseY) {
		
		ContainerCell containerCell = null;
		foreach (GameObject cell in containerCells) {
			
			containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();

			if (CursorIsInBCell (rectTransform, mouseX, mouseY)) {
				if (draggedItems.Count > 0)
					moveItems (draggedItems, previousInventoryCell, containerCell);
				break;
			}
		}

		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		draggedItems.Clear();
	}

	//Проверка нахождения курсора внутри какой либо из ячеек инвентаря
	bool CursorIsInBCell(RectTransform cellRectTransf, float mouseX, float mouseY) {
		
		if ((mouseX >= (cellRectTransf.position.x - cellRectTransf.rect.width / 2)) && (mouseX <= (cellRectTransf.position.x + cellRectTransf.rect.width / 2)) &&
			(mouseY >= (cellRectTransf.position.y - cellRectTransf.rect.height / 2)) && (mouseY <= (cellRectTransf.position.y + cellRectTransf.rect.height / 2))) {
			return true;
		}
		return false;
	}
		

	public List<Item> moveItems(List<Item> items, ContainerCell fromCell, ContainerCell toCell) {
		int itemsCount = items.Count;
		List<Item> result = toCell.addItems (items);

		if (fromCell != null) {
			if (result.Count == itemsCount) {
				List<Item> buffer = new List<Item> ();
				buffer.AddRange (toCell.items);
				fromCell.items.Clear ();
				toCell.items.Clear ();
				fromCell.items.AddRange(buffer);
				toCell.items.AddRange(result);

			} else 
				fromCell.items.AddRange (result);
			result.Clear ();
		}

		return result;
	}

	/*
	List<Item> moveItems(List<Item> items, ContainerCell toCell) {
		List<Item> resultItems = items;

		if ((toCell == null) || (items.Count == 0)) {
			Debug.Log (resultItems.Count);
			return items;
		}
		else {
			resultItems = moveItems (items, findCell (items[0].GetType()));
			resultItems = toCell.addItems (resultItems);
			return resultItems;
		}
			
	}
	*/

	ContainerCell findCell(System.Type itemType, List<GameObject> containerCells) {
		
		foreach (GameObject cell in containerCells) {
			ContainerCell containerCell = cell.GetComponent<ContainerCell> ();
			if ((containerCell.items.Count == 0) || ((containerCell.getFreeSpace () > 0) && (containerCell.items [0].GetType () == itemType))) {
				Debug.Log (cell.name);
				Debug.Log (containerCell.getFreeSpace ());
				return containerCell;
			}
		}
		return null;
	}

	public List<Item> addItems(List<Item> items, string storageName) {
		GameObject storage = GameObject.Find (storageName);
		foreach (Transform child in storage.transform) {
			ContainerCell cell = child.GetComponent<ContainerCell> ();
			items = cell.addItems (items);
		}
		return items;
	}
}
