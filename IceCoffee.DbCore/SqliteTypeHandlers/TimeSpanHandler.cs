namespace IceCoffee.DbCore.SqliteTypeHandlers
{
    public class TimeSpanHandler : SqliteTypeHandler<TimeSpan>
    {
        public override TimeSpan Parse(object value)
            => TimeSpan.Parse((string)value);
    }
}