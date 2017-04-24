using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container: Transform {

	public List<GameObject> containerCells { get; }

	private static Item draggedItem; //Предмет, который игрок перетаскивает
	private static ContainerCell previousInventoryCell; //Блок, из которого игрок начал тащить предмет. Необходим для свопа обьектов в инвентаре

	public Container (int cellCount, int columnCount, string containerTag, GameObject cellPrefab) {

		containerCells = new List<GameObject> ();
		GameObject[] panels = GameObject.FindGameObjectsWithTag(containerTag);
		RectTransform cellRectTransform = cellPrefab.GetComponent<RectTransform> ();
		RectTransform containerRectTransform = GameObject.FindGameObjectWithTag (containerTag).GetComponent<RectTransform>();

		float width = containerRectTransform.rect.width / columnCount;
		float ratio = width / cellRectTransform.rect.width;
		float height = cellRectTransform.rect.height * ratio;
		int rowCount = cellCount / columnCount;
		if (cellCount % rowCount > 0)
			rowCount++;
		foreach (GameObject panel in panels) {
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
		
	public static void processMClick(float mouseX, float mouseY, List<GameObject> containerCells) {
		foreach (GameObject cell in containerCells) {

			ContainerCell containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();

			if (CursorIsInBCell(rectTransform, mouseX, mouseY)) {
				if (containerCell.item == null)
					containerCell.item = new Axe ();
				else
					containerCell.item.joinItems (new Axe ());
			}
		}

	}

	public static void processRClick(float mouseX, float mouseY, List<GameObject> containerCells) {
		foreach (GameObject cell in containerCells) {

			ContainerCell containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();

			if (CursorIsInBCell(rectTransform, mouseX, mouseY)) {
				if (containerCell.item == null)
					containerCell.item = new Hammer ();
				else
					containerCell.item.joinItems (new Hammer ());
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
		bool cursorIsInCell = false;
		ContainerCell containerCell = null;
		Item item = null;

		foreach (GameObject cell in containerCells) {
			containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();
			item = containerCell.item;

			if (CursorIsInBCell (rectTransform, mouseX, mouseY)) {
				cursorIsInCell = true;
				break;
			}
		}

		if (cursorIsInCell && draggedItem != null) {
			if (item == null) 
				containerCell.item = draggedItem;
			else if (!item.joinItems(draggedItem)) {
				previousInventoryCell.item = item;
				containerCell.item = draggedItem;	
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

}
