namespace TesteCamposDealer.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class FixVlrTotalToDec : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.Venda ALTER COLUMN vlrTotal DECIMAL(18, 2) NOT NULL");
        }

        public override void Down()
        {
            Sql("ALTER TABLE dbo.Venda ALTER COLUMN vlrTotal FLOAT NOT NULL");
        }

    }
}
