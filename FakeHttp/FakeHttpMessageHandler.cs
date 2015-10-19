﻿using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace FakeHttp
{
    /// <summary>
    /// A <see cref="System.Net.Http.HttpMessageHandler"/> that retrieves http resonse messages from
    /// an alternate storage rather than from a given http endpoint
    /// </summary>
    public sealed class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly IReadonlyResponseStore _store;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="store">The storage mechansim for responses</param>
        public FakeHttpMessageHandler(IReadonlyResponseStore store)
        {
            if (store == null) throw new ArgumentNullException("store");

            _store = store;
        }

        /// <summary>
        /// Override the base class to skip http and retrieve message from storage
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The stored response message</returns>
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _store.FindResponse(request);
        }
    }
}
