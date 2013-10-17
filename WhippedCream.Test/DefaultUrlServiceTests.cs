using NUnit.Framework;

namespace WhippedCream
{
	[TestFixture]
	public class DefaultUrlServiceTests
	{
		[Test]
		public void Constructor_NullBaseUri()
		{
			try
			{
				new DefaultUrlService(null, null);
				Assert.Fail("We expect a System.ArgumentNullException when we pass in a null value for the baseUri parameter.");
			}
			catch (System.ArgumentNullException) { }
		}
		[Test]
		public void Constructor_BaseUrlReturnsCorrectValue()
		{
			System.Uri baseUri = new System.Uri("http://www.google.com");
			IUrlService service = new DefaultUrlService(baseUri, null);

			Assert.AreEqual(baseUri.ToString(), service.BaseUrl, @"
The BaseUrl property should return the value of the uri that was passed to the constructor.
");
		}
		[Test]
		public void Constructor_BaseUriReturnsCorrectValue()
		{
			System.Uri baseUri = new System.Uri("http://www.google.com");
			IUrlService service = new DefaultUrlService(baseUri, null);

			Assert.AreEqual(baseUri, service.BaseUri, @"
The BaseUri property should return the value of the uri that was passed to the constructor.
");
		}

		//GetUrl
		//----------------------------------------------------------

		[Test]
		public void GetUrl_NoPreceedingSlash()
		{
			System.Uri baseUri = new System.Uri("http://www.google.com");
			IUrlService service = new DefaultUrlService(baseUri, null);

			Assert.AreEqual("http://www.google.com/RelativePath/Path", service.GetUrl("RelativePath/Path"), @"
The GetUrl method should take in a path and append it to the end of the base Url.
If the path has a preceeding slash or not, it shouldn't matter.
");
		}
		[Test]
		public void GetUrl_PreceedingSlash()
		{
			System.Uri baseUri = new System.Uri("http://www.google.com");
			IUrlService service = new DefaultUrlService(baseUri, null);

			Assert.AreEqual("http://www.google.com/RelativePath/Path", service.GetUrl("/RelativePath/Path"), @"
The GetUrl method should take in a path and append it to the end of the base Url.
If the path has a preceeding slash or not, it shouldn't matter.
");
		}
		[Test]
		public void GetUrl_WhiteSpaceAllOver()
		{
			System.Uri baseUri = new System.Uri("http://www.google.com");
			IUrlService service = new DefaultUrlService(baseUri, null);

			Assert.AreEqual("http://www.google.com/RelativePath/Path", service.GetUrl("   RelativePath/Path	"), @"
The GetUrl method should take in a path and append it to the end of the base Url.
All whitespace should be trimmed off of the path.
");
		}
		[Test]
		public void GetUrl_NullPath()
		{
			System.Uri baseUri = new System.Uri("http://www.google.com");
			IUrlService service = new DefaultUrlService(baseUri, null);

			Assert.AreEqual(baseUri.ToString(), service.GetUrl(null), @"
The GetUrl method should take in a path and append it to the end of the base Url.
If null is passed for the path, then the base url should be returned.
");
		}

		//GetUri
		//-------------------------------------------------------

		[Test]
		public void GetUri_NoPreceedingSlash()
		{
			System.Uri baseUri = new System.Uri("http://www.google.com");
			IUrlService service = new DefaultUrlService(baseUri, null);

			Assert.AreEqual(new System.Uri("http://www.google.com/RelativePath/Path"), service.GetUri("RelativePath/Path"), @"
The GetUri method should take in a path and append it to the end of the base Url.
If the path has a preceeding slash or not, it shouldn't matter.
");
		}
		[Test]
		public void GetUri_PreceedingSlash()
		{
			System.Uri baseUri = new System.Uri("http://www.google.com");
			IUrlService service = new DefaultUrlService(baseUri, null);

			Assert.AreEqual(new System.Uri("http://www.google.com/RelativePath/Path"), service.GetUri("/RelativePath/Path"), @"
The GetUri method should take in a path and append it to the end of the base Url.
If the path has a preceeding slash or not, it shouldn't matter.
");
		}
		[Test]
		public void GetUri_WhiteSpaceAllOver()
		{
			System.Uri baseUri = new System.Uri("http://www.google.com");
			IUrlService service = new DefaultUrlService(baseUri, null);

			Assert.AreEqual(new System.Uri("http://www.google.com/RelativePath/Path"), service.GetUri("	RelativePath/Path  "), @"
The GetUri method should take in a path and append it to the end of the base Url.
If the path has a preceeding slash or not, it shouldn't matter.
");
		}
		[Test]
		public void GetUri_NullPath()
		{
			System.Uri baseUri = new System.Uri("http://www.google.com");
			IUrlService service = new DefaultUrlService(baseUri, null);

			Assert.AreEqual(new System.Uri("http://www.google.com"), service.GetUri(null), @"
The GetUri method should take in a path and append it to the end of the base Url.
If null is passed in for the path, then the Base Uri should be returned.
");
		}

		//GetServiceUrl
		//----------------------------------------------------

		[Test]
		public void GetServiceUrl_ServicePrefixPreceedingSlash()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "/api");

			Assert.AreEqual("https://www.cnn.gov/api/RelativeServicePath/PathAgain", service.GetServiceUrl("RelativeServicePath/PathAgain"), @"
The GetServiceUrl should take in a path and append the service prefix to the base url, then append
the path to the result of that.

Having a preceeding slash before the service prefix should not prevent the service from creating
a valid url.
");
		}
		[Test]
		public void GetServiceUrl_ServicePrefixNoPreceedingSlash()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api");

			Assert.AreEqual("https://www.cnn.gov/api/RelativeServicePath/PathAgain", service.GetServiceUrl("RelativeServicePath/PathAgain"), @"
The GetServiceUrl should take in a path and append the service prefix to the base url, then append
the path to the result of that.
");
		}
		[Test]
		public void GetServiceUrl_ServicePrefixPostSlash()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api/");

			Assert.AreEqual("https://www.cnn.gov/api/RelativeServicePath/PathAgain", service.GetServiceUrl("RelativeServicePath/PathAgain"), @"
The GetServiceUrl should take in a path and append the service prefix to the base url, then append
the path to the result of that.

Having a preceeding slash before the service prefix should not prevent the service from creating
a valid url.
");
		}
		[Test]
		public void GetServiceUrl_ServicePrefixNoPostSlash()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api");

			Assert.AreEqual("https://www.cnn.gov/api/RelativeServicePath/PathAgain", service.GetServiceUrl("RelativeServicePath/PathAgain"), @"
The GetServiceUrl should take in a path and append the service prefix to the base url, then append
the path to the result of that.
");
		}
		[Test]
		public void GetServiceUrl_ServicePrefixWhiteSpaceAllOver()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "	api	");

			Assert.AreEqual("https://www.cnn.gov/api/RelativeServicePath/PathAgain", service.GetServiceUrl("RelativeServicePath/PathAgain"), @"
The GetServiceUrl should take in a path and append the service prefix to the base url, then append
the path to the result of that.

All whitespace should be trimmed off of the prefix before it is used.
");
		}
		[Test]
		public void GetServiceUrl_ServicePrefixEmpty()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, string.Empty); 

			Assert.AreEqual("https://www.cnn.gov/RelativeServicePath/PathAgain", service.GetServiceUrl("RelativeServicePath/PathAgain"), @"
The GetServiceUrl should take in a path and append the service prefix to the base url, then append
the path to the result of that.

If the service prefix is empty, then it should be ignored.
");
		}
		[Test]
		public void GetServiceUrl_ServicePrefixNull()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, null); 

			Assert.AreEqual("https://www.cnn.gov/RelativeServicePath/PathAgain", service.GetServiceUrl("RelativeServicePath/PathAgain"), @"
The GetServiceUrl should take in a path and append the service prefix to the base url, then append
the path to the result of that.

If we pass null in for the service prefix, then it should be ignored. 
");
		}
		[Test]
		public void GetServiceUrl_PathPreceedingSlash()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "/api");

			Assert.AreEqual("https://www.cnn.gov/api/RelativeServicePath/PathAgain", service.GetServiceUrl("/RelativeServicePath/PathAgain"), @"
The GetServiceUrl should take in a path and append the service prefix to the base url, then append
the path to the result of that.

Having a preceeding slash before the path should not prevent the service from creating
a valid url.
");
		}
		[Test]
		public void GetServiceUrl_PathNoPreceedingSlash()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api");

			Assert.AreEqual("https://www.cnn.gov/api/RelativeServicePath/PathAgain", service.GetServiceUrl("RelativeServicePath/PathAgain"), @"
The GetServiceUrl should take in a path and append the service prefix to the base url, then append
the path to the result of that.
");
		}
		[Test]
		public void GetServiceUrl_PathWhiteSpaceAllOver()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api");

			Assert.AreEqual("https://www.cnn.gov/api/RelativeServicePath/PathAgain", service.GetServiceUrl("  	RelativeServicePath/PathAgain  "), @"
The GetServiceUrl should take in a path and append the service prefix to the base url, then append
the path to the result of that.

All whitespace should be trimmed off of the path before it is used.
");
		}
		[Test]
		public void GetServiceUrl_PathEmpty()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api"); 

			Assert.AreEqual("https://www.cnn.gov/api", service.GetServiceUrl(""), @"
The GetServiceUrl should take in a path and append the service prefix to the base url, then append
the path to the result of that.

If the path is empty, then it should be ignored.
");
		}
		[Test]
		public void GetServiceUrl_PathNull()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api");

			Assert.AreEqual("https://www.cnn.gov/api", service.GetServiceUrl(null), @"
The GetServiceUrl should take in a path and append the service prefix to the base url, then append
the path to the result of that.

If we pass null in for the path, then it should be ignored. 
");
		}

		//GetServiceUri
		//---------------------------------------------------------
		[Test]
		public void GetServiceUri_ServicePrefixPreceedingSlash()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "/api");

			Assert.AreEqual(new System.Uri("https://www.cnn.gov/api/RelativeServicePath/PathAgain"), service.GetServiceUri("RelativeServicePath/PathAgain"), @"
The GetServiceUri should take in a path and append the service prefix to the base url, then append
the path to the result of that.

Having a preceeding slash before the service prefix should not prevent the service from creating
a valid url.
");
		}
		[Test]
		public void GetServiceUri_ServicePrefixNoPreceedingSlash()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api");

			Assert.AreEqual(new System.Uri("https://www.cnn.gov/api/RelativeServicePath/PathAgain"), service.GetServiceUri("RelativeServicePath/PathAgain"), @"
The GetServiceUri should take in a path and append the service prefix to the base url, then append
the path to the result of that.
");
		}
		[Test]
		public void GetServiceUri_ServicePrefixPostSlash()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api/");

			Assert.AreEqual(new System.Uri("https://www.cnn.gov/api/RelativeServicePath/PathAgain"), service.GetServiceUri("RelativeServicePath/PathAgain"), @"
The GetServiceUri should take in a path and append the service prefix to the base url, then append
the path to the result of that.

Having a preceeding slash before the service prefix should not prevent the service from creating
a valid url.
");
		}
		[Test]
		public void GetServiceUri_ServicePrefixNoPostSlash()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api");

			Assert.AreEqual(new System.Uri("https://www.cnn.gov/api/RelativeServicePath/PathAgain"), service.GetServiceUri("RelativeServicePath/PathAgain"), @"
The GetServiceUri should take in a path and append the service prefix to the base url, then append
the path to the result of that.
");
		}
		[Test]
		public void GetServiceUri_ServicePrefixWhiteSpaceAllOver()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "	api	");

			Assert.AreEqual(new System.Uri("https://www.cnn.gov/api/RelativeServicePath/PathAgain"), service.GetServiceUri("RelativeServicePath/PathAgain"), @"
The GetServiceUri should take in a path and append the service prefix to the base url, then append
the path to the result of that.

All whitespace should be trimmed off of the prefix before it is used.
");
		}
		[Test]
		public void GetServiceUri_ServicePrefixEmpty()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, string.Empty); 

			Assert.AreEqual(new System.Uri("https://www.cnn.gov/RelativeServicePath/PathAgain"), service.GetServiceUri("RelativeServicePath/PathAgain"), @"
The GetServiceUri should take in a path and append the service prefix to the base url, then append
the path to the result of that.

If the service prefix is empty, then it should be ignored.
");
		}
		[Test]
		public void GetServiceUri_ServicePrefixNull()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, null); 

			Assert.AreEqual(new System.Uri("https://www.cnn.gov/RelativeServicePath/PathAgain"), service.GetServiceUri("RelativeServicePath/PathAgain"), @"
The GetServiceUri should take in a path and append the service prefix to the base url, then append
the path to the result of that.

If we pass null in for the service prefix, then it should be ignored. 
");
		}
		[Test]
		public void GetServiceUri_PathPreceedingSlash()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "/api");

