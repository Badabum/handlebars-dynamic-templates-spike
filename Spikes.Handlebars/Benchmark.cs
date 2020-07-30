using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using HandlebarsDotNet;

namespace Spikes.Handlebars
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [RPlotExporter]
    public class Benchmark
    {
        static readonly string TemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "levis-template.hbs");
        private string _templateContent;
        
        [GlobalSetup]
        public void Setup()
        {
            _templateContent = File.ReadAllText(TemplatePath);
            HandlebarsDotNet.Handlebars.RegisterHelper("date", (writer, context, args) =>
            {
                DateTime dt = (DateTime)args[0];
                var format = args[1] as string;
                writer.WriteSafeString(dt.ToString(format));
            });
            HandlebarsDotNet.Handlebars.RegisterHelper("price", (writer, context, args) =>
            {
                string formated = args[0].GetType() switch
                {
                    {} d when d == typeof(double) => ((double)args[0]).ToString("0.00"),
                    {} i when i == typeof(int) => ((int)args[0]).ToString("0.00"),
                    {} dd when dd == typeof(decimal) => ((decimal)args[0]).ToString("0.00"),
                    _ => args[0].ToString()
                };
                writer.WriteSafeString(formated);
            });
        }

        [Benchmark(Baseline = true)]
        public string Compile() => LevisCompiler.Compile(_templateContent);
    }
}