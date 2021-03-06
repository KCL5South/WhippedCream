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
	using WhippedCream;
	
	[Repository(DataLayerState.Live, typeof(UserRepository))]
	[Repository(DataLayerState.Testing, typeof(UserRepositoryInMemoryRepo))]
	[RepositoryContext(DataLayerState.Live, typeof(UserRepository))]
	[RepositoryContext(DataLayerState.Testing, typeof(UserRepositoryDataServiceContext))]
	[RepositoryServicePath("UserRepository")]
	[RepositoryServiceType(typeof(UserRepositoryDataService))]
	public interface IUserRepository : IRepository
	{
	    IQueryable<User> Users { get; }
	    IQueryable<Address> Addresses { get; }
	    IQueryable<AgentId> AgentIds { get; }
	    IQueryable<ContactNumber> ContactNumbers { get; }
	    IQueryable<EMailAddress> EMailAddresses { get; }
	    IQueryable<Preference> Preferences { get; }
	    IQueryable<Website> Websites { get; }
	    IQueryable<AgentAgencyAssociation> AgentAgencyAssociations { get; }
	    IQueryable<Awards> Awards { get; }
	    IQueryable<Files> Files { get; }
	    IQueryable<FilesFileData> FilesFileDatas { get; }
	    IQueryable<FilesWizardInfo> FilesWizardInfoes { get; }
	    IQueryable<LoginEntry> LoginEntries { get; }
	    IQueryable<Badges> Badges { get; }
	    IQueryable<MultiplyByTwoResult> MultiplyByTwoResults { get; }
	
	    IEnumerable<Awards> QueryAwards(Nullable<int> number);
	
	    int MultiplyByTwo(Nullable<int> number);
	
	    int MultiplyByTwoQuiet(Nullable<int> number);
	}
}
