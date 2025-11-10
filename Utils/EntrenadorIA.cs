using Microsoft.ML;
using System;
using System.IO;

namespace GymManager.Utils
{
    public static class EntrenadorIA
    {
        public static void EntrenarModelo()
        {
            var ml = new MLContext();

            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ML", "dataset_rutinas.csv");
            if (!File.Exists(dataPath))
            {
                Console.WriteLine("❌ No se encontró el dataset: " + dataPath);
                return;
            }

            // 📥 1. Cargar dataset
            var data = ml.Data.LoadFromTextFile<RutinaInput>(
                path: dataPath,
                hasHeader: true,
                separatorChar: ',');

            // ⚙️ 2. Pipeline base (transformación de categorías a números)
            var basePipeline = ml.Transforms.Categorical.OneHotEncoding(new[]
            {
                new InputOutputColumnPair("ObjetivoEncoded", nameof(RutinaInput.Objetivo)),
                new InputOutputColumnPair("TipoCargaEncoded", nameof(RutinaInput.TipoCarga)),
                new InputOutputColumnPair("GrupoMuscularEncoded", nameof(RutinaInput.GrupoMuscular)),
                new InputOutputColumnPair("EjercicioEncoded", nameof(RutinaInput.Ejercicio))
            })
            .Append(ml.Transforms.Concatenate("Features",
                "ObjetivoEncoded", "TipoCargaEncoded",
                "GrupoMuscularEncoded", "EjercicioEncoded"));

            // 🎯 3. Entrenar tres modelos distintos

            // --- SERIES ---
            var pipelineSeries = basePipeline
                .Append(ml.Transforms.CopyColumns("Label", "Series"))
                .Append(ml.Regression.Trainers.Sdca(labelColumnName: "Label", maximumNumberOfIterations: 100));
            var modelSeries = pipelineSeries.Fit(data);

            // --- REPETICIONES ---
            var pipelineReps = basePipeline
                .Append(ml.Transforms.CopyColumns("Label", "Repeticiones"))
                .Append(ml.Regression.Trainers.Sdca(labelColumnName: "Label", maximumNumberOfIterations: 100));
            var modelReps = pipelineReps.Fit(data);

            // --- CARGA ---
            var pipelineCarga = basePipeline
                .Append(ml.Transforms.CopyColumns("Label", "Carga"))
                .Append(ml.Regression.Trainers.Sdca(labelColumnName: "Label", maximumNumberOfIterations: 100));
            var modelCarga = pipelineCarga.Fit(data);

            // 💾 4. Guardar los modelos entrenados
            ml.Model.Save(modelSeries, data.Schema, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modelo_series.zip"));
            ml.Model.Save(modelReps, data.Schema, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modelo_reps.zip"));
            ml.Model.Save(modelCarga, data.Schema, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modelo_carga.zip"));

            Console.WriteLine("✅ Modelos entrenados y guardados correctamente.");
            Console.WriteLine("📂 Directorio base: " + AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
