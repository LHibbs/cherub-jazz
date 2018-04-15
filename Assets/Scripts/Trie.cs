using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class Trie {
	private TrieNode root;

	public Trie() {
		root = new TrieNode("");
	}

	public TrieNode getRoot() {
		return root;
	}

	//Assumes no extra spaces at the end of each line
	public void insert(string sentence) {
		insert(sentence.Split(' '));
	}

	public void insert(string[] sentence) {
		TrieNode curNode = root;
		for( int i = 0; i < sentence.Length; i++) {
			string curWord = sentence[i];
			TrieNode nextNode;
			if(curNode.getChildren().ContainsKey(curWord)) {
				nextNode = curNode.getChildren()[curWord];
			} else {
				nextNode = new TrieNode(curWord);
				curNode.getChildren()[curWord] = nextNode;
			}
			if( i == sentence.Length - 1) {
				nextNode.setEnd(true);
			}
			curNode = nextNode;
		}
	}

	public bool contains(string sentence) {
		return contains(sentence.Split(' '));
	}

	public bool contains(string[] sentence){
		TrieNode curNode = root;
		for(int i = 0; i < sentence.Length; i++) {
			string curWord = sentence[i];
			if(!curNode.getChildren().ContainsKey(curWord)) {
				return false;
			} else {
				curNode = curNode.getChildren()[curWord];
			}
			if(i == sentence.Length - 1) {
				if(curNode.isEnd()) {
					return true;
				} else {
					return false;
				}
			}
		}
		return false;
	}

	public void delete(string sentence){
		delete(sentence.Split(' '));
	}

	public void delete(string[] sentence){
		delete(root, sentence, 0);
	}

	private bool delete(TrieNode curNode, string[] sentence, int index) {
		if(index == sentence.Length) {
			if(!curNode.isEnd()) {
				return false;
			} else {
				curNode.setEnd(false);
				return (curNode.getChildren().Count == 0);
			}
		}
		string wordAt = sentence[index];
		if(!curNode.getChildren().ContainsKey(wordAt)) {
			return false;
		}
		TrieNode nextNode = curNode.getChildren()[wordAt];
		bool shouldDeleteCurNode = delete(nextNode, sentence, index + 1);
		if(shouldDeleteCurNode) {
			curNode.getChildren().Remove(wordAt);
			return (curNode.getChildren().Count == 0);
		}
		return false;
	}

	public string[] getNextWords(TrieNode curNode) {
		return curNode.getChildren().Keys.ToArray();
	}

}
