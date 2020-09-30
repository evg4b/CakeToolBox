using System;

namespace CakeToolBox.Environment.TempObjects
{
    public interface ITempObject<T> : IDisposable
    {
        public T Value { get; }
    }
}