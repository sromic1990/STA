using System;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace vexe {
	/// <summary>
	/// Code Writen by https://answers.unity.com/users/146979/vexe.html
	/// code link: https://answers.unity.com/questions/956123/add-and-select-game-view-resolution.html
	/// </summary>
	public static class GameViewUtils {
#if UNITY_EDITOR
		static object gameViewSizesInstance;
		static MethodInfo getGroup;
		static GameViewSizeGroupType gvsgt = GameViewSizeGroupType.Standalone;

		static GameViewUtils() {
			var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
			var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
			var instanceProp = singleType.GetProperty("instance");
			getGroup = sizesType.GetMethod("GetGroup");
			gameViewSizesInstance = instanceProp.GetValue(null, null);
		}

		public enum GameViewSizeType {
			AspectRatio, FixedResolution
		}

		[MenuItem("Tools/vexe/GameView Size/AddSize")]
		public static void AddTestSize() {
			AddCustomSize(GameViewSizeType.AspectRatio, 123, 456, "Test size");
		}

		[MenuItem("Tools/vexe/GameView Size/Set Test size")]
		public static void CheckExitByName() {
			Debug.Log(SizeExistsByName("Test size"));
		}

		[MenuItem("Tools/vexe/GameView Size/Query16:9 Test")]
		public static void Check16x9LandSacpe() {
			Debug.Log(SizeExistsByValue(GameViewSizeType.AspectRatio, 16, 9));
		}

		[MenuItem("Tools/vexe/GameView Size/Set 16:9 Landscape")]
		public static void Set16x9Landscape() {
			SetSize(FindSizeByValue(GameViewSizeType.AspectRatio, 16, 9));
		}
		[MenuItem("Tools/vexe/GameView Size/Set 16:9 Portrait")]
		public static void Set16x9Portrait() {
			SetSize(FindSizeByValue(GameViewSizeType.AspectRatio, 9, 16));
		}
		[MenuItem("Tools/vexe/GameView Size/Set 39:18 Landscape")]
		public static void SetIPhoneXandscape() {
			SetSize(FindSizeByValue(GameViewSizeType.AspectRatio, 39, 18));
		}
		[MenuItem("Tools/vexe/GameView Size/Set 18:39 Portrait")]
		public static void SetIPhoneXPortrait() {
			SetSize(FindSizeByValue(GameViewSizeType.AspectRatio, 18, 39));
		}

		public static void SetSize(int index) {
			if(index < 0) return;
			//Debug.Log("new Size: " + index);
			var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
			var selectedSizeIndexProp = gvWndType.GetProperty("selectedSizeIndex",
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			var gvWnd = EditorWindow.GetWindow(gvWndType);
			selectedSizeIndexProp.SetValue(gvWnd, index, null);
		}
		public static void AddCustomSize(GameViewSizeType viewSizeType, int width, int height, string text) {
			// GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupTyge);
			// group.AddCustomSize(new GameViewSize(viewSizeType, width, height, text);

			GetViewSizeGroup();
			var group = GetGroup(gvsgt);
			var addCustomSize = getGroup.ReturnType.GetMethod("AddCustomSize"); // or group.GetType().
			var gvsType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
			var ctor = gvsType.GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(string) });
			var newSize = ctor.Invoke(new object[] { (int)viewSizeType, width, height, text });
			addCustomSize.Invoke(group, new object[] { newSize });
		}

		public static bool SizeExistsByName(string text) {
			return FindSizeByName(text) != -1;
		}
		public static bool SizeExistsByValue(GameViewSizeType viewType, int width, int height) {
			return FindSizeByValue(viewType, width, height) != -1;
		}

		private static void GetViewSizeGroup() {
			gvsgt = GetCurrentGroupType();
		}

		[MenuItem("Tools/vexe/GameView Size/Log Current Group Type")]
		public static void LogCurrentGroupType() {
			Debug.Log(GetCurrentGroupType());
		}
		public static GameViewSizeGroupType GetCurrentGroupType() {
			var getCurrentGroupTypeProp = gameViewSizesInstance.GetType().GetProperty("currentGroupType");
			return (GameViewSizeGroupType)(int)getCurrentGroupTypeProp.GetValue(gameViewSizesInstance, null);

		}

		public static int FindSizeByValue(GameViewSizeType viewType, int width, int height) {
			// GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupType);
			// string[] texts = group.GetDisplayTexts();
			// for loop...
			string text = width + (viewType.Equals(GameViewSizeType.AspectRatio) ? ":" : "x") + height;
			GetViewSizeGroup();
			var group = GetGroup(gvsgt);
			var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
			var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
			for(int i = 0; i < displayTexts.Length; i++) {
				string display = displayTexts[i];
				string aspct = "";
				// the text we get is "Name (W:H)" if the size has a name, or just "W:H" e.g. 16:9
				// so if we're querying a custom size text we substring to only get the name
				// You could see the outputs by just logging
				// Debug.Log(display);
				int pren = display.LastIndexOf('(');
				int x = display.Length - pren + 1;
				if(pren != -1) {
					aspct = display.Substring(pren + 1);
					pren = aspct.IndexOf(')');
					if(pren != -1) {
						aspct = aspct.Substring(0, pren);
					}
				}
				//MyDebug.Log("{0} - {1} - {2} - {3}", display, pren, text, aspct);
				if(aspct == text)
					return i;
			}
			return -1;
		}
		public static int FindSizeByName(string text) {
			// GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupType);
			// string[] texts = group.GetDisplayTexts();
			// for loop...

			GetViewSizeGroup();
			var group = GetGroup(gvsgt);
			var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
			var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
			for(int i = 0; i < displayTexts.Length; i++) {
				string display = displayTexts[i];
				string aspct = "";
				// the text we get is "Name (W:H)" if the size has a name, or just "W:H" e.g. 16:9
				// so if we're querying a custom size text we substring to only get the name
				// You could see the outputs by just logging
				// Debug.Log(display);
				int pren = display.LastIndexOf('(');
				if(pren != -1)
					aspct = display.Substring(0, pren - 1); // -1 to remove the space that's before the prens. This is very implementation-depdenent
				if(aspct == text)
					return i;
			}
			return -1;
		}

		public static bool SizeExists(GameViewSizeGroupType sizeGroupType, int width, int height) {
			return FindSize(sizeGroupType, width, height) != -1;
		}
		public static int FindSize(GameViewSizeGroupType sizeGroupType, int width, int height) {
			// goal:
			// GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupType);
			// int sizesCount = group.GetBuiltinCount() + group.GetCustomCount();
			// iterate through the sizes via group.GetGameViewSize(int index)

			var group = GetGroup(sizeGroupType);
			var groupType = group.GetType();
			var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
			var getCustomCount = groupType.GetMethod("GetCustomCount");
			int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
			var getGameViewSize = groupType.GetMethod("GetGameViewSize");
			var gvsType = getGameViewSize.ReturnType;
			var widthProp = gvsType.GetProperty("width");
			var heightProp = gvsType.GetProperty("height");
			var indexValue = new object[1];
			for(int i = 0; i < sizesCount; i++) {
				indexValue[0] = i;
				var size = getGameViewSize.Invoke(group, indexValue);
				int sizeWidth = (int)widthProp.GetValue(size, null);
				int sizeHeight = (int)heightProp.GetValue(size, null);
				if(sizeWidth == width && sizeHeight == height)
					return i;
			}
			return -1;
		}

		static object GetGroup(GameViewSizeGroupType type) {
			return getGroup.Invoke(gameViewSizesInstance, new object[] { (int)type });
		}
#endif
	}
}