using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Disorder
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var paths = args.Select(arg => Path.Combine(GetParentPath(AppDomain.CurrentDomain.BaseDirectory, 5), arg)).ToList();
            foreach (var path in paths)
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    var lines = (await File.ReadAllLinesAsync(file)).Select(line => Regex.Match(line, @"\d+\.\s{1}(.+)").Groups[1].Value).ToList();
                    var rnd = new Random();
                    await File.WriteAllLinesAsync(file, lines.OrderBy(_ => rnd.Next()).Select((item, inedx) => $"{inedx + 1}. {item}").ToList());
                }
            }
        }

        static string GetParentPath(string path, int depth)
        {
            return string.Join(Path.DirectorySeparatorChar, path.TrimEnd(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar).SkipLast(depth));
        }
    }
}
