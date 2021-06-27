using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Services.Implementation.NewsRating.JsonModels;
using Newtonsoft.Json;
using Serilog;

namespace NewsAggregator.Services.Implementation.NewsRating
{

    public class NewsRatingService : INewsRatingService
    {

        public async Task<IEnumerable<NewsDto>> Rating(IEnumerable<NewsDto> newsWithoutRating)
        {
            try
            {
                var newsWithRating = new List<NewsDto>();

                var afinn = await GetAfinnWords();

                foreach (var newsDto in newsWithoutRating)
                {
                    var body = newsDto.Body;

                    body = ClearBody(body);

                    var responseNews = await Response(body);

                    if (responseNews == null)
                    {
                        Log.Error($" Response news is unsucceeded {newsDto.Url}");
                        continue;
                    }

                    var rating = RatingNews(afinn, responseNews);

                    if (rating !=null)
                    {
                        newsDto.Rating = (float) rating;  
                    }
                    

                    newsWithRating.Add(newsDto);

                }

                return newsWithRating;
            }
            catch (Exception e)
            {
                Log.Error($"Error Rate News {e.Message}");
                return default;
            }
        }

        private float? RatingNews(Dictionary<string, int> afinn, IEnumerable<string> words)
        {
            var afin = afinn;
            try
            {
                var count = 0;
                var rate = 0;
                foreach (var word in words)
                {
                    foreach (var pair in afin.Where(pairs => pairs.Key.Equals(word)))
                    {
                        count ++;
                        rate += pair.Value;
                    }
                }

                if (count == 0)
                {
                    return null;
                }
                var result = (float) rate / count * 100;

                return result;
            }
            catch (Exception e)
            {
                Log.Error($"Error rating news return null {e}");
                return null;
            }


        }

        private async Task<Dictionary<string, int>> GetAfinnWords()
        {
            var pathAfinn =
                @"C:\Users\wladi\OneDrive\documents\GitHub\test\NewsAggregator\NewsAggregators.Services.Implementation\NewsRating\AFINNJson\AFINN-ru.json";

            var readAfinn = await File.ReadAllTextAsync(pathAfinn);

            var result = JsonConvert.DeserializeObject<Dictionary<string, int>>(readAfinn);

            return result;
        }

        private async Task<List<string>> Response(string body)
        {
            try
            {
                var httplient = new HttpClient();
                httplient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var url =
                    "http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey=c4964bda2afbf420b8d3fb1d4dc66dadcbaf91a9";

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent("[{\"text\":\"" + body + "\"}]",
                        Encoding.UTF8,
                        "application/json")
                };
                var response = await httplient.SendAsync(request);
                var resultResponse = await response.Content.ReadAsStringAsync();

                var listResponse = JsonConvert.DeserializeObject<ModelResponse[]>(resultResponse);

                if (listResponse == null)
                {
                    Log.Error($"Error NewsResponse return null {body} ");
                    return new List<string> { null };
                }
                var lemmasList = listResponse[0].annotations.lemma;

                var responseWord = lemmasList.Select(word => word.value).ToList();

                return responseWord;
            }
            catch (Exception e)
            {
                Log.Error($"This Body is invalid {e.Message}. {body}");
                return new List<string>() { null };
            }
        }

        private string ClearBody(string body)
        {
            var clearBody = @"<p>(.*?)<\/p>";
            var clearedBody = Regex.Matches(body, clearBody);
            var aggregate = clearedBody.Aggregate("", (current, n) => current + $"{n} ");
            if (string.IsNullOrEmpty(aggregate))
            {
                var clear = Regex.Replace(body, @"(<.*?>)|(http.*?\s)|(\n\t)", " ");
                char[] ch = { '.', '!', '?', '"', '\r' };
                var split = clear.Split(ch, StringSplitOptions.RemoveEmptyEntries);
                var agg = split.Aggregate("", (current, n) => current + $"{n.Trim()} ");
                return agg;
            }
            var clearMarkup = Regex.Replace(aggregate, @"(<.*?>)", " ");

            return clearMarkup;
        }
    }
}