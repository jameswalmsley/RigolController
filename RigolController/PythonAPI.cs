using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using RigolPSU;

namespace RigolController
{
    public class PythonAPI
    {
        private ScriptEngine engine;
        private ScriptScope scope;

        public PythonAPI()
        {
            engine = Python.CreateEngine();
            scope = engine.CreateScope();

            ScriptRuntime runtime = engine.Runtime;

            runtime.LoadAssembly(typeof(String).Assembly);
            runtime.LoadAssembly(typeof(Uri).Assembly);

        }

        public void AddVariable(string name, object obj)
        {
            scope.SetVariable(name, obj);
        }

        public void AddModule(string name)
        {
            scope.ImportModule(name);
        }

        public dynamic ExecutePython(string code)
        {
            return engine.Execute(code, scope);
        }

        public int Add(int a, int b)
        {

            //RigolPSU.RigolPSU psu = new RigolPSU.RigolPSU("");
            //psu.Connect();

            //scope.SetVariable("psu", psu);
            //scope.ImportModule("time");

            int result = 0;

            var code = @"psu.Channels[0].SetVoltage(5)
time.sleep(5)
psu.Channels[0].SetVoltage(4)
time.sleep(1)
";

            //ScriptSource source = engine.CreateScriptSourceFromString(code, SourceCodeKind.SingleStatement);
            //var res = source.Execute(scope);

            //Task.Delay(1000);

            //var res = engine.Execute(code, scope);

            return result;
        }


    }
}
