using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;
using CNTKImageProcessing;

namespace MNISTModel
{
    class Program
    {
        static void Main(string[] args)
        {
            var loadedModel = Function.Load("mnist.onnx", DeviceDescriptor.CPUDevice, ModelFormat.ONNX);

            var predictedValue = EvaluateModelOnImage("2.png", loadedModel);
            Console.WriteLine($"Actual - 2; Predicted - {predictedValue}");
            Console.Write(Environment.NewLine);

            predictedValue = EvaluateModelOnImage("5.png", loadedModel);
            Console.WriteLine($"Actual - 5; Predicted - {predictedValue}");
            Console.Write(Environment.NewLine);

            predictedValue = EvaluateModelOnImage("8.png", loadedModel);
            Console.WriteLine($"Actual - 8; Predicted - {predictedValue}");
            Console.Write(Environment.NewLine);

            Console.ReadLine();
        }

        public static int EvaluateModelOnImage(string imagePath, Function model)
        {
            var image = new Bitmap(Bitmap.FromFile(imagePath), new Size(28, 28));

            // Get the first input layer of the model. This is so we can use our new 
            // image to insert into the model for a prediction.
            var input = model.Arguments.Single();

            Console.WriteLine($"Input variable name - {input.Name}");

            var shape = input.Shape;

            var inputDataMap = new Dictionary<Variable, Value>();
            var resizedCHW = image.ParallelExtractCHW(); // Extract channel, height, and width of image
            var inputVal = Value.CreateBatch(shape, resizedCHW, DeviceDescriptor.CPUDevice);
            inputDataMap.Add(input, inputVal);

            Variable outputVar = model.Output;

            var outputDataMap = new Dictionary<Variable, Value>
            {
                { outputVar, null }
            };

            model.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);

            var outputVal = outputDataMap[outputVar];
            var outputData = outputVal.GetDenseData<float>(outputVar);

            var last = outputData.Last();

            var highest = last.Max();
            int highestIndex = Array.IndexOf(last.ToArray(), highest);

            return highestIndex;
        }
    }
}
