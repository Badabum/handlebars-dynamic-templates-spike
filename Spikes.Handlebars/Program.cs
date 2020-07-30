using System.IO;
using System.Threading.Tasks;

namespace Spikes.Handlebars
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*
             * Runs template compilation and saves compiled html page to output directory
             */
            
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "levis-template.hbs");
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "levis-invoice.html");
            var templateContent = await File.ReadAllTextAsync(templatePath);
            LevisCompiler.RegisteHelpers();
            var compiledHtml = LevisCompiler.Compile(templateContent);
            await File.WriteAllTextAsync(outputPath, compiledHtml);
            
            // to run benchmarks uncomment
            // var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}