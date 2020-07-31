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
            LevisCompiler.RegisteHelpers();
        }

        [Benchmark(Baseline = true)]
        public string Compile() => LevisCompiler.Compile(_templateContent);
    }
}