using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckSieve.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new []{ "Id", "Album", "Artist", "ListensCount", "ReleaseYear", "Title" },
                values: new object[,]
                {
                    { Guid.NewGuid(), "Nevermind", "Nirvana", 1_000_000ul, 1994, "Smels Like Teen Spirit" },
                    { Guid.NewGuid(), "Cruel Summer", "Taylor Swift", 100_000ul, 2023, "Cruel Summer" },
                    { Guid.NewGuid(), "DAMN.", "Kendrick Lamar", 2_000_000ul, 2018, "HUMBLE." },
                    { Guid.NewGuid(), "Something in the Orange", "OUR LAST NIGHT", 40_000ul, 2022, "Something in the Orange" },
                    { Guid.NewGuid(), "The Spark", "Enter Shikari", 567_000ul, 2017, "The Spark" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
