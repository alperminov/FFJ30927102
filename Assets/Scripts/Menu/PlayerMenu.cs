using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour {

	const int MOUSE_LEFT_BUTTON = 0;
	const int MOUSE_RIGHT_BUTTON = 1;
	const int MOUSE_WHEEL_BUTTON = 2;

	public string playerContainerName;
	public string storageContainerName;
	public string playerName;
	public string containerTag;
	public int containerColumnCount = 4;
	public int containerCellCount = 16; //Количество ячеек в контейнере
	public GameObject containerCellPrefab; //Ссылка блок инвентаря в Unity, задается в Unity UI

	private bool inventoryIsActive;
	private Canvas canvas;
	private bool mouseLkHold; //Флаг зажатия ЛКМ
	private float clickTime= 0f;
	private Container container;
	private Container storageContainer;
	private Player player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find(playerName).GetComponent<Player> ();
		canvas = GetComponent<Canvas> ();
		canvas.enabled = false;
		inventoryIsActive = false;
		container = new Container (containerCellCount, containerColumnCount, containerTag, containerCellPrefab);
		//storageContainer = new Container (storageContainerCellCount, containerColumnCount, storageContainerName, containerCellPrefab);
	}
	
	// Update is called once per frame
	//TODO: Процессить клик по обьекту, только если мышь находится в области этого обьекта
	void Update () {

		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;

		clickTime += Time.deltaTime;

		if (inventoryIsActive) {
			if (Input.GetMouseButtonDown (MOUSE_RIGHT_BUTTON)) {
				container.processRClick (mouseX, mouseY);

			}

			if (Input.GetMouseButtonDown (MOUSE_WHEEL_BUTTON)) {
				container.processMClick (mouseX, mouseY);

			}

			//Обработка зажатия ЛКМ
			if (Input.GetMouseButtonUp (MOUSE_LEFT_BUTTON)) {
				clickTime = 0;

			}
				
			if (Input.GetMouseButton(MOUSE_LEFT_BUTTON))
				clickTime += Time.deltaTime;
			
			if (Input.GetMouseButton (MOUSE_LEFT_BUTTON) && !mouseLkHold && clickTime > 0.6) {
				mouseLkHold = true;
				container.processKeyHold (mouseX, mouseY);
				clickTime = 0;
			} else if (!Input.GetMouseButton (MOUSE_LEFT_BUTTON) && mouseLkHold) {
				container.processKeyRelease (mouseX, mouseY);
				mouseLkHold = false;
			}
		}

		if (Input.GetKeyDown ("i")) {
			if (inventoryIsActive) {
				canvas.enabled = false;
				inventoryIsActive = false;
				Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
			}
			else {
				canvas.enabled = true;
				inventoryIsActive = true;
			}
		}

		if (Input.GetKeyDown ("f")) {
			List<Item> items = new List<Item> ();
			for (int i = 0; i < 99; i++) {
				items.Add (new Axe ());
				items.Add (new Hammer ());
			}
			container.addItems (items, playerContainerName);
		}
	}
}
