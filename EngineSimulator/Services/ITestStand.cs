namespace EngineSimulator.Services
{
    public interface ITestStand
    {
        void StartSimulating(float Tarea);
        void SetExperimentTime(float seconds);
    }
}
