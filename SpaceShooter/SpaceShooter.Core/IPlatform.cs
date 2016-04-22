using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public interface IPlatform
    {
        IList<string> ReadLines(string fileName);
        string ReadText(string fileName);
        void WriteText(string fileName, string text);
    }

    public interface IPlatformAsync : IPlatform
    {
        Task<IList<string>> ReadLinesAsync(string fileName);
        Task<string> ReadTextAsync(string fileName);
        Task WriteTextAsync(string fileName, string text);
    }
}
