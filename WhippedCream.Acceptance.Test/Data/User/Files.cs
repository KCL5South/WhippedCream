//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
namespace WhippedCream.Data.User
{
    
    [System.Data.Services.Common.DataServiceEntity]
    [System.Data.Services.Common.DataServiceKey("UserId", "FileKey")]
    public partial class Files : System.IEquatable<Files>
    {
        public System.Guid UserId { get; set; }
        public System.Guid FileKey { get; set; }
        public int FileType { get; set; }
        public System.DateTime EntryDate { get; set; }
    
    	#region Members overriden from System.Object
    
    	public override bool Equals(object obj)
    	{
    		return obj is Files ? Equals(obj as Files) : false;
    	}
    
    	/// <summary>
    	/// The Implementation of this algorithm was taken
    	/// from [here](http://www.isthe.com/chongo/tech/comp/fnv/).
    	/// </summary>
    	public override int GetHashCode()
    	{
    		int hash = 13;
    		unchecked
    		{
    			hash = (hash * 16777619) ^ UserId.GetHashCode();
    			hash = (hash * 16777619) ^ FileKey.GetHashCode();
    			hash = (hash * 16777619) ^ FileType.GetHashCode();
    			hash = (hash * 16777619) ^ EntryDate.GetHashCode();
    		}
    		return hash;
    	}
    
    	public override string ToString()
    	{
    		return base.ToString();
    	}
    
    	#endregion
    
    	#region IEquatable<Files> Members
    
    	public bool Equals(Files rhs)
    	{
    		if(rhs == null)
    			return false;
    			
    		return System.Collections.Generic.EqualityComparer<Files>.Equals(UserId, rhs.UserId) && System.Collections.Generic.EqualityComparer<Files>.Equals(FileKey, rhs.FileKey) && System.Collections.Generic.EqualityComparer<Files>.Equals(FileType, rhs.FileType) && System.Collections.Generic.EqualityComparer<Files>.Equals(EntryDate, rhs.EntryDate);
    	}
    
    	#endregion
    }
}