using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TrieNode {
	private Dictionary<string, TrieNode> children;
	private string content;
    private bool end;

	public TrieNode(string content) {
		children = new Dictionary<string, TrieNode>();
		this.content = content;
		end = false;
	}

	public Dictionary<string, TrieNode> getChildren() {
		return children;
	}

	public string[] getChildWords() {
		return children.Keys.ToArray();
	}
	public string getContent() {
		return content;
	}

	public bool isEnd() {
		return end;
	}
	public void setEnd(bool newEnd) {
		end = newEnd;
	}
}
