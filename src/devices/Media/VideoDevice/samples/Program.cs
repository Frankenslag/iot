﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
using System.IO;
using Iot.Device.Media;

namespace V4l2.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            VideoConnectionSettings settings = new VideoConnectionSettings(0, (2560, 1920), PixelFormat.JPEG);
            using VideoDevice device = VideoDevice.Create(settings);

            // Get the supported formats of the device
            foreach (PixelFormat item in device.GetSupportedPixelFormats())
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();

            // Get the resolutions of the format
            foreach ((uint Width, uint Height) in device.GetPixelFormatResolutions(PixelFormat.YUYV))
            {
                Console.Write($"{Width}x{Height} ");
            }
            Console.WriteLine();

            // Query v4l2 controls default and current value
            VideoDeviceValue value = device.GetVideoDeviceValue(VideoDeviceValueType.Rotate);
            Console.WriteLine($"{value.Name} Min: {value.Minimum} Max: {value.Maximum} Step: {value.Step} Default: {value.DefaultValue} Current: {value.CurrentValue}");

            string path = Directory.GetCurrentDirectory();

            // Take photos
            device.Capture($"{path}/jpg_direct_output.jpg");

            // Change capture setting
            device.Settings.PixelFormat = PixelFormat.YUV420;

            // Convert pixel format
            Color[] colors = VideoDevice.Yv12ToRgb(device.Capture(), settings.CaptureSize);
            Bitmap bitmap = VideoDevice.RgbToBitmap(settings.CaptureSize, colors);
            bitmap.Save($"{path}/yuyv_to_jpg.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
