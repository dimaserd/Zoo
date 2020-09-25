using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models.Bag;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Переопределитель интерфейса
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericInterfaceOverrider<T> : IGenericInterfaceOverrider
        where T : class 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bag"></param>
        /// <param name="overrider"></param>
        /// <returns></returns>
        public abstract Task OverrideInterfaceAsync(GenericUserInterfaceBag bag, GenericUserInterfaceModelBuilder<T> overrider);

        /// <summary>
        /// Получить переопределитель
        /// </summary>
        /// <returns></returns>
        public Overrider GetOverrider()
        {
            return new Overrider
            {
                OverrideFunction = async (bag, model) =>
                {
                    var builder = new GenericUserInterfaceModelBuilder<T>(model, bag);

                    await OverrideInterfaceAsync(bag, builder);
                    //TODO Сделать кещируемым все что было до установки выпадающих списков
                    await ProccessDropDownDatas(bag, model);
                }
            };
        }

        private static Task ProccessDropDownDatas(GenericUserInterfaceBag bag, GenerateGenericUserInterfaceModel model)
        {
            return ProccessInterfaceModel(bag, model.Interface);
        }

        private static async Task ProccessInterfaceModel(GenericUserInterfaceBag bag, GenericInterfaceModel interfaceModel)
        {
            foreach (var block in interfaceModel.Blocks)
            {
                if(block.InterfaceType == UserInterfaceType.GenericInterfaceForArray || block.InterfaceType == UserInterfaceType.GenericInterfaceForClass)
                {
                    await ProccessInterfaceModel(bag, interfaceModel);
                }

                if (block.InterfaceType == UserInterfaceType.DropDownList && block.DropDownData.DataProviderTypeFullName != null)
                {
                    var providerTypeFullName = block.DropDownData.DataProviderTypeFullName;

                    block.DropDownData.SelectList = (await bag.CallSelectListItemDataProvider(providerTypeFullName)).ToList();
                }
            }
        }
    }
}