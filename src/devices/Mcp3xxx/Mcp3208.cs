﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Device.Spi;

namespace Iot.Device.Adc
{
    /// <summary>
    /// MCP3208 Analog to Digital Converter (ADC)
    /// </summary>
    public class Mcp3208 : Mcp30xx32xx
    {
        /// <summary>
        /// Constructs Mcp3008 instance
        /// </summary>
        /// <param name="spiDevice">Device used for SPI communication</param>
        public Mcp3208(SpiDevice spiDevice) : base(spiDevice, pinCount: 8, adcResolutionBits: 12) { }
    }
}
