namespace Anxilaris.Utils.Core.Interfaces
{
    using System.Collections.Generic;

    public interface IReadable<TResult, UFilters>: IReadable<long, TResult, UFilters>
    {

    }

    public interface IReadable<TId, TResult, UFilters>
    {
        TResult GetById(TId id);

        IEnumerable<TResult> Get(ref UFilters filters);
    }
}
