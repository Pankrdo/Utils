namespace Anxilaris.Utils.Core.Interfaces
{
    public interface IAssignable<TObject> : IAssignable<long, TObject>
    {

    }

    public interface IAssignable<TId, TObject>
    {
        TId Save(TObject value);
    }
}
