﻿using DbUp;
using System.Reflection;

namespace DbUpgrader
{
    internal class Program
    {
        static int Main(string[] args)
        {

            var connectionString =
                args.FirstOrDefault()
                ?? "data source=IC_ACER_NITRO_5\\SQLEXPRESS;initial catalog=SpendWiseDB;trusted_connection=true;TrustServerCertificate=True;";
            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}