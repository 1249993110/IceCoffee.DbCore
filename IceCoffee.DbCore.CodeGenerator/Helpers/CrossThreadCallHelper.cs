namespace IceCoffee.DbCore.CodeGenerator.Helpers
{
    internal static class CrossThreadCallHelper
    {
        private static void SendOrPostCallback(object state)
        {
            if (state is CrossThreadCallArgs args)
            {
                args.AppendTextCallBack?.Invoke(args.Message);
            }
        }

        public static void Post(Action<string> appendTextCallBack, string message)
        {
            Program.MainThreadSyncContext?.Post(SendOrPostCallback, new CrossThreadCallArgs(appendTextCallBack, message));
        }

        public static void Send(Action<string> appendTextCallBack, string message)
        {
            Program.MainThreadSyncContext?.Send(SendOrPostCallback, new CrossThreadCallArgs(appendTextCallBack, message));
        }
    }
}