using System.IO;
using System.Linq;
using Lottery.Data;
using Lottery.Data.Model;
using OfficeOpenXml;

namespace Lottery.Service
{
    public class ExcelManager : IExcelManager
    {
        private readonly IRepository<Code> _codeRepository;

        public ExcelManager(IRepository<Code> codeRepository)
        {
            _codeRepository = codeRepository;
        }

        public void ProcessExcelPackage(FileInfo fileInfo)
        {
            using (var content = new ExcelPackage(fileInfo))
            {
                ProcessExcelPackage(content);
            }
        }

        public void ProcessExcelPackage(Stream stream)
        {
            using (var content = new ExcelPackage(stream))
            {
                ProcessExcelPackage(content);
            }
        }

        private void ProcessExcelPackage(ExcelPackage content)
        {
            var worksheet = content.Workbook.Worksheets[1];
            var numberOfRows = worksheet.Dimension.Rows;

            for (var i = 1; i <= numberOfRows; i++)
            {
                var code = worksheet.Cells[i, 1].Value.ToString();
                var isWinning = bool.Parse(worksheet.Cells[i, 2].Value.ToString());

                if (!_codeRepository.GetAll().Any(x => x.CodeValue == code))
                {
                    var codeObject = new Code
                    {
                        CodeValue = code,
                        IsWinning = isWinning
                    };

                    _codeRepository.Insert(codeObject);
                }
            }
        }
    }
}
