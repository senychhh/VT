// using Microsoft.AspNetCore.Razor.TagHelpers;
// using Microsoft.AspNetCore.Routing;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using System;

// namespace Kolbasin_lab1.TagHelpers
// {
//     [HtmlTargetElement("pager")]
// public class PagerTagHelper : TagHelper
// {
//     private readonly LinkGenerator _linkGenerator;
//     private readonly IHttpContextAccessor _httpContextAccessor;

//     public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
//     {
//         _linkGenerator = linkGenerator;
//         _httpContextAccessor = httpContextAccessor;
//     }

//     [HtmlAttributeName("current-page")] public int CurrentPage { get; set; }
//     [HtmlAttributeName("total-pages")] public int TotalPages { get; set; }
//     [HtmlAttributeName("action")] public string Action { get; set; } = "Index";
//     [HtmlAttributeName("controller")] public string Controller { get; set; } = "Product";
//     [HtmlAttributeName("category")] public string? Category { get; set; }

//     private int Prev => CurrentPage == 1 ? 1 : CurrentPage - 1;
//     private int Next => CurrentPage == TotalPages ? TotalPages : CurrentPage + 1;

//     public override void Process(TagHelperContext context, TagHelperOutput output)
//     {
//         output.TagName = "nav";
//         var ul = new TagBuilder("ul");
//         ul.AddCssClass("pagination justify-content-center");

//         ul.InnerHtml.AppendHtml(CreateListItem("«", Prev, CurrentPage == 1));
//         for (int i = 1; i <= TotalPages; i++)
//             ul.InnerHtml.AppendHtml(CreateListItem(i.ToString(), i, false, i == CurrentPage));
//         ul.InnerHtml.AppendHtml(CreateListItem("»", Next, CurrentPage == TotalPages));

//         output.Content.SetHtmlContent(ul);
//     }

//     private TagBuilder CreateListItem(string text, int pageNo, bool disabled = false, bool active = false)
//     {
//         var li = new TagBuilder("li");
//         li.AddCssClass("page-item");
//         if (disabled) li.AddCssClass("disabled");
//         if (active) li.AddCssClass("active");

//         var a = new TagBuilder("a");
//         a.AddCssClass("page-link");

//         var url = _linkGenerator.GetPathByAction(Action, Controller, new { pageNo, category = Category });
//         a.Attributes.Add("href", disabled ? "#" : url);
//         a.InnerHtml.AppendHtml(text);

//         li.InnerHtml.AppendHtml(a);
//         return li;
//     }
// }

// }
