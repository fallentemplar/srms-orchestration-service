﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace srms_orchestration_service.Config
{
    public class RestClient
    {
        private readonly HttpClient _client;
        public RestClient()
        {
            _client = new HttpClient();
            SetParameters();
        }

        private void SetParameters()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> Get<T>(string url)
        {
            HttpResponseMessage httpResponseMessage = await _client.GetAsync(url);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string response = httpResponseMessage.Content.ReadAsStringAsync().Result;
                T parsedResponse = JsonConvert.DeserializeObject<T>(response);
                return parsedResponse;
            }
            throw new Exception("Cannot retrieve contact");
        }

        public async Task<T> Post<T>(string url, Object body)
        {
            string jsonContent = JsonConvert.SerializeObject(body);
            HttpResponseMessage httpResponseMessage = await _client.PostAsync(url, new StringContent(jsonContent, Encoding.UTF8, "application/json"));
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string response = httpResponseMessage.Content.ReadAsStringAsync().Result;
                T parsedResponse = JsonConvert.DeserializeObject<T>(response);
                return parsedResponse;
            }
            throw new Exception("Cannot create contact");
        }

        public async Task<T> Put<T>(string url, Object body)
        {
            string jsonContent = JsonConvert.SerializeObject(body);
            HttpResponseMessage httpResponseMessage = await _client.PutAsync(url, new StringContent(jsonContent, Encoding.UTF8, "application/json"));
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string response = httpResponseMessage.Content.ReadAsStringAsync().Result;
                T parsedResponse = JsonConvert.DeserializeObject<T>(response);
                return parsedResponse;
            }
            throw new Exception("Cannot update contact");
        }

        public async Task<bool> Delete(string url)
        {
            HttpResponseMessage httpResponseMessage = await _client.DeleteAsync(url);

            return httpResponseMessage.IsSuccessStatusCode;
        }
    }
}