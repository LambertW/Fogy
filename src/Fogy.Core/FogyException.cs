using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core
{
	[Serializable]
	public class FogyException : Exception
	{
		/// <summary>
		/// Creates a new <see cref="FogyException"/> object.
		/// </summary>
		public FogyException()
		{

		}

		/// <summary>
		/// Creates a new <see cref="FogyException"/> object.
		/// </summary>
		public FogyException(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{

		}

		/// <summary>
		/// Creates a new <see cref="FogyException"/> object.
		/// </summary>
		/// <param name="message">Exception message</param>
		public FogyException(string message)
			: base(message)
		{

		}

		/// <summary>
		/// Creates a new <see cref="FogyException"/> object.
		/// </summary>
		/// <param name="message">Exception message</param>
		/// <param name="innerException">Inner exception</param>
		public FogyException(string message, Exception innerException)
			: base(message, innerException)
		{

		}
	}
}
