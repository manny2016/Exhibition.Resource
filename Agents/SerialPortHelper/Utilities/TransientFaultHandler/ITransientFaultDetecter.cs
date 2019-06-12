namespace SerialPortHelper
{
    public interface ITransientFaultDetecter<T>
    {
        bool Detect(T condition,bool ifHasDetailErrorMessageThrowIt);
    }
}