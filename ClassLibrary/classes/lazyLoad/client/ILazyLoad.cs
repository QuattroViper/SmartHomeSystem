using System;

namespace ClassLibrary.classes.lazyLoad.client
{
    internal interface ILazyLoad<T>
    {
        T lazyLoad(Guid guid);
    }

    internal interface ILazyLoadAll<T>
    {
        T lazyLoad();
    }

    internal interface ILazyLoadTwo<T>
    {
        T lazyLoad(Guid guid, Guid guidTwo);
    }
}