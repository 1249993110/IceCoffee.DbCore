namespace IceCoffee.DbCore.CodeGenerator
{
    internal interface IView
    {
        string Label { get; }

        int Sort { get; }
    }
}