using System;

namespace SpExecSqlConverter
{
  public class DisposableObject : IDisposable
  {
    #region IDisposable

    private readonly Action disposeAction;

    private bool disposed;

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          this.disposeAction();
        }
        this.disposed = true;
      }
    }

    ~DisposableObject()
    {
      this.Dispose(false);
    }

    #endregion

    public DisposableObject(Action disposeAction)
    {
      this.disposeAction = disposeAction;
    }
  }
}
