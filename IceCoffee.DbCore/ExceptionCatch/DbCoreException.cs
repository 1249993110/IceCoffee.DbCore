namespace IceCoffee.DbCore.ExceptionCatch
{
    /// <summary>
    /// 数据库核心异常
    /// </summary>
    public class DbCoreException : CustomExceptionBase
    {
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="message"></param>
        public DbCoreException(string message) : base(message)
        {
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DbCoreException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}