﻿



//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WhippedCream.Data.User
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Data.Services;
	using Microsoft.Practices.Unity;
	using System.Data.Services.Common;
	using System.ServiceModel.Web;
	using WhippedCream;
	
	[System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]
	public partial class UserRepositoryDataService : DataService<IUserRepository>
	{
		public IUnityContainer Container { get; private set; }
	
		public UserRepositoryDataService(IUnityContainer container)
		{
			if(container == null)
				throw new ArgumentNullException("container");
	
			Container = container;
		}
	
		protected override IUserRepository CreateDataSource()
	    {
			IRepositoryFactory factory = Container.Resolve<IRepositoryFactory>();
	        return factory.CreateRepository<IUserRepository>();
	    }
	
	    protected override void HandleException(HandleExceptionArgs args)
	    {
	        if (args.Exception is DataServiceException)
	            return;
	
	        // Return a new DataServiceException as "400: bad request."
	        if (args.Exception != null && args.Exception.InnerException != null)
	        {
	            args.Exception =
	                new DataServiceException(400,
	                    args.Exception.InnerException.Message);
	        }
	        else if (args.Exception != null)
	        {
	            args.Exception = new DataServiceException(400, args.Exception.Message);
	        }
	        else
	        {
	            args.Exception = new DataServiceException(400, "Unknown");
	        }
	    }
	
	    protected override void OnStartProcessingRequest(ProcessRequestArgs args)
	    {
			if(Container.Resolve<IWhippedCreamDataLayer>().State == DataLayerState.Testing)
				base.OnStartProcessingRequest(args);
			else
				throw new System.Web.HttpException(401, "");
	    }
	
		public void Dispose() { }
	
	    public static void InitializeService(DataServiceConfiguration config)
	    {
	        config.UseVerboseErrors = true;
	        config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
	        config.SetEntitySetAccessRule("*", EntitySetRights.All);
			config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
	    }
	
		[WebGet]
	    public IEnumerable<Awards> QueryAwards(Nullable<int> number)
		{
			return this.CurrentDataSource.QueryAwards(number);
		}
		[WebGet]
	    public int MultiplyByTwo(Nullable<int> number)
		{
			return this.CurrentDataSource.MultiplyByTwo(number);
		}
		[WebGet]
	    public int MultiplyByTwoQuiet(Nullable<int> number)
		{
			return this.CurrentDataSource.MultiplyByTwoQuiet(number);
		}
	}
}

