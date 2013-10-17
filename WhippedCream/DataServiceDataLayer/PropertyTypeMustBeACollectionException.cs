namespace WhippedCream.DataServiceDataLayer
{
	/// <summary>
	/// This exception is thrown within the <see cref="WhippedCreamUpdatableContext"/> methods
	/// that expect a property name to point to a property with a return type that is an IList.
	/// </summary>
	public class PropertyTypeMustBeACollectionException : System.Exception
	{
		/// <summary>
		/// This is how the exception formats it's message.
		/// </summary>
		public const string MessageFormat = "The type {0} was expected to be an IList";

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type">The property type that was encountered.</param>
		public PropertyTypeMustBeACollectionException(System.Type type)
			: base(string.Format(MessageFormat, type == null ? "<unknown>" : type.ToString())) { }
	}
}