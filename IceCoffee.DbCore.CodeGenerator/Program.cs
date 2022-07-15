namespace IceCoffee.DbCore.CodeGenerator
{
    internal static class Program
    {
        internal static SynchronizationContext MainThreadSyncContext { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var mainFrom = new MainForm();
            MainThreadSyncContext = SynchronizationContext.Current;
            Application.Run(mainFrom);
        }
    }
}