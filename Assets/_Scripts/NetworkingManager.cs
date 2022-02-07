using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkingManager : MonoBehaviour
{
	private static string loginPath = "/login/signin/";

	public static event Action<string> Response;

	public static IEnumerator SendLoginRequest(string url, string userName, string password)
	{
		string newUrl = url + loginPath;

		WWWForm form = new WWWForm();
		form.AddField( "Login", userName );
		form.AddField( "Password", password );

		var postRequest = UnityWebRequest.Post( newUrl, form );
		postRequest.timeout = 5;
		yield return postRequest.SendWebRequest();

		if (postRequest.result != UnityWebRequest.Result.Success)
		{
			Response?.Invoke( postRequest.error );
		}
		else
		{
			Response?.Invoke( postRequest.downloadHandler.text );
		}
	}
}
