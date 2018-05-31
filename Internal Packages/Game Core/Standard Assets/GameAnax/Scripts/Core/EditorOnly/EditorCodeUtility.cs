using System;
using System.Reflection;

using UnityEngine;
using UnityEngineInternal;

using GameAnax.Core.Utility;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameAnax.Core.Editor {
	public static class EditorCodeUtility {
		// % (ctrl on Windows, cmd on macOS)
		// # (shift)
		// & (alt)
#if UNITY_EDITOR
		[MenuItem("Tools/GameAnax/DeleteMyPlayerPrefs &#d")]
		static void DeleteMyPrefs() {
			PlayerPrefs.DeleteAll();
		}

		[MenuItem("Tools/GameAnax/Clear Console &#c")]
		public static void ClearLogConsole() {
			var assembly = Assembly.GetAssembly(typeof(UnityEditor.SceneView));
			var type = assembly.GetType("UnityEditor.LogEntries");
			MyDebug.Log("is type: {0}", null == type);
			var method = type.GetMethod("Clear");
			method.Invoke(null, null);
		}
#endif
	}
}