namespace Exhibition.Core
{
    public interface ITransientFaultDetecter<T>
    {
        bool Detect(T condition,bool ifHasDetailErrorMessageThrowIt);
    }
}