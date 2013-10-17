namespace WhippedCream.DataServiceDataLayer
{
	/// <summary>
	/// This exception is thrown by <see cref="WhippedCreamUpdatableContext"/> members that 
	/// can't find a property to match a given property name.
	/// </summary>
	public class UnknownPropertyException : System.Exception
	{
		/// <summary>
		/// This is how the exception formats it's message.
		/// </summary>
		public const string MessageFormat = "The property ({0}) was not found within {1}";

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="property">The Property name we're looking for.</param>
		/// <param name="targetType">The type we're trying to find the property on.</param>
		public UnknownPropertyException(string property, System.Type targetType)
			: base(string.Format(MessageFormat, string.IsNullOrWhiteSpace(property) ? "<unknown>" : property, 
								 targetType == null ? "<unknown>" : targetType.ToString()))
		{ }
	}
}