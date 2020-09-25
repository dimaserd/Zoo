using Croco.Core.Documentation.Models;
using NUnit.Framework;
using System.Linq;
using Zoo.GenericUserInterface.Models;

namespace Zoo.GenericUserInterface.Tests
{
    public class CheckBaseTypes
    {
        /// <summary>
        /// Internal свойство не должно показываться
        /// </summary>
        [Test]
        public void CheckDropDownDataType()
        {
            var descr = CrocoTypeDescription
                .GetDescription(typeof(DropDownListData))
                .GetMainTypeDescription();

            Assert.IsTrue(descr.IsClass);

            var props = descr.Properties;

            Assert.AreEqual(2, props.Count);
            Assert.IsNull(props.FirstOrDefault(x => x.PropertyDescription.PropertyName == nameof(DropDownListData.DataProviderTypeFullName)));
        }
    }
}