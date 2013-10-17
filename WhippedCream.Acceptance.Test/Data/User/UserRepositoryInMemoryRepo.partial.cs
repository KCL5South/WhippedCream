using System.Linq;

namespace WhippedCream.Data.User
{
	public partial class UserRepositoryInMemoryRepo
	{
		partial void ResolveRelationships(ODataOperation operation, object entity)
		{
			if(entity == null)
				return;

			System.Type entityType = entity.GetType();

			if(operation == ODataOperation.Insert)
			{
				if(entityType == typeof(Address))
				{
					Address targetEntity = entity as Address;
					User targetUser = Users.FirstOrDefault(a => a.UserId == targetEntity.UserId);
					if(targetUser!= null)
						targetUser.Addresses.Add(targetEntity);
				}
				else if(entityType == typeof(AgentId))
				{
					AgentId targetEntity = entity as AgentId;
					User targetUser = Users.FirstOrDefault(a => a.UserId == targetEntity.UserId);
					if(targetUser!= null)
						targetUser.AgentIds.Add(targetEntity);
				}
				else if(entityType == typeof(Badges))
				{
					Badges targetEntity = entity as Badges;
					User targetUser = Users.FirstOrDefault(a => a.UserId == targetEntity.UserId);
					if(targetUser!= null)
						targetUser.Badges.Add(targetEntity);
				}
				else if(entityType == typeof(ContactNumber))
				{
					ContactNumber targetEntity = entity as ContactNumber;
					User targetUser = Users.FirstOrDefault(a => a.UserId == targetEntity.UserId);
					if(targetUser!= null)
						targetUser.ContactNumbers.Add(targetEntity);
				}
				else if(entityType == typeof(EMailAddress))
				{
					EMailAddress targetEntity = entity as EMailAddress;
					User targetUser = Users.FirstOrDefault(a => a.UserId == targetEntity.UserId);
					if(targetUser!= null)
						targetUser.EMailAddresses.Add(targetEntity);
				}
				else if(entityType == typeof(Preference))
				{
					Preference targetEntity = entity as Preference;
					User targetUser = Users.FirstOrDefault(a => a.UserId == targetEntity.UserId);
					if(targetUser!= null)
						targetUser.Preferences.Add(targetEntity);
				}
				else if(entityType == typeof(Website))
				{
					Website targetEntity = entity as Website;
					User targetUser = Users.FirstOrDefault(a => a.UserId == targetEntity.UserId);
					if(targetUser!= null)
						targetUser.Websites.Add(targetEntity);
				}
			}
		}
	}
}