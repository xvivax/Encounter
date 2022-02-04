using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class ScreensManager : MonoBehaviour
{
	[SerializeField]
	private UIDocument loginScreen;
	[SerializeField]
	private UIDocument gameScreen;

	private VisualElement root;
	private string userName;
	private string password;
	private string result;


	private void OnEnable()
	{
		LoadDefaultScreen();
	}

	private void LoadDefaultScreen()
	{
		ClearAllScreens();
		LoginScreenHandler();
	}

	private void LoginScreenHandler()
	{
		loginScreen.enabled = true;
		root = loginScreen.rootVisualElement;

		root.Q<Button>( "login-button" ).clicked += () =>
		{
			userName = root.Q<TextField>( "user-input-field" ).text;
			password = root.Q<TextField>( "password-input-field" ).text;

			//root.Q<Label>( "result-label" ).text = userName;

			StartCoroutine( SendRequest() );

			//ClearAllScreens();
			//GameScreenHandler();
		};

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


	private IEnumerator SendRequest()
	{
		string url = "http://www.demo.en.cx/Login.aspx";

		WWWForm form = new WWWForm();
		form.AddField( "Login", userName );
		form.AddField( "Password", password );

		var request = UnityWebRequest.Post( url, form);
		request.redirectLimit = 1;
		yield return request.SendWebRequest();

		root.Q<Label>( "result-label" ).text = request.downloadHandler.text;

	}
}
