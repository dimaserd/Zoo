﻿using Ecc.Logic.Settings;

namespace Ecc.Logic.Services
{
    /// <summary>
    /// Провайдер пикселя для прочитки эмейла
    /// </summary>
    public class EccPixelUrlProvider
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="settings"></param>
        public EccPixelUrlProvider(EccSettings settings)
        {
            PixelEmalUrlFormat = settings.PixelUrlProviderOptions.PixelEmailUrlFormat;
        }

        string PixelEmalUrlFormat { get; }

        /// <summary>
        /// Получить адрес для установки пикселя для определения прочитанности писем
        /// </summary>
        /// <param name="interactionId"></param>
        /// <returns></returns>
        public string GetPixelEmailUrl(string interactionId)
        {
            return string.Format(PixelEmalUrlFormat, interactionId);
        }
    }
}