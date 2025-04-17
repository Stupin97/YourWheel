namespace YourWheel.Domain.Dto
{
    /// <summary>
    /// Dto для пагинации
    /// </summary>
    public class PaginationDto<T>
    {
        /// <summary>
        /// Всего элементов
        /// </summary>
        public int TotalItems { get; }

        /// <summary>
        /// Элементов на странице
        /// </summary>
        public int ItemsPerPage { get; }

        /// <summary>
        /// Страница
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Список сущности
        /// </summary>
        public T[] ReturnedEntities { get; }

        /// <summary>
        /// Пагинация
        /// </summary>
        /// <param name="totalItems">Всего элементов</param>
        /// <param name="itemsPerPage">Элементов на странице</param>
        /// <param name="offset">Смещение</param>
        /// <param name="returnedEntities">Возвращаемые сущности</param>
        public PaginationDto(int totalItems, int itemsPerPage, int offset, IEnumerable<T> returnedEntities)
        {
            this.TotalItems = totalItems;

            this.ItemsPerPage = itemsPerPage;

            this.Offset = offset;

            this.ReturnedEntities = returnedEntities as T[] ?? returnedEntities.ToArray();
        }
    }
}
