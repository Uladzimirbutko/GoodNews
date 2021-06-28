using System;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsAggregator.Models;

namespace NewsAggregator.HtmlHelpers
{
    public static class PaginationHelper
    {
        public static HtmlString CreatePagination(this IHtmlHelper html,
            PageInfo pageInfo,
            Func<int, string> pageUrl)
        {
            var sb = new StringBuilder();
            var count = pageInfo.PageNumber;

            if (pageInfo.PageNumber >= 6)
            {
                var strPrevious = $"<li class=\"page-item\"><a class=\"page-link\" href=\"{pageUrl(1)}\">Previous</a></li>";
                sb.Append(strPrevious);
            }

            var page = pageInfo.PageNumber;

            if (pageInfo.PageNumber>=6)
            {
                page -= 5;
            }

            for (int i = page; i <= pageInfo.TotalPages; i++)
            {
                
                
                if (i != pageInfo.PageNumber)
                {
                    var str = $"<li class=\"page-item\">" +
                              $"<a class=\"page-link\" href=\"{pageUrl(i)}\"> {i}" +
                              $"</a>" +
                              $"</li>";
                    sb.Append(str);
                }

                if (i == pageInfo.PageNumber)
                {
                   var strActive = $"<li class=\"page-item active\" aria-current=\"page\"> " +
                          $"<span class=\"page-link\">{i}</span>" +
                          $"</li>";
                   sb.Append(strActive);
                   count = i;
                }

                if (i >= count+5)
                {
                    var strNext = $"<li class=\"page-item\"><a class=\"page-link\" href=\"{pageUrl(i+1)}\">Next</a></li>";
                    sb.Append(strNext);
                    break;
                }
            }
            return new HtmlString(sb.ToString());
        }
    }
}