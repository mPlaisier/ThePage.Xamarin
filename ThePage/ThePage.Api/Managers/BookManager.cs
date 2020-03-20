using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ThePage.Api
{
    public class BookManager
    {
        #region FETCH

        public static async Task<List<Book>> FetchBooks(CancellationToken cancellationToken)
        {
            try
            {
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Get, Constants.ThePageAPI_Url + EndPoints.GetBooks))
                using (var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode == false)
                    {
                        throw new ApiException
                        {
                            StatusCode = (int)response.StatusCode,
                            Content = content
                        };
                    }
                    return JsonConvert.DeserializeObject<List<Book>>(content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<Book> FetchBook(string id, CancellationToken cancellationToken)
        {
            try
            {
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Get, $"{Constants.ThePageAPI_Url}{EndPoints.GetBook}{id}"))
                using (var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode == false) //TODO check if needed in other method
                    {
                        throw new ApiException
                        {
                            StatusCode = (int)response.StatusCode,
                            Content = content
                        };
                    }
                    return JsonConvert.DeserializeObject<Book>(content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ADD

        public static async Task<Book> AddBook(Book content, CancellationToken cancellationToken)
        {
            try
            {
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, Constants.ThePageAPI_Url + EndPoints.GetBooks))
                {
                    var json = JsonConvert.SerializeObject(content);
                    using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;

                        using (var response = await client
                            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                            .ConfigureAwait(false))
                        {
                            response.EnsureSuccessStatusCode();
                            var newItem = await response.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<Book>(newItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PATCH

        public static async Task<Book> UpdateBook(Book content, CancellationToken cancellationToken)
        {
            try
            {
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Patch, $"{Constants.ThePageAPI_Url}{EndPoints.PatchBook}{content.Id}"))
                {
                    var json = JsonConvert.SerializeObject(content);
                    using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;

                        using (var response = await client
                            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                            .ConfigureAwait(false))
                        {
                            response.EnsureSuccessStatusCode();
                            var newItem = await response.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<Book>(newItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DELETE

        public static async Task<bool> DeleteBook(Book content, CancellationToken cancellationToken)
        {
            try
            {
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Delete, $"{Constants.ThePageAPI_Url}{EndPoints.DeleteBook}{content.Id}"))
                {
                    using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                        .ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
