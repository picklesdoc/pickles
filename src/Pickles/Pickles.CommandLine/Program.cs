#region License

/*
    Copyright [2011] [Jeffrey Cameron]

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

#endregion

using System;
using System.IO.Abstractions;
using System.Reflection;
using Autofac;
using NLog;

namespace PicklesDoc.Pickles.CommandLine
{
    internal class Program
    {
      private static readonly Logger log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(Runner).Assembly);
            builder.Register<FileSystem>(_ => new FileSystem()).As<IFileSystem>().SingleInstance();
            builder.RegisterModule<PicklesModule>();
            var container = builder.Build();

            var configuration = container.Resolve<Configuration>();

            var commandLineArgumentParser = container.Resolve<CommandLineArgumentParser>();
            var shouldContinue = commandLineArgumentParser.Parse(args, configuration, Console.Out);

            if (shouldContinue)
            {
                if (log.IsInfoEnabled)
                {
                    log.Info("Pickles v.{0}{1}", Assembly.GetExecutingAssembly().GetName().Version,
                                   Environment.NewLine);
                    new ConfigurationReporter().ReportOn(configuration, message => log.Info(message));
                }

                var runner = container.Resolve<Runner>();
                runner.Run(container);

                if (log.IsInfoEnabled)
                {
                    log.Info("Pickles completed successfully");
                }
            }
        }
    }
}