using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour {

	const int MOUSE_LEFT_BUTTON = 0;
	const int MOUSE_RIGHT_BUTTON = 1;
	const int MOUSE_WHEEL_BUTTON = 2;

	public string playerInventoryTag;
	public string playerTag;

	private bool inventoryIsActive;
	private Canvas canvas;
	private bool mouseLkHold; //Флаг зажатия ЛКМ
	private float clickTime= 0f;
	private Container playerInventory;
	private Container storageContainer;
	private Player player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag (playerTag).GetComponent<Player> ();
		canvas = GetComponent<Canvas> ();
		canvas.enabled = false;
		inventoryIsActive = false;
		playerInventory = player.playerInventory;
		//container = new Container (containerCellCount, inventoryColumnCount, containerTag, containerCellPrefab);
	}
	
	// Update is called once per frame
	//TODO: Процессить клик по обьекту, только если мышь находится в области этого обьекта
	void Update () {

		float mouseX = Input.mousePosition.x;
		float mouseY = Input.mousePosition.y;

		clickTime += Time.deltaTime;

		if (inventoryIsActive) {
			if (Input.GetMouseButtonDown (MOUSE_RIGHT_BUTTON)) {
				Container.processRClick (mouseX, mouseY, playerInventory.containerCells);

			}

			if (Input.GetMouseButtonDown (MOUSE_WHEEL_BUTTON)) {
				Container.processMClick (mouseX, mouseY, playerInventory.containerCells);

			}

			//Обработка зажатия ЛКМ
			if (Input.GetMouseButtonUp (MOUSE_LEFT_BUTTON)) {
				clickTime = 0;

			}
				
			if (Input.GetMouseButton(MOUSE_LEFT_BUTTON))
				clickTime += Time.deltaTime;
			
			if (Input.GetMouseButton (MOUSE_LEFT_BUTTON) && !mouseLkHold && clickTime > 0.6) {
				mouseLkHold = true;
				Container.processKeyHold (mouseX, mouseY, playerInventory.containerCells);
				clickTime = 0;
			} else if (!Input.GetMouseButton (MOUSE_LEFT_BUTTON) && mouseLkHold) {
				Container.processKeyRelease (mouseX, mouseY, playerInventory.containerCells);
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
}
