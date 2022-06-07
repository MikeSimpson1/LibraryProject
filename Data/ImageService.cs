using IronPython.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApplication.Data
{
    public class ImageService
    {
        public void test()
        {
            var engine = Python.CreateEngine();
            var source = engine.CreateScriptSourceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "calc.py"));
            var scope = engine.CreateScope();

            source.Execute(scope);
            var script = scope.GetVariable("calculator");
            var scriptInstance = engine.Operations.CreateInstance(script);
            Console.WriteLine("Script: " + scriptInstance.add(5, 5));
        }
    }
}
