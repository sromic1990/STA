using System.Xml.Serialization;


[System.Serializable]
public class UndoData {
	public string LastBoard;
	[XmlIgnore]
	public int LastScore;


	public UndoData() {
		LastBoard = string.Empty;
		LastScore = 0;
	}
}
