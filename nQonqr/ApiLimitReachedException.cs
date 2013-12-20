using System;

namespace nQonqr
{
	/// <summary>
	/// Exception thrown when the API has made calls more quickly or more often than it's allowed.
	/// </summary>
	public class ApiLimitReachedException
		: Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApiLimitReachedException"/> class.
		/// </summary>
		public ApiLimitReachedException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ApiLimitReachedException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public ApiLimitReachedException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ApiLimitReachedException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		/// <param name="inner">The exception raised which prompted this exception.</param>
		public ApiLimitReachedException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}