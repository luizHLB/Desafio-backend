using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.Data.Migrations
{
    /// <inheritdoc />
    public partial class PlanSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" INSERT INTO ""Plan"" (""Name"", ""Period"", ""Price"", ""Fine"", ""Extra"", ""CreatedBy"", ""CreatedAt"") VALUES
                                    ('7 dias', 7, 30, 20, 50, 0 , NOW()),
                                    ('15 dias', 15, 28, 40, 50, 0, NOW()),
                                    ('30 dias', 30, 22, 0, 50, 0, NOW()),
                                    ('45 dias', 45, 20, 0, 50, 0, NOW()),
                                    ('50 dias', 50, 18, 0, 50, 0, NOW());
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
