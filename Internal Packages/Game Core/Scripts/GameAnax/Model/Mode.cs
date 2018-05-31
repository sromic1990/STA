using System.Xml.Serialization;
using System.Collections.Generic;

using UnityEngine;

using GameAnax.Core.Interfaces;

using GameAnax.Game.CommonSystem;
using GameAnax.Game.Leaderboard;

[System.Serializable]
public class Mode : ICopy<Mode> {
	[XmlAttribute("ModeName", typeof(string))]
	public GameModes ModeName;
	#region Field to save in File But not dispaly in Editor"
	[HideInInspector]
	public int playCount;
	[HideInInspector]
	public bool isTutorial;

	[HideInInspector]
	public int score;
	[HideInInspector]
	public int bestScore;
	//
	[HideInInspector]
	public string boardData;

	[HideInInspector]
	public List<UndoData> undos;
	//
	[HideInInspector]
	public float playTime;
	//[HideInInspector]
	//public int lastRemoveTileCost;
	//[HideInInspector]
	//public int lastSwitchTileCost;
	//[HideInInspector]
	//public int lastUndoCost;
	[HideInInspector]
	public int coinAwarded;

	//[HideInInspector]
	//public int matchWon;
	//[HideInInspector]
	//public int matchLost;
	//[HideInInspector]
	//public int contiWon;
	//[HideInInspector]
	//public int contiLost;
	//
	#endregion

	#region Field to Show in Editor, but do not save in player progress fi";e
	[XmlIgnore]
	public bool isModeActive;
	[XmlIgnore]
	public bool isMultiplayer;
	//[Space(10)]
	//[Header("Power Related Fields")]
	//[XmlIgnore]
	//public string switchTileDescription;
	//[XmlIgnore]
	//public int switchTilePowerCost;
	//
	//[Space(5)]
	//[XmlIgnore]
	//public int removeTileCount;
	//[XmlIgnore]
	//public int removeTilePowerCost;
	//[XmlIgnore]
	//public List<int> removeableTiles;
	//[XmlIgnore]
	//public List<int> removeableTilesPower;
	//
	//[Space(5)]
	//[XmlIgnore]
	//public int undoMovePowerCost;
	//[XmlIgnore]
	//public int maxUndoStep;
	//

	[Space(10)]
	[Header("Gameover & Bonus on Score")]
	[XmlIgnore]
	public bool isOneMoreTimeVideo;
	[XmlIgnore]
	public int oneMoreTimerSecs;
	[Space(10)]
	[XmlIgnore]
	public int coinBonusOnGOScore;
	[XmlIgnore]
	public int coinBonusAtEveryScore;
	[XmlIgnore]
	public int maxUndoStep;
	[XmlIgnore]
	public List<Level> levels;



	[Space(10)]
	[Header("Leaderboard Related Fields")]
	[XmlIgnore]
	public List<Scoreboard> leaderboards;
	[XmlIgnore]
	public List<Achievement> achievements;
	#endregion

	public Mode() {
		SetDefault();
	}
	public Mode(GameModes thisMode) : this() {
		ModeName = thisMode;
	}
	public Mode Copy() {
		Mode m = new Mode();
		m.ModeName = ModeName;
		m.levels = this.levels;
		m.achievements = new List<Achievement>();
		m.achievements.AddRange(this.achievements.ToArray());
		return m;
	}
	public void SetDefault() {
		isTutorial = false;
		score = 0;
		bestScore = 0;
		playTime = 0;
		//
		coinAwarded = 1;
		undos = new List<UndoData>();
		levels = new List<Level>();
		achievements = new List<Achievement>();
	}
}
[System.Serializable]
public class ModeButtonVisual {
	public Color bgColor;
	public string modeName;
}
[System.Serializable]
public class ModeButtonObjs {
	public SpriteRenderer bg;
	public TextMesh text;
}