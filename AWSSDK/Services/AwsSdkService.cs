using AWSSDK.Common;
using AWSSDK.Interfaces;
using Npgsql;
using System.Data.Common;

namespace AWSSDK.Services
{
    public class AwsSdkService : IAwsSdkService
    {
        public AwsSdkService() { }
        public async Task<Dictionary<string, Dictionary<string, string>>> SummaryCard()
        {
            return await CloudWatchAction.GetSummaryCardAsync();
        }
        public async Task<List<string>> GetAvailableIdentifiersAsync()
        {
            var sumaryCard = await CloudWatchAction.GetSummaryCardAsync();
            var result = sumaryCard.Where(entry => entry.Value["available"] == "yes").Select(entry => entry.Key).ToList();
            return result;
        }

        public async Task<string> CreateDBSnapshotAsync(string dbInstanceIdentifier, string snapshotType)
        {
            return await RDSAction.CreateDBSnapshotAsync(dbInstanceIdentifier, snapshotType);
        }

        public async Task<bool> RestoreDBInstanceFromSnapshot(string dbInstanceIdentifier, string snapshotIdentifier)
        {
            return await RDSAction.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, snapshotIdentifier);
        }

        public async Task<bool> CheckSubdomainExistenceAsync(string subdomainToCheck)
        {
            var exits = await Route53Action.CheckSubdomainExistence(subdomainToCheck);
            return exits;
        }

        public async Task<bool> IsDedicatedTypeAsync(string dbIdentifier)
        {
            var result = await RDSAction.IsDedicatedTypeAsync(dbIdentifier);
            return result;
        }

        public async Task<bool> CheckExitRDS(string dbIdentifier)
        {
            var RDSInfs = await RDSAction.GetRDSInformation();
            if (RDSInfs.ContainsKey(dbIdentifier))
            {
                return true;
            }
            return false;
        }

        public bool DeleteTenantDb(string serverEndpoint, string tennantDB)
        {
            try
            {
                // Replace these values with your actual RDS information
                string username = "postgres";
                string password = "Emr!23456789";
                int port = 5432;
                // Connection string format for SQL Server
                string connectionString = $"Host={serverEndpoint};Port={port};Username={username};Password={password};";

                // Create and open a connection
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Delete database
                        using (DbCommand command = connection.CreateCommand())
                        {
                            command.CommandText = @$"
                                                    DO $$ 
                                                    DECLARE
                                                        pid_list text;
                                                        query_text text;
                                                    BEGIN
                                                        -- Get a comma-separated list of active process IDs (pids) for the specified database
                                                        SELECT string_agg(pid::text, ',') INTO pid_list
                                                        FROM pg_stat_activity
                                                        WHERE datname = '{tennantDB}';

                                                        -- Construct the query to terminate each connection
                                                        query_text := 'SELECT pg_terminate_backend(' || pid_list || ')';

                                                        -- Execute the query to terminate connections
                                                        EXECUTE query_text;
                                                    END $$;";
                            command.CommandText += @$"DROP DATABASE {tennantDB};";
                            command.ExecuteNonQuery();
                        }

                        Console.WriteLine($"Database: {tennantDB} deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: Delete TenantDb {ex.Message}");
                        throw new Exception($"Error: Delete TenantDb {ex.Message}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
