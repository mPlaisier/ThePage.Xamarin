using System;
namespace ThePage.Core
{
    public interface ICellDebug
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