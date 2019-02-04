using System;
using System.ComponentModel.DataAnnotations;

namespace Data
{
    /// <summary>
    /// Модель новости
    /// </summary>
    public partial class Article
    {
        /// <summary>Идентификатор</summary>
        public int Id { get; set; }

        /// <summary>Дата публикации</summary>
        public DateTime PublishDate { get; set; }

        /// <summary>Заголовок</summary>
        [Required]
        public string Title { get; set; }

        /// <summary>Текст</summary>
        [Required]
        public string Content { get; set; }

        /// <summary>URL источника</summary>
        [Required]
        public string Url { get; set; }
    }
}
