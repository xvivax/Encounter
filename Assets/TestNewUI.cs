using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestNewUI : MonoBehaviour
{
	private VisualElement root;
	private VisualTreeAsset visualTree;

	private void OnEnable()
	{
		root = GetComponent<UIDocument>().rootVisualElement;

		var button = root.Q<Button>("login-button");
		button.clicked += GoToGameUI;

	}

	private void GoToGameUI()
	{
		
	}
}
