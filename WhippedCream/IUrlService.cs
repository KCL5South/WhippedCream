namespace WhippedCream
{
	/// <summary>
	/// This service is used by the DataServiceDataLayer to generate
	/// Urls and Uris for WCF Data Services.
	/// </summary>
	public interface IUrlService
	{
		/// <summary>
		/// Gets the Root Url for all services.
		/// </summary>
		string BaseUrl { get; }
		/// <summary>
		/// Gets the Root Uri for all services.
		/// </summary>
		System.Uri BaseUri { get; }

		/// <summary>
		/// Appends <paramref name="path"/> to <see cref="P:BaseUrl"/>.
		/// </summary>
		/// <param name="path">The path to append to the Base Url</param>
		/// <returns>Returns the Base Url with the path appended to the end.</returns>
		string GetUrl(string path);
		/// <summary>
		/// Appends <paramref name="path"/> to <see cref="P:BaseUri"/>.
		/// </summary>
		/// <param name="path">The path to append to the Base Uri</param>
		/// <returns>Returns the Base Uri with the path appended to the end.</returns>
		System.Uri GetUri(string path);

		/// <summary>
		/// Appends <paramref name="path"/> to a service specific Url.
		/// </summary>
		/// <param name="path">The path to append to a service specific Url.</param>
		/// <returns>Returns a service specific url with path appended to it.</returns>
		string GetServiceUrl(string path);
		/// <summary>
		/// Appends <paramref name="path"/> to a service specific Uri.
		/// </summary>
		/// <param name="path">The path to append to a service specific Uri.</param>
		/// <returns>Returns a service specific uri with path appended to it.</returns>
		System.Uri GetServiceUri(string path);
	}
}
