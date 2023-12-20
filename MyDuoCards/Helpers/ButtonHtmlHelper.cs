using Azure.Core;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using MyDuoCards.Models.DBModels;
//using System.Security.Policy;

namespace MyDuoCards.Helpers
{
	public static class ButtonHtmlHelper
	{
		public static HtmlString ButtonView(this IHtmlHelper htmlHelper, string uri, int pageOfLoop, int currentPage, string? searchString = "")
		{
			TagBuilder btn = new TagBuilder("a");
			btn.MergeAttribute("type", "button");
			btn.Attributes.Add("href", $"{uri}?searchString={searchString}&page={pageOfLoop}");
			btn.MergeAttribute("style", "margin-right: 5px");
			btn.InnerHtml.Append(pageOfLoop.ToString());

			if (currentPage == pageOfLoop) btn.AddCssClass("btn btn-warning disabled");
			else btn.AddCssClass("btn btn-outline-light");

			StringWriter sw = new StringWriter();
			btn.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);

			return new HtmlString(sw.ToString());

		}
	}
}
