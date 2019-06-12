namespace SerialPortHelper
{
    using System;

    public interface ITransientFaultHandler<R, T>
    {
        ITransientFaultDetecter<T> Detecter { get; }
        Func<R> Function { get; }

        R Execute();
    }
}