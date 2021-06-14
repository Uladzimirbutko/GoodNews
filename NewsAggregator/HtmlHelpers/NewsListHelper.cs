using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Models;

namespace NewsAggregator.HtmlHelpers
{
    public static class NewsListHelper
    {
        public static HtmlString CreateListNews(this IHtmlHelper html,
            IEnumerable<NewsDto> news)
        {
            var sb = new StringBuilder();
            foreach (var newsDto in news)
            {
                //todo logic html helpers
            }
            return new HtmlString(sb.ToString());
        }
    }
}