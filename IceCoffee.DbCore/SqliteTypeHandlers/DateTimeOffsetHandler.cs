namespace IceCoffee.DbCore.SqliteTypeHandlers
{
    public class DateTimeOffsetHandler : SqliteTypeHandler<DateTimeOffset>
    {
        public override DateTimeOffset Parse(object value)
            => DateTimeOffset.Parse((string)value);
    }
}
