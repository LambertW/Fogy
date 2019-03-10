using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fogy.Core
{
	public class DisposeAction : IDisposable
	{
		public static readonly DisposeAction Empty = new DisposeAction(null);

		private Action _action;

		/// <summary>
		/// Creates a new <see cref="DisposeAction"/> object.
		/// </summary>
		/// <param name="action">Action to be executed when this object is disposed.</param>
		public DisposeAction(Action action)
		{
			_action = action;
		}

		public void Dispose()
		{
			// Interlocked prevents multiple execution of the _action.
			var action = Interlocked.Exchange(ref _action, null);
			action?.Invoke();
		}
	}
}
