using Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsParser
{
    /// <summary>
    /// Интерфейс для парсера новостей
    /// </summary>
    public interface INewsParser
    {
        /// <summary>
        /// Метод извлекает новости с сайта
        /// </summary>
        /// <param name="count">Количество новостей</param>
        /// <returns>Возвращает перечисление извлеченных новостей</returns>
        Task<IEnumerable<Article>> GetNewsAsync(int count);
    }
}
