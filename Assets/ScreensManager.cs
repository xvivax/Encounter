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
	private string result = string.Empty;
	private string url;


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

		var vilniusRadioButton = root.Q<RadioButton>("vilnius-radio-button");
		var demoRadioButton = root.Q<RadioButton>("demo-radio-button");

		vilniusRadioButton.SetSelected( true );

		root.Q<Button>( "login-button" ).clicked += () =>
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

			StartCoroutine( SendLoginRequest() );

			//ClearAllScreens();
			//GameScreenHandler();
		};

		root.Q<Button>( "email-button" ).clicked += EmailButtonClicked;
	}

	private void EmailButtonClicked()
	{
		StartCoroutine( SendEmailRequest() );
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

	private IEnumerator SendLoginRequest()
	{
		string path = "/Login.aspx";
		string newUrl = url + path;

		WWWForm form = new WWWForm();
		form.AddField( "Login", userName );
		form.AddField( "Password", password );

		var postRequest = UnityWebRequest.Post( newUrl, form);
		postRequest.redirectLimit = 1;
		postRequest.timeout = 1;
		yield return postRequest.SendWebRequest();


		if (postRequest.result != UnityWebRequest.Result.Success)
		{
			if (postRequest.result == UnityWebRequest.Result.InProgress)
			{
				result = "Failed! Request in progress";
			}
			else if (postRequest.result == UnityWebRequest.Result.ConnectionError)
			{
				if (postRequest.responseCode == 302) // 302 - Http redirection response code
				{
					result = "Connected?";
				}
				else
				{
					result = postRequest.error;
				}
			}
			else if (postRequest.result == UnityWebRequest.Result.ProtocolError)
			{
				result = "Failed! Protocol error";
			}
			else if (postRequest.result == UnityWebRequest.Result.DataProcessingError)
			{
				result = "Failed! Data processing error";
			}

			root.Q<Label>( "result-label" ).text = result;
		}
		else
		{
			root.Q<Label>( "result-label" ).text = postRequest.downloadHandler.text;
		}
	}

	private IEnumerator SendEmailRequest()
	{
		string path = "/EnMail.aspx";
		string newUrl = url + path;
		var getRequest = UnityWebRequest.Get( newUrl );

		yield return getRequest.SendWebRequest();

		if (getRequest.result != UnityWebRequest.Result.Success)
		{
			root.Q<Label>( "result-label" ).text = getRequest.error;
		}
		else
		{
			root.Q<Label>( "result-label" ).text = getRequest.downloadHandler.text;
		}
	}
}