			Assert.AreEqual(new System.Uri("https://www.cnn.gov/api/RelativeServicePath/PathAgain"), service.GetServiceUri("/RelativeServicePath/PathAgain"), @"
The GetServiceUri should take in a path and append the service prefix to the base url, then append
the path to the result of that.

Having a preceeding slash before the path should not prevent the service from creating
a valid url.
");
		}
		[Test]
		public void GetServiceUri_PathNoPreceedingSlash()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api");

			Assert.AreEqual(new System.Uri("https://www.cnn.gov/api/RelativeServicePath/PathAgain"), service.GetServiceUri("RelativeServicePath/PathAgain"), @"
The GetServiceUri should take in a path and append the service prefix to the base url, then append
the path to the result of that.
");
		}
		[Test]
		public void GetServiceUri_PathWhiteSpaceAllOver()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api");

			Assert.AreEqual(new System.Uri("https://www.cnn.gov/api/RelativeServicePath/PathAgain"), service.GetServiceUri("  	RelativeServicePath/PathAgain  "), @"
The GetServiceUri should take in a path and append the service prefix to the base url, then append
the path to the result of that.

All whitespace should be trimmed off of the path before it is used.
");
		}
		[Test]
		public void GetServiceUri_PathEmpty()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api"); 

			Assert.AreEqual(new System.Uri("https://www.cnn.gov/api"), service.GetServiceUri(""), @"
The GetServiceUri should take in a path and append the service prefix to the base url, then append
the path to the result of that.

If the path is empty, then it should be ignored.
");
		}
		[Test]
		public void GetServiceUri_PathNull()
		{
			System.Uri baseUri = new System.Uri("https://www.cnn.gov");
			IUrlService service = new DefaultUrlService(baseUri, "api");

			Assert.AreEqual(new System.Uri("https://www.cnn.gov/api"), service.GetServiceUri(null), @"
The GetServiceUri should take in a path and append the service prefix to the base url, then append
the path to the result of that.

If we pass null in for the path, then it should be ignored. 
");
		}
	}
}
