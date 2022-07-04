using AnotherExcelWriterPOC;
using System.Diagnostics;

try
{
    Stopwatch sw = new Stopwatch();
    sw.Start();
    ExcelWriter.CreateExcelFile();
    sw.Stop();

    Console.WriteLine($"Time elapsed in seconds : {sw.ElapsedMilliseconds / 1000}");
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}