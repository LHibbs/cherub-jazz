using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordController : MonoBehaviour {

	public GameObject word;
	private Transform canvas;
	public GameObject currentMSGObj;
	private CurrentMSG currentMSGScript;

	private List<List<string>> words = new List<List<string>>();
	void Start () {
		canvas = GameObject.Find("Canvas").transform;
		LoadWords();
		SpawnNextWords();

		currentMSGScript = currentMSGObj.GetComponent<CurrentMSG>();
	}

	void LoadWords() {
		
		//TEMP LIST
		for(int i = 0; i < 100; i++) {
			words.Add(new List<string>{});
			for(int j = 0; j < Random.Range(1, 10); j++) {
				words[i].Add("Line: " + i + " | Word: " + j);
			}
		}

		/*foreach(List<string> line in words) {
			foreach(string thisWord in line) {
				Debug.Log(thisWord);
			}
		}*/
	}

	void SpawnNextWords() {
		//Instantiate(GameObject, transform parent);
		//currentMSGScript.Msg    <--- List<string>

		/*
		foreach(line in words)
			foreach(word in Msg)
				if(! line[i] = word)
					STOP!
			passed that ^ ?
			spawn (line[one more than in Msg right now])
			^ max of 15
		 */

		 for(int i = 0; i < words.Count; i++) {
			 if(words[i].Count < currentMSGScript.Msg.Count) {
				 continue;
			 }
			 for(int j = 0; j < currentMSGScript.Msg.Count; j++) {
				 if()
			 }
		 }
	}

	void UpdateCurrentMSG() {

	}

	void DeleteWords() {

	}
}
