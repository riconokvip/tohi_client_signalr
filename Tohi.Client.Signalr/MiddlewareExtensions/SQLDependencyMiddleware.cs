namespace Tohi.Client.Signalr.MiddlewareExtensions
{
    public static class SQLDependencyMiddleware
    {
        public static void UseTableDependency<T>(this IApplicationBuilder applicationBuilder, string connectionString) where T : BaseDependency
        {
            try
            {
                var serviceProvider = applicationBuilder.ApplicationServices;
                var service = serviceProvider.GetService<T>();
                service.InitSqlDependency(connectionString);
                Console.WriteLine($"[SQLDependency]: Init trigger to {typeof(T).Name} successfully");
            }
            catch (Exception ex)
            {
                throw new Exception($"[SQLDependency]: exception {ex.Message}");
            }
        }
    }
}
