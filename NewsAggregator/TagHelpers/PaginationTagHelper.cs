using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NewsAggregator.Models;

namespace NewsAggregator.TagHelpers
{
    public class PaginationTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public PageInfo PageModel { get; set; }
        public string PageAction { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public override void  Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var result = new TagBuilder("div");

            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                var tag = new TagBuilder("a");
                var anchorInnerHtml = i.ToString();
                tag.Attributes["href"] = urlHelper.Action(PageAction, new {page = i});
                tag.InnerHtml.Append(anchorInnerHtml);
                result.InnerHtml.AppendHtml(tag);
            }

            output.Content.AppendHtml(result.InnerHtml);
        }

        //public string GetAnchorInnerHtml(int i, PageInfo info)
        //{
        //    var anchorInnerHtml = "";
        //    if (info.TotalPages<=10)
        //    {
        //        anchorInnerHtml = i.ToString();
        //    }
        //    else
        //    {
        //        if ((i-info.PageNumber >= 2 || info.PageNumber-i >=2) && i !=0 && i !=info.TotalPages)
        //        {
        //            anchorInnerHtml = "...";
        //        }
        //        else
        //        {
        //            anchorInnerHtml = i.ToString();
        //        }
        //    }
        //}
    }
}