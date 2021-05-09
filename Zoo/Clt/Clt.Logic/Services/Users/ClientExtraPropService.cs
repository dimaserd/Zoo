using Clt.Contract.Models.Clients.Properties;
using Clt.Model.Entities;
using Croco.Core.Contract;
using Croco.Core.Contract.Application;
using Croco.Core.Contract.Models;
using Croco.Core.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Clt.Logic.Services.Users
{
    /// <summary>
    /// Сервис для работы с дополнительными свойствами клиента
    /// </summary>
    public class ClientExtraPropService : BaseCltService
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="application"></param>
        public ClientExtraPropService(ICrocoAmbientContextAccessor context, ICrocoApplication application) : base(context, application)
        {
        }

        /// <summary>
        /// Добавить или обновить свойство для клиента
        /// </summary>
        /// <typeparam name="TPropValue"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> AddOrUpdatePropValue<TPropValue>(AddOrUpdateClientProperty<TPropValue> model)
        {
            if(!await Query<Client>().AnyAsync(x => x.Id == model.ClientId))
            {
                return new BaseApiResponse(false, "Клиент не найден по указанному идентификатору");
            }

            var propEntity = await Query<ClientExtraProperty>()
                .FirstOrDefaultAsync(x => x.ClientId == model.ClientId && x.PropertyName == model.PropertyName);

            if(propEntity != null)
            {
                propEntity.TypeFullName = GetTypeFullName<TPropValue>();
                propEntity.ValueDataJson = Tool.JsonConverter.Serialize(model.PropertyValue);

                UpdateHandled(propEntity);

                return await TrySaveChangesAndReturnResultAsync("Значение свойства обновлено");
            }

            propEntity = new ClientExtraProperty
            {
                ClientId = propEntity.ClientId,
                TypeFullName = GetTypeFullName<TPropValue>(),
                ValueDataJson = Tool.JsonConverter.Serialize(model.PropertyValue)
            };

            CreateHandled(propEntity);

            return await TrySaveChangesAndReturnResultAsync("Значение свойства добавлено");
        }

        /// <summary>
        /// Получить значение свойства
        /// </summary>
        /// <typeparam name="TPropValue"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ClientExtraPropertyModel<TPropValue>> GetPropValue<TPropValue>(GetClientProperty model)
        {
            var propEntity = await Query<ClientExtraProperty>()
                .FirstOrDefaultAsync(x => x.ClientId == model.ClientId && x.PropertyName == model.PropertyName);

            if(propEntity == null)
            {
                return null;
            }

            var expectedTypeFullName = GetTypeFullName<TPropValue>();
            if (propEntity.TypeFullName != expectedTypeFullName)
            {
                var mes = $"Типы данных для свойства = {model.PropertyName}, для клиента c Id={model.ClientId} не совпадают. " +
                    $"Ожидалось {expectedTypeFullName}, но получено {propEntity.TypeFullName}";
                throw new InvalidOperationException(mes);
            }

            return new ClientExtraPropertyModel<TPropValue>
            {
                PropertyName = propEntity.PropertyName,
                Value = Tool.JsonConverter.Deserialize<TPropValue>(propEntity.ValueDataJson)
            };
        }

        private static string GetTypeFullName<TPropValue>()
        {
            return typeof(TPropValue).FullName;
        }
    }
}