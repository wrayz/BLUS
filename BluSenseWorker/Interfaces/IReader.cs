using System.Collections.Generic;

namespace BluSenseWorker.Interfaces
{
    public interface IReader
    {
        List<Dictionary<string, string>> GetDictionaries();
    }
}