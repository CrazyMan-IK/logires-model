namespace BrailleCanvas.Interfaces;

public interface IHasValue<T>
{
    T Value { get; }
}