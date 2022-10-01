using System;
using System.Collections.Generic;

public class Observable<T>
{
    public Observable() {}
    public Observable(T value)
    {
        this.value = value;
    }

    public static implicit operator T(Observable<T> data)
    {
        return data.value;
    }

    protected T _value;
    public virtual T value
    {
        get
        {
            return _value;
        }
        set
        {
            if (!EqualityComparer<T>.Default.Equals(_value, value))
            {
                _value = value;
                onValueUpdated?.Invoke();
            }
        }
    }
    
    public event Action onValueUpdated;

    public void Set(T value) => this._value = value;
}