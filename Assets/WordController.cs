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
		currentMSGScript = currentMSGObj.GetComponent<CurrentMSG>();
		LoadWords();
		SpawnNextWords();
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

		List<string> wordsToSpawn = new List<string>();
		//Debug.Log("Words is: " + words.Count);
		for(int i = 0; i < words.Count; i++) {
			if(words[i].Count <= currentMSGScript.Msg.Count) {
				//Debug.Log("Exited at line " + i + " because line is too short");
				continue;
			}
			//Debug.Log("Passed line length test");
			bool addThisWord = true;
			for(int j = 0; j < currentMSGScript.Msg.Count; j++) {
				if(words[i][j] != currentMSGScript.Msg[j]) {
					//Debug.Log("Exited at line " + i + " word " + j + " because word does not match");
					addThisWord = false;
					break;
				}
			}
			//Debug.Log("Passed word match test");
			//If we get here:
			// 1. The length of this line is > currentMSG
			// 2. Every word in the current msg is equal to the coorosponding part of the line
			if(addThisWord) {
				wordsToSpawn.Add(words[i][currentMSGScript.Msg.Count]);
			}
			//Debug.Log("Added word to list: " + words[i][currentMSGScript.Msg.Count]);
		}

		 foreach (string w in wordsToSpawn) {
			 Debug.Log("Word in spawn list: " + w);
		 }
	}

	void UpdateCurrentMSG() {

	}

	void DeleteWords() {

	}
}
