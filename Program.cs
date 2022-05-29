using System;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Runtime.InteropServices;

namespace hilbert_curve
{
	class Program
	{
		static bool[,] Rotate(bool[,] baseArray)
		{
			int width = baseArray.GetLength(0);
			int maxIndex = width - 1;
			bool[,] rotateLeft = new bool[width, width];

			for (int col = 0; col < width; col++)
			{
				for (int element = 0; element < width; element++)
				{
					rotateLeft[maxIndex - col, element] = baseArray[element, col];
				}
			}
			return rotateLeft;
		}
		static void Main(string[] args)
		{
			int N = 12;
			string savepath = "C:\\Users\\hjuz0\\source\\repos\\Hilbert curve";
			Stopwatch sw = new Stopwatch();
			sw.Start();
			bool[,] baseArray = new bool[3, 3] { { true, false, true }, { true, false, true }, { true, true, true } };

			int width = baseArray.GetLength(0);
			for (int i = 0; i < N; i++)
			{
				width = (baseArray.GetLength(0) * 2) + 1;
				int maxIndex = width - 1;

				bool[,] left = Rotate(baseArray);
				bool[,] newGrid = new bool[width, width];

				for (int row = 0; row < left.GetLength(0); row++)
				{
					for (int col = 0; col < left.GetLength(0); col++)
					{
						newGrid[row, col] = left[row, col];
						newGrid[row, maxIndex - col] = left[row, col];
						newGrid[row + baseArray.GetLength(0) + 1, col] = baseArray[row, col];
						newGrid[row + baseArray.GetLength(0) + 1, maxIndex - col] = baseArray[row, col];
					}
					newGrid[row, (newGrid.GetLength(0) - 1) / 2] = false;
				}
				newGrid[(newGrid.GetLength(0) - 1) / 2, 0] = newGrid[(newGrid.GetLength(0) - 1) / 2, newGrid.GetLength(0) - 1] = true;
				newGrid[((newGrid.GetLength(0) - 1) / 2) + 1, (newGrid.GetLength(0) - 1) / 2] = true;

				baseArray = newGrid;
				Console.WriteLine($"It took {sw.ElapsedMilliseconds / 1000.0} seconds to do layer {i + 1} ");
			}
            
			Console.WriteLine("Done with calculations!");

			byte[] byteArray = new byte[width * width];

			for (int i = 0; i < width; i++)
            {
                byte[] bitten = new byte[width];
                for (int j = 0; j < width; j++)
                {
					byteArray[(i*width) + j] = Convert.ToByte(Convert.ToInt16(baseArray[i, j])*255);
                }
            }

			BitmapSource image = BitmapSource.Create(width, width, 4, 4, PixelFormats.Gray8, BitmapPalettes.Gray256, byteArray,width);

			FileStream imageFileStream = new FileStream($"{savepath}\\hilbert_{N}.png",FileMode.Create);
			PngBitmapEncoder encoder = new PngBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(image));
			encoder.Save(imageFileStream);

			Console.WriteLine("Done with saving, press any button to exit!");

			imageFileStream.Close();

			Console.ReadKey();
		}
	}
}
