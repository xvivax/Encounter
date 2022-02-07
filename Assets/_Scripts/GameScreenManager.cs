using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameScreenManager : MonoBehaviour
{
	[SerializeField]
	private UserPreferencesSO userPreference;

	private string url = string.Empty;
	private void OnEnable()
	{
		if (userPreference.URL == string.Empty)
		{
			print( "String empty" );
			return;
		}
		url = userPreference.URL;
		print( userPreference.URL );
		StartCoroutine( GetDataForMainPage() );
	}

	private IEnumerator GetDataForMainPage()
	{
		var request = UnityWebRequest.Get( url );
		yield return request.SendWebRequest();
	}
}
