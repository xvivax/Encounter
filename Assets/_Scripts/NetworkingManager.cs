using System;
using System.Collections;
using System.Collections.Generic;
using HtmlAgilityPack;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkingManager
{
	private static string loginPath = "/login/signin/";

	public static event Action<HtmlParser> Response;

	private static HtmlParser parser = new HtmlParser();

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
			Response?.Invoke( parser.ParseToHtmlDocument( postRequest.error )  );
		}
		else
		{
			Response?.Invoke( parser.ParseToHtmlDocument( postRequest.downloadHandler.text ) );
		}
	}

	public static IEnumerator GetRequest(string url, Action<HtmlParser> Callback)
	{
		var getRequest = UnityWebRequest.Get( url );
		yield return getRequest.SendWebRequest();
		
		if (getRequest.result != UnityWebRequest.Result.Success)
		{
			//Callback( getRequest.error );
			Callback( parser.ParseToHtmlDocument( getRequest.error ) );
		}
		else
		{
			//Callback( getRequest.downloadHandler.text );
			Callback( parser.ParseToHtmlDocument( getRequest.downloadHandler.text) );
		}
	}
}
