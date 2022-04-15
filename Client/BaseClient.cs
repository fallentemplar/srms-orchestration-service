﻿using Microsoft.Extensions.Options;
using srms_orchestration_service.Client.ContactsService;
using srms_orchestration_service.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srms_orchestration_service.Client
{
    public abstract class BaseClient
    {
        protected readonly string BaseUrl;
        protected readonly string Token;

        protected readonly RestClient _restClient;
        protected BaseClient(RestClient restClient, IOptions<ContactsServiceClientConfig> clientConfig)
        {
            _restClient = restClient;
            BaseUrl = clientConfig.Value.Url;
            Token = clientConfig.Value.Token;
        }

        protected string CreateUrl(string endpoint)
        {
            return String.Format("{0}{1}", BaseUrl, endpoint);
        }

        protected string CreateUrl(string endpoint,string parameter)
        {
            return String.Format("{0}{1}/{2}", BaseUrl, endpoint,parameter);
        }
    }
}
