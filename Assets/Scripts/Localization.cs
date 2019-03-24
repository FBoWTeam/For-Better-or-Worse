using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LocaleDatabase
{
	string Translate(string key);
}

public static class I18n
{
	private static LocaleDatabase locale;

	public static void LoadLang(string key)
	{
		SimpleLocaleLoader loader = new SimpleLocaleLoader();
		loader.LoadDatabase(key);
		locale = loader.GetLoadedDatabase();
	}

	public static string Translate(string key)
	{
		return locale.Translate(key);
	}
}

public class SimpleLocaleLoader
{
	private LocaleDatabase loadedDatabase;

	public void LoadDatabase(string name)
	{
		loadedDatabase = new LocaleDatabaseImpl();
		Object[] langDatas = Resources.LoadAll("Lang/" + name, typeof(TextAsset));
		if (langDatas != null)
		{
			foreach (Object textAsset in langDatas)
			{
				if (textAsset is TextAsset)
				{
					string[] lines = ((TextAsset)textAsset).text.Split('\n');
					foreach (string line in lines)
					{
						string[] pair = line.Split(new char[] { '=' }, 2);
						if (pair.Length > 1)
						{
							pair[1] = pair[1].Replace("\r", string.Empty);
							((LocaleDatabaseImpl)loadedDatabase).PutValue(pair[0], pair[1]);
						}
					}
				}
			}
		}
	}

	public LocaleDatabase GetLoadedDatabase()
	{
		return loadedDatabase;
	}

	private class LocaleDatabaseImpl : LocaleDatabase
	{
		private Dictionary<string, string> langValues;

		public LocaleDatabaseImpl()
		{
			langValues = new Dictionary<string, string>();
		}

		public void PutValue(string key, string value)
		{
			langValues.Add(key, value);
		}

		public string Translate(string key)
		{
			string translated;
			if (langValues.TryGetValue(key, out translated))
			{
				return translated;
			}
			else
			{
				return key;
			}
		}
	}
}
