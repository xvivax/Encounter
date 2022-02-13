using System;
using System.Collections;
using System.Collections.Generic;
using HtmlAgilityPack;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class ScreensManager : MonoBehaviour
{
	[SerializeField]
	private UIDocument loginScreen;
	[SerializeField]
	private UIDocument gameScreen;
	[SerializeField]
	private UserPreferencesSO userPreference;

	private VisualElement root;
	private string userName;
	private string password;
	private string url;

	private RadioButton vilniusRadioButton;
	private RadioButton demoRadioButton;


	private void OnEnable()
	{
		NetworkingManager.Response += ServerResponse;
		LoadDefaultScreen();
	}

	private void ServerResponse( HtmlParser response )
	{
		var userName = response.GetUserName();

		List<Games> games = response.GetGamesList();

		string temp = string.Empty;
		foreach (var item in games)
		{
			temp += item.Name.ToString() + "\n ";
		}

		root.Q<Label>( "result-label" ).text = userName + "\n" + temp;
	}

	private void LoadDefaultScreen()
	{
		ClearAllScreens();

		StartCoroutine( NetworkingManager.GetRequest( "m.vilnius.en.cx", GetResponse));


		LoginScreenHandler();
	}

	private void GetResponse(HtmlParser response)
	{
		string userName = response.GetUserName();

		root.Q<Label>( "result-label" ).text = userName;
	}

	private void LoginScreenHandler()
	{
		loginScreen.enabled = true;
		root = loginScreen.rootVisualElement;

		vilniusRadioButton = root.Q<RadioButton>("vilnius-radio-button");
		demoRadioButton = root.Q<RadioButton>("demo-radio-button");

		vilniusRadioButton.SetSelected( true );
		url = vilniusRadioButton.label;
		
		userPreference.URL = url;

		root.Q<Button>( "login-button" ).clicked += LoginButtonClicked;
		root.Q<Button>( "reset-cookies-button" ).clicked += ResetButtonClicked;
	}

	private void LoginButtonClicked()
	{
		userName = root.Q<TextField>( "user-input-field" ).text;
		password = root.Q<TextField>( "password-input-field" ).text;

		if (vilniusRadioButton.value)
		{
			url = vilniusRadioButton.label;
		}
		else if (demoRadioButton.value)
		{
			url = demoRadioButton.label;
		}

		url = "m." + url;
		userPreference.URL = url;

		StartCoroutine( NetworkingManager.SendLoginRequest(url, userName, password) );

	}

	private void ResetButtonClicked()
	{
		UnityWebRequest.ClearCookieCache();
	}

	private void GameScreenHandler()
	{
		gameScreen.enabled = true;
		var root = gameScreen.rootVisualElement;

		root.Q<Button>( "back-button" ).clicked += () =>
		{
			ClearAllScreens();
			LoginScreenHandler();
		};
	}

	private void ClearAllScreens()
	{
		loginScreen.enabled = false;
		gameScreen.enabled = false;
	}

}
