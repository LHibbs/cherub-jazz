using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordController : MonoBehaviour {

	public TextAsset storyFile;
	private int maxWords = 3;
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

	void Update() {
		if(Input.GetButtonUp("Reset")) {
			ResetWords();
		}
	}

	void LoadWords() {
		string[] lines = storyFile.text.Split('\n');
		for (int i = 0; i < lines.Length; i++) {
			string[] thisLine = lines[i].Split(',');
			List<string> sendLine = new List<string>();
			foreach (string s in thisLine) {
				if(s == "") {
					break;
				}
				sendLine.Add(s);
			}
			words.Add(sendLine);
		}

	}

	void SpawnNextWords() {
		List<string> compatibleWords = new List<string>();
		for(int i = 0; i < words.Count; i++) {
			if(words[i].Count <= currentMSGScript.Msg.Count) {
				continue;
			}
			bool addThisWord = true;
			for(int j = 0; j < currentMSGScript.Msg.Count; j++) {
				if(words[i][j] != currentMSGScript.Msg[j]) {
					addThisWord = false;
					break;
				}
			}
			//If we get here:
			// 1. The length of this line is > currentMSG
			// 2. Every word in the current msg is equal to the coorosponding part of the line
			if(addThisWord) {
				compatibleWords.Add(words[i][currentMSGScript.Msg.Count]);
			}
		}
		//If compatibleWords is empty, exit early
		if(compatibleWords.Count == 0) {
			return;
		}
		//Take a sample of the words which work
		//Probability of being chosen = (number needed)/(number left)
		List<string> wordsToSpawn = new List<string>();
		for (int i = compatibleWords.Count - 1; i >= 0; i--) {
			//If wordsToSpawn has enough entries, then we're done here
			if (wordsToSpawn.Count >= maxWords) {
				break;
			}
			//If this word is already in wordsToSpawn, then skip it and go to the next word
			if(wordsToSpawn.Contains(compatibleWords[i])) {
				continue;
			}
			float rand = Random.Range(0f, 1f);
			//Decide if this word should go in the wordsToSpawn, randomly, 
			// with the probibilty of a word being chosen increasing over time
			// Every word has an equal chance og being chosen
			if(rand <= (maxWords * 1.0f - wordsToSpawn.Count) / (i+1.0f)) {
				wordsToSpawn.Add(compatibleWords[i]);
			}
		}

		//Debug line to list every word in wordsToSpawn
		foreach (string w in wordsToSpawn) {
			Transform newWord = Instantiate(word, canvas).transform;
			RectTransform rt = newWord.GetComponent<RectTransform>();
			rt.position = RandomPosition();
			Text newWordText = newWord.Find("Text").GetComponent<Text>();
			newWordText.text = w;
		}
	}

	public void WordClicked(string wordString) {
		currentMSGScript.AddWord(wordString);
		DeleteWords();
		SpawnNextWords();
	}

	Vector3 RandomPosition() {
		float xPos = Random.Range(-7f, 7f);
		float yPos = Random.Range(-2f, 2f);
		return new Vector3(xPos, yPos);
	}

	void DeleteWords() {
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Word")) {
			Destroy(g);
		}
	}

	void ResetWords() {
		currentMSGScript.EmptyMsg();
		DeleteWords();
		SpawnNextWords();
	}
}
