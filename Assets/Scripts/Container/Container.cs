using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container: Transform {

	public List<GameObject> containerCells { get; }


	public Container (int cellCount, int columnCount, string containerTag, GameObject cellPrefab) {

		containerCells = new List<GameObject> ();
		GameObject panel = GameObject.FindGameObjectWithTag (containerTag);
		RectTransform cellRectTransform = cellPrefab.GetComponent<RectTransform> ();
		RectTransform containerRectTransform = GameObject.FindGameObjectWithTag (containerTag).GetComponent<RectTransform>();

		float width = containerRectTransform.rect.width / columnCount;
		float ratio = width / cellRectTransform.rect.width;
		float height = cellRectTransform.rect.height * ratio;
		int rowCount = cellCount / columnCount;
		if (cellCount % rowCount > 0)
			rowCount++;

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
