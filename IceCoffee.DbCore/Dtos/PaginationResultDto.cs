using System.Collections;

namespace IceCoffee.DbCore.Dtos
{
    public class PaginationResultDto
    {
        /// <summary>
        /// 总计
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public IEnumerable Items { get; set; } = Enumerable.Empty<object>();
    }

    public class PaginationResultDto<T>
    {
        /// <summary>
        /// 总计
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        /// <summary>
        /// Implicitly converts the specified <paramref name="dto"/> to an <see cref="PaginationResultDto"/>.
        /// </summary>
        /// <param name="dto">The value to convert.</param>
        public static implicit operator PaginationResultDto(PaginationResultDto<T> dto)
        {
            return new PaginationResultDto()
            {
                Total = dto.Total,
                Items = dto.Items
            };
        }
    }
}