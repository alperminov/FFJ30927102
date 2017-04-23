using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour {

	const int MOUSE_LEFT_BUTTON = 0;
	const int MOUSE_RIGHT_BUTTON = 1;
	const int MOUSE_WHEEL_BUTTON = 2;

	bool inventoryIsActive;
	Canvas canvas;
	public GameObject containerCellPrefab; //Ссылка блок инвентаря в Unity, задается в Unity UI
	public string containerTag;
	public string playerInventoryTag;
	public int containerCellCount = 16, inventoryColumnCount = 4; //Параметры сетки инвентаря
	Item draggedItem; //Предмет, который игрок перетаскивает
	bool mouseLkHold; //Флаг зажатия ЛКМ
	float clickTime= 0f;
	ContainerCell previousInventoryCell; //Блок, из которого игрок начал тащить предмет. Необходим для свопа обьектов в инвентаре
	Container playerInventory;

	// Use this for initialization
	void Start () {
		canvas = GetComponent<Canvas> ();
		canvas.enabled = false;
		inventoryIsActive = false;
		playerInventory = new Container (containerCellCount, inventoryColumnCount, playerInventoryTag, containerCellPrefab);
	}
	
	// Update is called once per frame
	void Update () {

		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;

		clickTime += Time.deltaTime;

		if (inventoryIsActive) {
			if (Input.GetMouseButtonDown (MOUSE_RIGHT_BUTTON)) {
				processRClick (mouseX, mouseY);

			}

			if (Input.GetMouseButtonDown (MOUSE_WHEEL_BUTTON)) {
				processMClick (mouseX, mouseY);

			}

			//Обработка зажатия ЛКМ
			if (Input.GetMouseButtonUp (MOUSE_LEFT_BUTTON)) {
				clickTime = 0;

			}
				
			if (Input.GetMouseButton(0))
				clickTime += Time.deltaTime;
			
			if (Input.GetMouseButton (MOUSE_LEFT_BUTTON) && !mouseLkHold && clickTime > 0.6) {
				mouseLkHold = true;
				processKeyHold (mouseX, mouseY);
				clickTime = 0;
			} else if (!Input.GetMouseButton (MOUSE_LEFT_BUTTON) && mouseLkHold) {
				processKeyRelease (mouseX, mouseY);
				mouseLkHold = false;
			}
		}

		if (Input.GetKeyDown ("i")) {
			if (inventoryIsActive) {
				canvas.enabled = false;
				inventoryIsActive = false;
			}
			else {
				canvas.enabled = true;
				inventoryIsActive = true;
			}
		}
	}

	void processMClick(float mouseX, float mouseY) {
		foreach (GameObject cell in playerInventory.containerCells) {

			ContainerCell containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();

			if (CursorIsInBCell(rectTransform, mouseX, mouseY)) {
				containerCell.setItem (new Axe ());
			}
		}

	}

	void processRClick(float mouseX, float mouseY) {
		foreach (GameObject cell in playerInventory.containerCells) {

			ContainerCell containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();

			if (CursorIsInBCell(rectTransform, mouseX, mouseY)) {
				containerCell.setItem (new Hammer ());
			}
		}

	}

	//Реализация Drag
	void processKeyHold(float mouseX, float mouseY) {

		foreach (GameObject cell in playerInventory.containerCells) {
			ContainerCell containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();
			Item item = containerCell.getItem ();
			if (CursorIsInBCell(rectTransform, mouseX, mouseY)) {
				if (item != null) {
					previousInventoryCell = containerCell;
					Cursor.SetCursor(item.getSprite (), Vector2.zero, CursorMode.Auto);
					draggedItem = item;
					containerCell.setItem (null);
					break;
				}
			}
		}
	}

	//Реализация Drop
	void processKeyRelease(float mouseX, float mouseY) {
		bool cursorIsInCell = false;
		ContainerCell containerCell = null;
		Item item = null;

		foreach (GameObject cell in playerInventory.containerCells) {
			containerCell = cell.GetComponent<ContainerCell> ();
			RectTransform rectTransform = cell.GetComponent<RectTransform> ();
			item = containerCell.getItem ();

			if (CursorIsInBCell (rectTransform, mouseX, mouseY)) {
				cursorIsInCell = true;
				break;
			}
		}

		if (cursorIsInCell && draggedItem != null) {
			if (item == null) {
				containerCell.setItem (draggedItem);
			} else {
				previousInventoryCell.setItem (item);
				containerCell.setItem (draggedItem);
			}
		}
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		draggedItem = null;
	}

	//Проверка нахождения курсора внутри какой либо из ячеек инвентаря
	bool CursorIsInBCell(RectTransform cellRectTransf, float mouseX, float mouseY) {
		if ((mouseX >= (cellRectTransf.position.x - cellRectTransf.rect.width / 2)) && (mouseX <= (cellRectTransf.position.x + cellRectTransf.rect.width / 2)) &&
			(mouseY >= (cellRectTransf.position.y - cellRectTransf.rect.height / 2)) && (mouseY <= (cellRectTransf.position.y + cellRectTransf.rect.height / 2))) {
			return true;
		}
		return false;
	}
}
