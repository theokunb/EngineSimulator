namespace EngineSimulator.Services
{
    public interface IEngine
    {
        event Action OnTemperatureChanged;
        float Tengine { get; set; }
        float Tmax { get; }
        float Runtime { get; }
        void Start(float Tarea);
        void Stop();
        void InitializeParams(IFileParser parser);
        float CalcalateVh(float m, float hm, float v, float hv);
        float CalculateVc(float c, float tArea, float tEngine);
    }
}
