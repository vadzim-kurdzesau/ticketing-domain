using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IWent.Persistence.Models;
using IWent.Persistence.Repositories.ConnectionProviders;

namespace IWent.Persistence.Repositories;

public class VenueRepository : IVenueRepository
{
    private readonly IDbConnectionProvider _connectionProvider;

    public VenueRepository(IDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task CreateAsync(Venue newVenue)
    {
        const string query = "dbo.sp_venues_insert";

        var parameters = new DynamicParameters();
        parameters.Add("Name", newVenue.Name, DbType.String);
        parameters.Add("Country", newVenue.Country, DbType.String);
        parameters.Add("Region", newVenue.Region, DbType.String);
        parameters.Add("City", newVenue.City, DbType.String);
        parameters.Add("Street", newVenue.Street, DbType.String);
        parameters.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

        using var connection = _connectionProvider.InitializeConnection();
        await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
        newVenue.Id = parameters.Get<int>("Id");
    }

    public Task DeleteAsync(int id)
    {
        const string query = "DELETE FROM venues WHERE id = @Id";

        using var connection = _connectionProvider.InitializeConnection();
        return connection.ExecuteAsync(query, new { id });
    }

    public Task<IEnumerable<Venue>> GetAllAsync()
    {
        const string query = "SELECT * FROM venues";

        using var connection = _connectionProvider.InitializeConnection();
        return connection.QueryAsync<Venue>(query);
    }

    public async Task<Venue?> GetAsync(int id)
    {
        const string query = "dbo.sp_venues_get_by_id";

        using var connection = _connectionProvider.InitializeConnection();

        //var result = await connection.QueryAsync<Venue, Section, Row, Seat, Venue>(query, (venue, section, row, seat) =>
        //{
        //    sections.Add(section);
        //    rows.Add(row);
        //    seats.Add(seat);
        //    return venue;
        //}, splitOn: "");

        var venueDictionary = new Dictionary<int, Venue>();
        var sectionDictionary = new Dictionary<int, Section>();
        var rowDictionary = new Dictionary<int, Dictionary<int, Row>>();

        var joinRows = await connection.QueryAsync<Venue, Section, Row, Seat, Venue>("sp_venues_get_by_id",
            (venue, section, row, seat) =>
            {
                Row rowEntry = null;
                if (!venueDictionary.TryGetValue(venue.Id, out var venueEntry))
                {
                    venueEntry = venue;
                    venueDictionary.Add(venueEntry.Id, venueEntry);
                }

                if (section != null && !sectionDictionary.TryGetValue(section.Id, out var sectionEntry))
                {
                    sectionEntry = section;
                    sectionDictionary.Add(sectionEntry.Id, sectionEntry);
                }

                if (row != null && section != null)
                {
                    if (!rowDictionary.TryGetValue(section.Id, out var rows))
                    {
                        rows = new Dictionary<int, Row>();
                        rowDictionary.Add(section.Id, rows);
                    }

                    rowEntry = row;
                    rows.TryAdd(row.Id, row);
                }

                if (seat != null)
                {
                    rowEntry.Seats.Add(seat);
                }

                return venueEntry;
            },
            new { Id = id },
            splitOn: "Id,Id,Id",
            commandType: CommandType.StoredProcedure);

        var venue = joinRows.FirstOrDefault();
        if (venue != null)
        {
            venue.Manifest = sectionDictionary.Values;
        }

        return venue;
    }

    public Task UpdateAsync(Venue update)
    {
        const string query = " UPDATE venues" +
                                " SET name = @Name, country = @Country, region = @Region, city = @City, street = @Street" +
                                " WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("Id", update.Id, DbType.Int32);
        parameters.Add("Name", update.Name, DbType.String);
        parameters.Add("Country", update.Country, DbType.String);
        parameters.Add("Region", update.Region, DbType.String);
        parameters.Add("City", update.City, DbType.String);
        parameters.Add("Street", update.Street, DbType.String);

        using var connection = _connectionProvider.InitializeConnection();
        return connection.ExecuteAsync(query, parameters);
    }
}
