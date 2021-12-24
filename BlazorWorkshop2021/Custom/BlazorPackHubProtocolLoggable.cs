// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Text;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace BlazorWorkshop2021.Custom
{
    /// <summary>
    /// Decorator (proxy) for an original <see cref="Microsoft.AspNetCore.Components.Server.BlazorPack.BlazorPackHubProtocol"/>
    /// </summary>
    internal sealed class BlazorPackHubProtocolLoggable : IHubProtocol
    {
        private readonly IHubProtocol _base;
        private readonly ILogger<BlazorPackHubProtocolLoggable> _logger;

        public BlazorPackHubProtocolLoggable(IHubProtocol @base, ILogger<BlazorPackHubProtocolLoggable> logger)
        {
            _base = @base;
            _logger = logger;
        }

        public bool TryParseMessage(ref ReadOnlySequence<byte> input, IInvocationBinder binder, out HubMessage? message)
        {
            var result = _base.TryParseMessage(ref input, binder, out message);
            _logger.LogWarning("IN: {@Message}", message);
            return result;
        }

        public void WriteMessage(HubMessage message, IBufferWriter<byte> output)
        {
            _logger.LogInformation("OUT: {@Message}", message);
            _base.WriteMessage(message, output);
        }

        public ReadOnlyMemory<byte> GetMessageBytes(HubMessage message)
            => _base.GetMessageBytes(message);

        public bool IsVersionSupported(int version)
            => _base.IsVersionSupported(version);

        public string Name => _base.Name;
        public int Version => _base.Version;
        public TransferFormat TransferFormat => _base.TransferFormat;
    }
}