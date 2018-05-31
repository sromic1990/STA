using System.Collections.Generic;
using System.Xml.Serialization;

using UnityEngine;

using GameAnax.Core.Interfaces;


[System.Serializable]
public class Level : ICopy<Level> {
	#region Field to save in File But not dispaly in Editor"
	public int levelNumber;
	[HideInInspector]
	public int score;
	[HideInInspector]
	public int bestScore;
	[HideInInspector]
	public int star;

	[HideInInspector]
	public float playTime;

	[HideInInspector]
	public string boardData;
	[HideInInspector]
	public List<UndoData> undos;

	#endregion

	#region Field to Show in Editor, but do not save in player progress file
	[Header("Core Data")]
	[XmlIgnore]
	public LevelType type;
	[XmlIgnore]
	public int moveOrSeconds;
	[XmlIgnore]
	public List<int> starValue;
	#endregion

	public Level() {
		SetDefault();
	}
	public Level Copy() {
		Level m = new Level();
		m.levelNumber = this.levelNumber;
		m.type = this.type;
		m.moveOrSeconds = this.moveOrSeconds;
		m.score = this.score;
		m.bestScore = this.bestScore;
		m.star = this.star;
		m.starValue = new List<int>();
		m.starValue.AddRange(this.starValue.ToArray());
		return m;
	}
	public void SetDefault() {
		this.score = 0;
		this.bestScore = 0;
		this.star = 0;
		this.undos = new List<UndoData>();
		this.starValue = new List<int>();
	}
}
public enum LevelType {
	Move,
	Timer
}
