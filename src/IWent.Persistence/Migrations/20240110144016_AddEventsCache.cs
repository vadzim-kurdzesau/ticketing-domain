using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IWent.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEventsCache : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
                    CREATE TABLE [dbo].[EventsCache] (
                        [Id] [nvarchar](449) NOT NULL,
                        [Value] [varbinary](max) NOT NULL,
                        [ExpiresAtTime] [datetimeoffset](7) NOT NULL,
                        [SlidingExpirationInSeconds] [bigint] NULL,
                        [AbsoluteExpiration] [datetimeoffset](7) NULL,
                    PRIMARY KEY CLUSTERED ([Id] ASC) 
                        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
                        ON [PRIMARY]) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE [dbo].[EventsCache]");
        }
    }
}
