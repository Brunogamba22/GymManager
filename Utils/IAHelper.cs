using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.IO;

namespace GymManager.Utils
{
    // Entrada de datos para predicción
    public class RutinaInput
    {
        [LoadColumn(0)]
        public string Objetivo { get; set; }

        [LoadColumn(1)]
        public string TipoCarga { get; set; }

        [LoadColumn(2)]
        public string GrupoMuscular { get; set; }

        [LoadColumn(3)]
        public string Ejercicio { get; set; }

        [LoadColumn(4)]
        public float Series { get; set; }

        [LoadColumn(5)]
        public float Repeticiones { get; set; }

        [LoadColumn(6)]
        public float Carga { get; set; }
    }

    // Salida de datos (cada modelo devuelve un solo valor)
    public class RutinaOutput
    {
        [ColumnName("Score")]
        public float Prediccion { get; set; }
    }

    public static class IAHelper
    {
        private static PredictionEngine<RutinaInput, RutinaOutput> _engineSeries;
        private static PredictionEngine<RutinaInput, RutinaOutput> _engineReps;
        private static PredictionEngine<RutinaInput, RutinaOutput> _engineCarga;

        private static readonly string modelSeriesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modelo_series.zip");
        private static readonly string modelRepsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modelo_reps.zip");
        private static readonly string modelCargaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modelo_carga.zip");

        static IAHelper()
        {
            try
            {
                var ml = new MLContext();

                if (File.Exists(modelSeriesPath))
                {
                    var modelSeries = ml.Model.Load(modelSeriesPath, out _);
                    _engineSeries = ml.Model.CreatePredictionEngine<RutinaInput, RutinaOutput>(modelSeries);
                }

                if (File.Exists(modelRepsPath))
                {
                    var modelReps = ml.Model.Load(modelRepsPath, out _);
                    _engineReps = ml.Model.CreatePredictionEngine<RutinaInput, RutinaOutput>(modelReps);
                }

                if (File.Exists(modelCargaPath))
                {
                    var modelCarga = ml.Model.Load(modelCargaPath, out _);
                    _engineCarga = ml.Model.CreatePredictionEngine<RutinaInput, RutinaOutput>(modelCarga);
                }

                Console.WriteLine("✅ Modelos IA cargados correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error al cargar modelos IA: " + ex.Message);
            }
        }

        public static (int series, int reps, string cargaStr) Predecir(
            string objetivo, string tipoCarga, string grupoMuscular, string ejercicio)
        {
            try
            {
                var input = new RutinaInput
                {
                    Objetivo = objetivo,
                    TipoCarga = tipoCarga,
                    GrupoMuscular = grupoMuscular,
                    Ejercicio = ejercicio
                };

                int s = _engineSeries != null ? (int)Math.Round(_engineSeries.Predict(input).Prediccion) : 4;
                int r = _engineReps != null ? (int)Math.Round(_engineReps.Predict(input).Prediccion) : 10;
                int c = _engineCarga != null ? (int)Math.Round(_engineCarga.Predict(input).Prediccion) : 70;

                string cargaStr = c == 0 ? "" : $"{c}%";
                return (s, r, cargaStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Error en predicción IA: " + ex.Message);
                return (4, 10, "70%"); // fallback por defecto
            }
        }

        public static void AgregarRutinaAlDataset(string objetivo, string tipoCarga, string grupoMuscular,
                                          string ejercicio, int series, int repeticiones, string carga)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string projectRoot = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\"));
                string dataPath = Path.Combine(projectRoot, "ML", "dataset_rutinas.csv");
                bool existe = File.Exists(dataPath);

                // Asegurarse de que la carpeta exista
                Directory.CreateDirectory(Path.GetDirectoryName(dataPath));

                using (var sw = new StreamWriter(dataPath, append: true))
                {
                    if (!existe)
                        sw.WriteLine("Objetivo,TipoCarga,GrupoMuscular,Ejercicio,Series,Repeticiones,Carga");

                    sw.WriteLine($"{objetivo},{tipoCarga},{grupoMuscular},{ejercicio},{series},{repeticiones},{carga}");
                }

                Console.WriteLine("✅ Nueva rutina agregada al dataset IA.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Error agregando rutina al dataset: " + ex.Message);
            }
        }



    }
}
