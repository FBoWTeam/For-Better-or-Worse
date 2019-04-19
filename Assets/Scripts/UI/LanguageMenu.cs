﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageMenu : MonoBehaviour
{
	public void SetLanguage(bool french)
	{
		if (french)
			I18n.LoadLang("fr_FR");
		else
			I18n.LoadLang("en_US");
		
		SceneManager.LoadScene(2);
	}
}