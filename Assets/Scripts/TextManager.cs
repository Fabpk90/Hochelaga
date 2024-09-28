using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class TextManager
{
	public static string csvText = "Textes";
	private static Dictionary<string, string> textsList = new();
	private static bool load = false;

	public static void LoadCSV()
	{
		if (textsList.Count > 0) textsList.Clear();

		TextAsset textFile = Resources.Load<TextAsset>(csvText);
		using StringReader reader = new StringReader(textFile.text);
		reader.ReadLine();

		while (true)
		{
			string line = reader.ReadLine();

			if (line == string.Empty || line == null)
			{
				break;
			}

			string[] valuesLine = line.Split(';');
			if (!textsList.ContainsKey(valuesLine[0]))
				textsList.Add(valuesLine[0], valuesLine[1]);
		}
		load = true;
	}

	public static string GetTextByID(string id)
	{
		if (!load || !textsList.ContainsKey(id)) return null;
		return textsList[id];
	}
}
