using System;
namespace ThePage.Core
{
    public interface ICellDebug
    {
    }

    public class CellDebug : ICellDebug
    {
    }

    public enum EDebugType
    {
        Alert,
        Toast,
        Data,
        BarcodeScanner
    }
}