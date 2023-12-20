using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using MyDuoCards.Models.DBModels;
//using System.Security.Policy;

namespace MyDuoCards.Helpers
{
	public static class CardHtmlHelper
	{
		public static HtmlString CardView(this IHtmlHelper htmlHelper, EnWord enWord, string userLogin, Func<int, string> urlAdd, Func<int, string> urlRemove)
		{
			TagBuilder divCard = new TagBuilder("div");
			TagBuilder divCardBody = new TagBuilder("div");
			TagBuilder cardTitle = new TagBuilder("h5");
			TagBuilder cardText = new TagBuilder("p");
			TagBuilder btn = new TagBuilder("a");

			cardTitle.InnerHtml.Append(enWord.EnWriting);
			cardTitle.AddCssClass("card-title");

			cardText.InnerHtml.Append(enWord.RuWord?.RuWriting ?? "no translation yet");
			cardText.AddCssClass("card-text");

			btn.MergeAttribute("type", "button");
			btn.AddCssClass("btn");

			if (enWord.Dictionaries.Any(d => d.User.Login == userLogin))
			{
				//tag.AddCssClass("disabled");
				btn.InnerHtml.Append("Remove");
				btn.AddCssClass("btn-danger");
				btn.MergeAttribute("href", urlRemove(enWord.RuWord.Id));
			}
			else
			{
				btn.InnerHtml.Append("Add");
				btn.AddCssClass("btn btn-success");
				btn.MergeAttribute("href", urlAdd(enWord.RuWord.Id));
			}

			divCardBody.AddCssClass("card-body");
			divCardBody.InnerHtml.AppendHtml(cardTitle);
			divCardBody.InnerHtml.AppendHtml(cardText);
			divCardBody.InnerHtml.AppendHtml(btn);

			divCard.AddCssClass("card");
			divCard.MergeAttribute("style", "margin: 3px");
			divCard.InnerHtml.AppendHtml(divCardBody);

			StringWriter sw = new StringWriter();
			divCard.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);

			return new HtmlString(sw.ToString());
		}
	}
}
