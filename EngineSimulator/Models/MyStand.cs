using EngineSimulator.Services;

namespace EngineSimulator.Models
{
    public class MyStand : ITestStand
    {
        public MyStand(IEngine engine)
        {
            this.engine = engine;
        }


        private readonly IEngine engine;
        private float maxTime;



        private void Engine_OnTemperatureChanged()
        {
            if (engine.Runtime >= maxTime)
            {
                engine.Stop();
                Console.WriteLine($"max temperature not reached in {maxTime} seconds");
            }
            else if (engine.Tengine >= engine.Tmax)
            {
                Console.WriteLine(engine.Runtime);
                engine.Stop();
            }
        }




        public void StartSimulating(float Tarea)
        {
            engine.OnTemperatureChanged += Engine_OnTemperatureChanged;
            engine.Start(Tarea);
        }
        public void SetExperimentTime(float seconds)
        {
            maxTime = seconds;
        }
    }
}
