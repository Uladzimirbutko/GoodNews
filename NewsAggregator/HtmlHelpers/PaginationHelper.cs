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
            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                var str = $"<li class=\"page - item active\">" +
                                $"<a  class=\"page-link\" href={pageUrl(i)}> {i.ToString()}" +
                                $"</a>" +
                                $"</li>";

                if (i == pageInfo.PageNumber)
                {
                    str = $"<li class=\"page - item active\"> " +
                          $"<a  class=\"page-link\" href={pageUrl(i)}> " +
                          $"{i.ToString()}" +
                          $"</a>" +
                          $"</li>";
                }

                sb.Append(str);

            }
            return new HtmlString(sb.ToString());
        }
    }
}