using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Disorder.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Disorder
{
    public class Startup : BackgroundService
    {
        private readonly Appsettings _settings;

        public Startup(IOptions<Appsettings> options)
        {
            _settings = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var paths = _settings.StartupArgs.Select(arg => Path.Combine(string.Join(Path.DirectorySeparatorChar, AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar).SkipLast(5)), arg)).ToList();
            foreach (var path in paths)
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    var lines = (await File.ReadAllLinesAsync(file, stoppingToken)).Select(line => Regex.Match(line, @"\d*\.?\s?(.+)").Groups[1].Value).ToList();
                    switch (_settings.OrderType)
                    {
                        case OrderType.None:
                            await File.WriteAllLinesAsync(file, lines.Select((item, inedx) => $"{inedx + 1}. {item}").ToList(), stoppingToken);
                            break;
                        case OrderType.Word:
                            await File.WriteAllLinesAsync(file, lines.OrderBy(item => item).Select((item, inedx) => $"{inedx + 1}. {item}").ToList(), stoppingToken);
                            break;
                        case OrderType.Randomize:
                            var rnd = new Random();
                            await File.WriteAllLinesAsync(file, lines.OrderBy(_ => rnd.Next()).Select((item, inedx) => $"{inedx + 1}. {item}").ToList(), stoppingToken);
                            break;
                    }

                }
            }
        }
    }
}
