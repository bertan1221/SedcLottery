using System.IO;
using System.Linq;
using Lottery.Data;
using Lottery.Data.Model;
using OfficeOpenXml;

namespace Lottery.Service
{
    public class LocalCodesManager : ICodesManager
    {
        private readonly IExcelManager _excelManager;
        public LocalCodesManager(IExcelManager excelManager)
        {
            _excelManager = excelManager;
        }

        public void ProcessCodes()
        {
            var folderName = @"CodeFiles\";
            var folderPath = $@"{Directory.GetCurrentDirectory()}\{folderName}";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var directoryInfo = new DirectoryInfo(folderPath);
            var files = directoryInfo.GetFiles("*.xlsx");

            foreach (var file in files)
            {
                _excelManager.ProcessExcelPackage(file);

                File.Delete(file.FullName);
            }
        }
    }
}
