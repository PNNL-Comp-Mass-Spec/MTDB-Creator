using MTDBFramework.Data;

namespace MTDBFramework
{
    public interface IProcessor
    {
        Options ProcessorOptions { get; set; }

        void AbortProcessing();

    }
}