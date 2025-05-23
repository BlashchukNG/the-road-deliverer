using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	public class PlayerPrefsEditorWindow : EditorWindow
	{
		private string key = "";
		private string stringValue = "";
		private float floatValue = 0f;
		private int intValue = 0;
		private Vector2 scrollPosition;

		[MenuItem("Window/PlayerPrefs Editor")]
		public static void ShowWindow()
		{
			GetWindow<PlayerPrefsEditorWindow>("PlayerPrefs Editor");
		}

		private void OnGUI()
		{
			GUILayout.Label("Работа с PlayerPrefs", EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal();
			key = EditorGUILayout.TextField("Ключ", key);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			stringValue = EditorGUILayout.TextField("Строковое значение", stringValue);
			if (GUILayout.Button("Set String"))
			{
				PlayerPrefs.SetString(key, stringValue);
				PlayerPrefs.Save();
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			floatValue = EditorGUILayout.FloatField("Число с плавающей точкой", floatValue);
			if (GUILayout.Button("Set Float"))
			{
				PlayerPrefs.SetFloat(key, floatValue);
				PlayerPrefs.Save();
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			intValue = EditorGUILayout.IntField("Целое число", intValue);
			if (GUILayout.Button("Set Int"))
			{
				PlayerPrefs.SetInt(key, intValue);
				PlayerPrefs.Save();
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			if (GUILayout.Button("Показать все ключи"))
			{
				Repaint();
			}

			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

			var unityPlayerPreferences = typeof(PlayerPrefs).GetNestedType("UnityPlayerPrefs", BindingFlags.NonPublic);
			var method = unityPlayerPreferences?.GetMethod("GetKeys", BindingFlags.Public | BindingFlags.Static);

			if (method != null)
			{
				var keys = method.Invoke(null, null) as string[];

				foreach (string currentKey in keys)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(currentKey, GUILayout.Width(150));

					if (PlayerPrefs.HasKey(currentKey))
					{
						string type = "unknown";
						if (float.TryParse(PlayerPrefs.GetString(currentKey), out float fValue))
						{
							type = "Float: " + fValue;
						}
						else if (int.TryParse(PlayerPrefs.GetString(currentKey), out int iValue))
						{
							type = "Int: " + iValue;
						}
						else
						{
							type = "String: " + PlayerPrefs.GetString(currentKey);
						}

						EditorGUILayout.LabelField(type);
					}

					if (GUILayout.Button("Удалить", GUILayout.Width(70)))
					{
						PlayerPrefs.DeleteKey(currentKey);
						PlayerPrefs.Save();
						Repaint();
					}

					EditorGUILayout.EndHorizontal();
				}
			}

			EditorGUILayout.EndScrollView();
		}
	}
}