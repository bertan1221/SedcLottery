using System.IO;

namespace Lottery.Service
{
    public interface IExcelManager
    {
        void ProcessExcelPackage(FileInfo fileInfo);
        void ProcessExcelPackage(Stream stream);
    }
}
