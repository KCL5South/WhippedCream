namespace WhippedCream
{
    /// <summary>
    /// This is a decorator interface.
    /// 
    /// We need to make sure that all of our repositories inherit from 
    /// <see cref="System.IDisposable"/> so, we require them to inherit
    /// from this.
    /// </summary>
    public interface IRepository : System.IDisposable { }
}