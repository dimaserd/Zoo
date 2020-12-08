using System.Linq;
using System.Threading.Tasks;
using Zoo.GenericUserInterface.Abstractions;
using Zoo.GenericUserInterface.Enumerations;
using Zoo.GenericUserInterface.Models.Definition;
using Zoo.GenericUserInterface.Services;

namespace Zoo.GenericUserInterface.Models.Overridings
{
    /// <summary>
    /// Переопределитель интерфейса
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UserInterfaceDefinition<T> : IGenericInterfaceOverrider
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
                MainOverrideFunction = (bag, model) =>
                {
                    var builder = new GenericUserInterfaceModelBuilder<T>(model, bag);

                    return OverrideInterfaceAsync(bag, builder);
                },
                SetDropDownDatasFunction = (bag, model) =>
                {
                    return ProccessDropDownDatas(bag, model);
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
                var blockType = block.InterfaceType;

                if (blockType == UserInterfaceType.GenericInterfaceForArray || blockType == UserInterfaceType.GenericInterfaceForClass)
                {
                    await ProccessInterfaceModel(bag, block.InnerGenericInterface);
                }

                if ((blockType == UserInterfaceType.DropDownList || blockType == UserInterfaceType.MultipleDropDownList) && block.DropDownData.DataProviderTypeFullName != null)
                {
                    var providerTypeFullName = block.DropDownData.DataProviderTypeFullName;

                    block.DropDownData.SelectList = (await bag.CallSelectListItemDataProvider(providerTypeFullName)).ToList();
                }
            }
        }
    }
}