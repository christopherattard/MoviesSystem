namespace Movies.Models
{
	/// <summary>
	/// Stores the credentials required for API authentication.
	/// </summary>
	public class LoginModel
	{
		/// <summary>
		/// Username for API authentication.
		/// </summary>
		[Required]
		public string Username { get; set; }
		/// <summary>
		/// Password for API authentication.
		/// </summary>
		[Required]
		public string Password { get; set; }
	}
}
