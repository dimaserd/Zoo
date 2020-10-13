using System;

namespace Zoo.ServerJs.Models.OpenApi
{
    /// <summary>
    /// Доки для построения удаленных вызовов
    /// </summary>
    public class RemoteJsOpenApiDocs
    {
        /// <summary>
        /// Описание ремоута
        /// </summary>
        public RemoteJsOpenApi Description { get; set; }

        /// <summary>
        /// Описание методов ремоута
        /// </summary>
        public JsOpenApiDocs Docs { get; set; }

        /// <summary>
        /// Когда были получены доки
        /// </summary>
        public DateTime DocsReceivedOnUtc { get; set; }

        /// <summary>
        /// Были ли получены доки
        /// </summary>
        public bool IsDocsReceived { get; set; }
        
        /// <summary>
        /// Исключение, которое возникло при последнем получении
        /// </summary>
        public ExcepionData ReceivingException { get; set; }

        /// <summary>
        /// Данные для авторизации
        /// </summary>
        public RemoteJsOpenApiAuthenticationData AuthenticationData { get; set; }
    }
}