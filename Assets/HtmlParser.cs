using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

public class HtmlParser
{
	private HtmlDocument htmlDoc;

	public HtmlParser ParseToHtmlDocument(string html)
	{
		htmlDoc = new HtmlDocument();
		htmlDoc.LoadHtml( html );

		return this;
	}

	public List<Games> GetGamesList()
	{
		List<Games> tempGames = new List<Games>();

		var htmlContentClassNodes = htmlDoc.DocumentNode.Descendants()
		.Where( n => n.HasClass( "content" ) );

		if (htmlContentClassNodes != null)
		{
			foreach (var item in htmlContentClassNodes)
			{
				var Ahrefs = item.SelectNodes( "//h1/a" );

				if (Ahrefs == null)
				{
					tempGames.Add( new Games() { Name = "Missing h1 and A tags", Link = "null" } );
				}

				foreach (var item1 in Ahrefs)
				{
					tempGames.Add( new Games() { Name = item1.InnerText, Link = "kolkas nera" } );
				}
			}
		}
		else
		{
			tempGames.Add( new Games() { Name = "null", Link = "null" } );
		}

		return tempGames;
	}

	public string GetUserName()
	{
		var htmlContentClassNodes = htmlDoc.DocumentNode.Descendants()
			.Where( n => n.HasClass( "header" ) );

		if (htmlContentClassNodes != null)
		{
			foreach (var item in htmlContentClassNodes)
			{
				var user = item.SelectSingleNode( "//table/tr/td/a" ).Attributes["href"].Value == "/users/details/";

				if (user == false)
				{
					return "nera userio";
				}
				else
				{
					return item.SelectSingleNode( "//table/tr/td/a" ).InnerText;
				}
			}
		}
		else
		{
			return "No such class";
		}

		return "Neaiskus erroras";
	}
}
