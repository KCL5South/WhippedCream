namespace WhippedCream.EntityFrameworkDataLayer
{
    /// <summary>
    /// Developers must implement this interface in order for the EntityFrameworkDataLayer to
    /// retreive initialization information.
    /// </summary>
    public interface IEntityFrameworkInitializerMap
    {
        /// <summary>
        /// Returns the initialization information needed for the entity framework.
        /// </summary>
        /// <typeparam name="TRepo">The Repository to retreive the initialization information for.</typeparam>
        /// <returns>
        ///     A <see cref="EntityFrameworkInitializer"/> object representing the information needed to initialize
        ///     the underlying context.
        ///     
        ///     Implementors should return EntityFrameworkInitializer.Empty if a mapping is unable to be made.
        /// </returns>
		/// <exception cref="UnableToFindRepositoryInitializerException">This is thrown if the implementation is unable to generate or find an initailizer for the type.</exception>
        EntityFrameworkInitializer GetInitializer<TRepo>();
    }
}