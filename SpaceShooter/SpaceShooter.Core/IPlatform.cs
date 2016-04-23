using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SpaceShooter
{
    public interface IPlatform
    {
        IList<string> ReadLines(string fileName);
        string ReadText(string fileName);
        void WriteText(string fileName, string text);
        void ToggleFullscreen();
    }

    public interface IPlatformAsync : IPlatform
    {
        Task<IList<string>> ReadLinesAsync(string fileName);
        Task<string> ReadTextAsync(string fileName);
        Task<XmlDocument> ReadXmlAsync(string fileName);
        Task<XmlDocument> ReadXmlAsync(IPlatformFile file);
        Task WriteTextAsync(string fileName, string text);
        Task WriteXmlAsync(IPlatformFile file, XmlDocument xml);
        Task<IPlatformFile> PickSaveFileAsync(params string[] fileTypes);
        Task<IPlatformFile> PickOpenFileAsync(params string[] fileTypes);
    }

    public interface IPlatformFile
    {
    }
}
