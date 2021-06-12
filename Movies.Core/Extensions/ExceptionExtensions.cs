using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.Core
{
	public static class ExceptionExtensions
	{
		/// <summary>
		/// Concatenates the message of the exception and all its inner exception messages.
		/// </summary>
		/// <param name="ex">Current exception.</param>
		/// <returns>Message containing all exception and inner exception messages.</returns>
		public static string Flatten(this Exception ex)
		{
			var message = "";
			var currentException = ex;
			while (currentException != null)
			{
				message += $" [{currentException.Message}]";
				currentException = currentException.InnerException;
			}

			return $"ERROR: {message}";
		}
	}
}
