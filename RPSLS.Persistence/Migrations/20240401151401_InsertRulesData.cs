using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPSLS.Persistence.Migrations
{
    public partial class InsertRulesData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Migrations/InsertedData/ChoicesAndRules.sql");
			migrationBuilder.Sql(File.ReadAllText(sqlFile));
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DELETE FROM [dbo].[Rules];");
			migrationBuilder.Sql("DELETE FROM [dbo].[Choices];");
		}
    }
}
