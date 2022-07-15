namespace IceCoffee.DbCore.CodeGenerator.Helpers
{
    internal class CrossThreadCallArgs
    {
        public Action<string> AppendTextCallBack { get; set; }

        public string Message { get; set; }

        public CrossThreadCallArgs(Action<string> appendTextCallBack, string message)
        {
            AppendTextCallBack = appendTextCallBack;
            Message = message;
        }
    }
}