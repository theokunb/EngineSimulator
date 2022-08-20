using EngineSimulator.Services;

namespace EngineSimulator.Models
{
    public class Engine : IEngine
    {
        public Engine()
        {
            isRunning = false;
            _Tengine = 0;
            elapsedTime = 0;
            index = 0;
        }



        private float elapsedTime;
        private bool isRunning;
        private float _Tengine;
        /// <summary>
        /// узел интерполирования правый
        /// </summary>
        private int index;
        /// <summary>
        /// шаг по времени
        /// </summary>
        private float tao = 1f;
        /// <summary>
        /// момент инерции двигателя
        /// </summary>
        private float I;
        /// <summary>
        /// кусочно линейная зависимость крутящего момента
        /// </summary>
        private int[] M;
        /// <summary>
        /// скорость вращения коленвала
        /// </summary>
        private int[] V;
        /// <summary>
        /// температура перегрева
        /// </summary>
        private float _Tmax;
        /// <summary>
        /// коэффициент зависимости скорости нагрева от крутящего момента
        /// </summary>
        private float Hm;
        /// <summary>
        /// коэффициент зависимости скорости нагрева от скорости вращения коленвала
        /// </summary>
        private float Hv;
        /// <summary>
        /// коэффициент зависимости скорости охлаждения от температуры двигателя и окружающей среды
        /// </summary>
        private float C;




        public event Action OnTemperatureChanged;
        public float Tengine
        {
            get => _Tengine;
            set
            {
                _Tengine = value;
                OnTemperatureChanged?.Invoke();
            }
        }
        public float Tmax => _Tmax;
        public float Runtime => elapsedTime;




        private float LinearAproximation(float x1, float x2, float y1, float y2, float x)
        {
            if (x1 == x2)
                throw new Exception();
            return (x - x1) * (y1 - y2) / (x1 - x2) + y1;
        }
        private int FindVIndex(float v)
        {
            int i = 0;
            while (v > V[i])
            {
                i++;
            }
            return i;
        }



        public void Start(float Tarea)
        {
            isRunning = true;
            _Tengine = Tarea;

            float v = 0;
            float m = M[0];
            while (isRunning)
            {
                elapsedTime += tao;
                var a = m / I;
                v += a * tao;

                index = FindVIndex(v);
                if (index == 0 || index >= M.Length || index >= V.Length)
                    throw new Exception();

                m = LinearAproximation(V[index - 1], V[index], M[index - 1], M[index], v);

                Tengine += CalcalateVh(m, Hm, v, Hv) + CalculateVc(C, Tarea, Tengine);
            }

        }
        public void Stop()
        {
            isRunning = false;
        }
        public void InitializeParams(IFileParser parser)
        {
            parser.AddParams(nameof(I), nameof(M), nameof(V), nameof(Tmax), nameof(Hm), nameof(Hv), nameof(C));
            foreach (var element in parser.Parse())
            {
                switch (element.Key)
                {
                    case nameof(I):
                        {
                            I = (float)element.Value;
                            break;
                        }
                    case nameof(M):
                        {
                            M = (int[])element.Value;
                            break;
                        }
                    case nameof(V):
                        {
                            V = (int[])element.Value;
                            break;
                        }
                    case nameof(Tmax):
                        {
                            _Tmax = (float)element.Value;
                            break;
                        }
                    case nameof(Hm):
                        {
                            Hm = (float)element.Value;
                            break;
                        }
                    case nameof(Hv):
                        {
                            Hv = (float)element.Value;
                            break;
                        }
                    case nameof(C):
                        {
                            C = (float)element.Value;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
        public float CalcalateVh(float m, float hm, float v, float hv)
        {
            return m * hm + MathF.Pow(v, 2) * hv;
        }

        public float CalculateVc(float c, float tArea, float tEngine)
        {
            return c * (tArea - tEngine);
        }
    }
}
