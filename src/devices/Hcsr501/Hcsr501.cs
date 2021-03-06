﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Device.Gpio;

namespace Iot.Device.Hcsr501
{
    /// <summary>
    /// PIR Sensor HC-SR501
    /// </summary>
    public class Hcsr501 : IDisposable
    {
        private GpioController _controller;
        private readonly int _outPin;

        /// <summary>
        /// Creates a new instance of the HC-SCR501.
        /// </summary>
        /// <param name="outPin">OUT Pin</param>
        /// <param name="pinNumberingScheme">Pin Numbering Scheme</param>
        public Hcsr501(int outPin, PinNumberingScheme pinNumberingScheme = PinNumberingScheme.Logical)
        {
            _outPin = outPin;

            _controller = new GpioController(pinNumberingScheme);
            _controller.OpenPin(outPin, PinMode.Input);
            _controller.RegisterCallbackForPinValueChangedEvent(outPin, PinEventTypes.Falling, Sensor_ValueChanged);
            _controller.RegisterCallbackForPinValueChangedEvent(outPin, PinEventTypes.Rising, Sensor_ValueChanged);
        }

        /// <summary>
        /// If a motion is detected, return true.
        /// </summary>
        public bool IsMotionDetected => _controller.Read(_outPin) == PinValue.High;

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Dispose()
        {
            if(_controller != null)
            {
                _controller.Dispose();
                _controller = null;
            }
        }

        /// <summary>
        /// Delegate used for <see cref="Hcsr501ValueChanged"/> event
        /// </summary>
        /// <param name="sender">Object firing the event</param>
        /// <param name="e">Event arguments</param>
        public delegate void Hcsr501ValueChangedHandle(object sender, Hcsr501ValueChangedEventArgs e);

        /// <summary>
        /// Triggering when HC-SR501 value changes
        /// </summary>
        public event Hcsr501ValueChangedHandle Hcsr501ValueChanged;

        private void Sensor_ValueChanged(object sender, PinValueChangedEventArgs e)
        {
            Hcsr501ValueChanged(sender, new Hcsr501ValueChangedEventArgs(_controller.Read(_outPin)));
        }
    }
}
