using UnityEngine;
using System.Collections;

//скрипт создает главное меню игры. Меню будет сдвинуто в левую часть экрана. 
//В будущем: С правой части должны будут выводиться подпункты меню
//Само собой, ввести звук при нажатие клавиш тоже бы не помешало.

public class mainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//создаем GUI для меню
	void OnGUI ()
	{
		if(GUI.Button(new Rect(Screen.width/6-100, Screen.height/2-95, 200, 30), "Начать игру"))
		{
			Application.LoadLevel(1);
		}
		if(GUI.Button(new Rect(Screen.width/6-100, Screen.height/2-55, 200, 30), "Выйти из игры"))
		{
			Application.Quit();
		}
	}
}
