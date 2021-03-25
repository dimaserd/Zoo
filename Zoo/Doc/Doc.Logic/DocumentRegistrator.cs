using Doc.Logic.Word.Abstractions;
using Doc.Logic.Word.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Doc.Logic
{
    /// <summary>
    /// Регситратор сервисов необходимых для работы с документами
    /// </summary>
    public static class DocumentRegistrator
    {
        /// <summary>
        /// Регистрирует <see cref="IWordProccessorEngine"/> в контейнере зависимостей
        /// </summary>
        /// <param name="services"></param>
        public static void Register(this IServiceCollection services)
        {
            services.AddSingleton<IWordProccessorEngine, DocOpenFormatWordEngine>();
        }
    }
}