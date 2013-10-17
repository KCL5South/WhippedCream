namespace WhippedCream.EntityFrameworkDataLayer
{
	/// <summary>
	/// This exception it thrown from <see cref="M:IEntityFrameworkInitializerMap.GetInitializer"/>
	/// when a mapping is unable to be made.
	/// </summary>
    public class UnableToFindRepositoryInitializerException : System.Exception
    {
		/// <summary>
		/// The string format that is used to generate the message of the exception.
		/// </summary>
        public const string MessageFormat = "The Entity Framework Data Layer was unable to find an Initializer for the repository {0}.";

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="repositoryType">The type of repository that an initializer was unable to be found for.</param>
        public UnableToFindRepositoryInitializerException(System.Type repositoryType)
            : base(string.Format(MessageFormat, repositoryType.ToString() ?? "null")) { }
    }
}