using EngineSimulator.Models;
using EngineSimulator.Services;


IEngine engine = new Engine();
IFileParser fileparser = new FileParser("input.txt");
engine.InitializeParams(fileparser);

ITestStand stand = new MyStand(engine);
stand.SetExperimentTime(2000);

Console.WriteLine("input area temperature");
string? input = Console.ReadLine();

if (float.TryParse(input, out float Tarea))
    stand.StartSimulating(Tarea);