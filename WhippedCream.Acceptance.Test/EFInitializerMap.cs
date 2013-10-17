namespace WhippedCream
{
    public class EFInitializerMap : WhippedCream.EntityFrameworkDataLayer.IEntityFrameworkInitializerMap
    {
        #region IEntityFrameworkInitializerMap Members

        EntityFrameworkDataLayer.EntityFrameworkInitializer EntityFrameworkDataLayer.IEntityFrameworkInitializerMap.GetInitializer<TRepo>()
        {
            return new EntityFrameworkDataLayer.EntityFrameworkInitializer()
            {
                ConnectionString = "Data Source=(local);Initial Catalog=UserDatabase;Integrated Security=True",
                Provider = "System.Data.SqlClient",
                DefaultContainerName = "UserRepository",
                CSDLResource = "res://*/WhippedCream.Data.User.UserDatabase.csdl",
				SSDLResource = "res://*/WhippedCream.Data.User.UserDatabase.ssdl",
				MSLResource = "res://*/WhippedCream.Data.User.UserDatabase.msl"
            };
        }

        #endregion
    }
}