using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {


	const int MOUSE_LEFT_BUTTON = 0;
	const int MOUSE_RIGHT_BUTTON = 1;
	const int MOUSE_WHEEL_BUTTON = 2;

	public GameObject inventoryBlockPrefab; //Ссылка блок инвентаря в Unity, задается в Unity UI
	List<GameObject> inventoryBlocks = new List<GameObject>();
	public int blockCount = 16, columnCount = 4; //Параметры сетки инвентаря
	Item draggedItem; //Предмет, который игрок перетаскивает
	bool mouseLkHold; //Флаг зажатия ЛКМ
	float clickTime= 0f;
	InventoryBlock previousInvBlock; //Блок, из которого игрок начал тащить предмет. Необходим для свопа обьектов в инвентаре


    void Start () {
		
		mouseLkHold = false;

		RectTransform rowRectTransform = inventoryBlockPrefab.GetComponent<RectTransform>();
		RectTransform containerRectTransform = gameObject.GetComponent<RectTransform>();


		float width = containerRectTransform.rect.width / columnCount;
		float ratio = width / rowRectTransform.rect.width;
		float height = rowRectTransform.rect.height * ratio;
		int rowCount = blockCount / columnCount;
		if (blockCount % rowCount > 0)
			rowCount++;

		//Генерация сетки инвентаря
		int j = 0;
		for (int i = 0; i < blockCount; i++)
		{

			if (i % columnCount == 0)
				j++;


			GameObject newBlock = Instantiate(inventoryBlockPrefab) as GameObject;
			newBlock.name = gameObject.name + " block at (" + i + "," + j + ")";
			newBlock.transform.SetParent(gameObject.transform);

			//Преобразование ячейки до нужного размера
			RectTransform rectTransform = newBlock.GetComponent<RectTransform>();

			float x = -containerRectTransform.rect.width / 2 + width * (i % columnCount);
			float y = containerRectTransform.rect.height / 2 - height * j;
			rectTransform.offsetMin = new Vector2(x, y);

			x = rectTransform.offsetMin.x + width;
			y = rectTransform.offsetMin.y + height;
			rectTransform.offsetMax = new Vector2(x, y);

			inventoryBlocks.Add (newBlock);

		}
    }
	

	void Update () {
		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;

		clickTime += Time.deltaTime;


		if (Input.GetMouseButtonDown (MOUSE_RIGHT_BUTTON)) {
			processRClick (mouseX, mouseY);

		}
			
		if (Input.GetMouseButtonDown (MOUSE_WHEEL_BUTTON)) {
			processMClick (mouseX, mouseY);

		}

		//Обработка зажатия ЛКМ
		if (Input.GetMouseButton (MOUSE_LEFT_BUTTON) && !mouseLkHold && clickTime > 1) {
			mouseLkHold = true;
			processKeyHold (mouseX, mouseY);
			clickTime = 0;
		} else if (!Input.GetMouseButton (MOUSE_LEFT_BUTTON) && mouseLkHold) {
			processKeyRelease (mouseX, mouseY);
			mouseLkHold = false;
		}
		

	}

	void processMClick(float mouseX, float mouseY) {
		foreach (GameObject block in inventoryBlocks) {
			
			InventoryBlock inventoryBlock = block.GetComponent<InventoryBlock> ();
			RectTransform rectTransform = block.GetComponent<RectTransform> ();

			if (CursorIsInBlock(rectTransform, mouseX, mouseY)) {
				inventoryBlock.setItem (new Axe ());
			}
		}
			
	}

	void processRClick(float mouseX, float mouseY) {
		foreach (GameObject block in inventoryBlocks) {

			InventoryBlock inventoryBlock = block.GetComponent<InventoryBlock> ();
			RectTransform rectTransform = block.GetComponent<RectTransform> ();

			if (CursorIsInBlock(rectTransform, mouseX, mouseY)) {
				inventoryBlock.setItem (new Hammer ());
			}
		}

	}

	//Реализация Drag
	void processKeyHold(float mouseX, float mouseY) {
		
		foreach (GameObject block in inventoryBlocks) {
			InventoryBlock inventoryBlock = block.GetComponent<InventoryBlock> ();
			RectTransform rectTransform = block.GetComponent<RectTransform> ();
			Item item = inventoryBlock.getItem ();
			if (CursorIsInBlock(rectTransform, mouseX, mouseY)) {
				if (item != null) {
					previousInvBlock = inventoryBlock;
					Cursor.SetCursor(item.getSprite (), Vector2.zero, CursorMode.Auto);
					draggedItem = item;
					inventoryBlock.setItem (null);
					break;
				}
			}
		}
	}

	//Реализация Drop
	void processKeyRelease(float mouseX, float mouseY) {
		bool cursorIsInBlock = false;
		InventoryBlock inventoryBlock = null;
		Item item = null;

		foreach (GameObject block in inventoryBlocks) {
			inventoryBlock = block.GetComponent<InventoryBlock> ();
			RectTransform rectTransform = block.GetComponent<RectTransform> ();
			item = inventoryBlock.getItem ();

			if (CursorIsInBlock (rectTransform, mouseX, mouseY)) {
				cursorIsInBlock = true;
				break;
			}
		}

		if (cursorIsInBlock && draggedItem != null) {
			if (item == null) {
				inventoryBlock.setItem (draggedItem);
			} else {
				previousInvBlock.setItem (item);
				inventoryBlock.setItem (draggedItem);
			}
		}
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		draggedItem = null;
	}

	//Проверка нахождения курсора внутри какой либо из ячеек инвентаря
	bool CursorIsInBlock(RectTransform blockRectTransf, float mouseX, float mouseY) {
		if ((mouseX >= (blockRectTransf.position.x - blockRectTransf.rect.width / 2)) && (mouseX <= (blockRectTransf.position.x + blockRectTransf.rect.width / 2)) &&
			(mouseY >= (blockRectTransf.position.y - blockRectTransf.rect.height / 2)) && (mouseY <= (blockRectTransf.position.y + blockRectTransf.rect.height / 2))) {
			return true;
		}
		return false;
	}
}
