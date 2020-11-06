﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.Spi;
using System.Text;
using System.Threading;
using Iot.Device.Nrf24l01;

// SPI0 CS0
SpiConnectionSettings senderSettings = new (0, 0)
{
    ClockFrequency = Nrf24l01.SpiClockFrequency,
    Mode = Nrf24l01.SpiMode
};
// SPI1 CS0
SpiConnectionSettings receiverSettings = new (1, 2)
{
    ClockFrequency = Nrf24l01.SpiClockFrequency,
    Mode = Nrf24l01.SpiMode
};
using SpiDevice senderDevice = SpiDevice.Create(senderSettings);
using SpiDevice receiverDevice = SpiDevice.Create(receiverSettings);

// SPI Device, CE Pin, IRQ Pin, Receive Packet Size
using Nrf24l01 sender = new Nrf24l01(senderDevice, 23, 24, 20);
using Nrf24l01 receiver = new Nrf24l01(receiverDevice, 5, 6, 20);
// Set sender send address, receiver pipe0 address (Optional)
byte[] receiverAddress = Encoding.UTF8.GetBytes("NRF24");
sender.Address = receiverAddress;
receiver.Pipe0.Address = receiverAddress;

// Binding DataReceived event
receiver.DataReceived += Receiver_ReceivedData;

// Loop
while (true)
{
    sender.Send(Encoding.UTF8.GetBytes("Hello! .NET Core IoT"));
    Thread.Sleep(2000);
}

void Receiver_ReceivedData(object sender, DataReceivedEventArgs e)
{
    var raw = e.Data;
    var res = Encoding.UTF8.GetString(raw);

    Console.Write("Received Raw Data: ");
    foreach (var item in raw)
    {
        Console.Write($"{item} ");
    }

    Console.WriteLine();

    Console.WriteLine($"Message: {res}");
    Console.WriteLine();
}
