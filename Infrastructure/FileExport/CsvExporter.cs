using CsvHelper;
using Application.Contracts.Infrastructure;


namespace Infrastructure.FileExport
{
    public class CsvExporter : ICsvExporter
    {
        //public byte[] ExportOrdersToCsv(List<OrderExportDto> orderExportDtos)
        //{
        //    using var memoryStream = new MemoryStream();
        //    using (var streamWriter = new StreamWriter(memoryStream))
        //    {
        //        //using var csvWriter = new CsvWriter(streamWriter);
        //        //csvWriter.WriteRecords(orderExportDtos);
        //    }

        //    return memoryStream.ToArray();
        //}
    }
}
