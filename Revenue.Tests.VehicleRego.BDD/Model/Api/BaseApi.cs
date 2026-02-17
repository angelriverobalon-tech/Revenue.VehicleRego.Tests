using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;

namespace Revenue.Tests.BDD.Model.Api
{
    public abstract class BaseApi<T> : IDisposable where T : BaseApi<T>
    {
        protected readonly RestClient _client;
        protected RestRequest? _request;
        protected RestResponse? _response;
        private readonly Dictionary<string, string> _defaultHeaders = new();
        private string? _accept;

        protected BaseApi()
        {
            _client = new RestClient();
        }

        public T WithHeader(string name, string value)
        {
            _defaultHeaders[name] = value;
            return (T)this;
        }

        public T WithAccept(string mediaType)
        {
            _accept = mediaType;
            return (T)this;
        }

        public async Task<T> GetAsync(string url)
        {
            var request = new RestRequest(url, Method.Get);
            _request = request;
            ApplyDefaultHeaders(request);
            var response = await _client.ExecuteAsync(request).ConfigureAwait(false);
            _response = response as RestResponse;
            return (T)this;
        }

        public async Task<T> PostJsonAsync(string url, object body)
        {
            var request = new RestRequest(url, Method.Post);
            _request = request;
            ApplyDefaultHeaders(request);
            // RestSharp can serialize automatically, but send raw JSON to be explicit
            var json = JsonSerializer.Serialize(body);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = await _client.ExecuteAsync(request).ConfigureAwait(false);
            _response = response as RestResponse;
            return (T)this;
        }

        public int GetStatusCode()
        {
            return _response is null ? 0 : (int)_response.StatusCode;
        }

        public Task<JsonElement> ReadJsonAsync()
        {
            if (_response is null)
                throw new InvalidOperationException("No response available. Execute a request first.");

            var s = _response.Content ?? string.Empty;
            using var doc = JsonDocument.Parse(s);
            return Task.FromResult(doc.RootElement.Clone());
        }

        public Task<TModel?> ReadJsonAsync<TModel>()
        {
            if (_response is null)
                throw new InvalidOperationException("No response available. Execute a request first.");

            var s = _response.Content ?? string.Empty;
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var model = JsonSerializer.Deserialize<TModel>(s, opts);
            return Task.FromResult(model);
        }

        public static bool JsonArrayContains(JsonElement root, string propertyName, string expected)
        {
            if (!root.TryGetProperty(propertyName, out var prop))
                return false;

            if (prop.ValueKind == JsonValueKind.Array)
            {
                foreach (var el in prop.EnumerateArray())
                {
                    if (el.ValueKind == JsonValueKind.String && el.GetString() == expected)
                        return true;
                }
            }

            // also support single string property
            if (prop.ValueKind == JsonValueKind.String)
            {
                return prop.GetString() == expected;
            }

            return false;
        }

        private void ApplyDefaultHeaders(RestRequest request)
        {
            if (!string.IsNullOrEmpty(_accept))
                request.AddHeader("Accept", _accept!);

            foreach (var kv in _defaultHeaders)
                request.AddHeader(kv.Key, kv.Value);
        }

        public void Dispose()
        {
            try
            {
                (_client as IDisposable)?.Dispose();
            }
            catch
            {
                // ignore dispose exceptions
            }
        }
    }
}
