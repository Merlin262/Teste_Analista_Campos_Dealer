namespace TesteCamposDealer.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class RemoveLegacyId : DbMigration
    {
        public override void Up()
        {
            //
            // =====================================================
            // REMOVE COLUNAS LEGADAS
            // =====================================================
            //

            // CLIENTE
            DropColumn("dbo.Cliente", "idClienteLegado");

            // PRODUTO
            DropColumn("dbo.Produto", "idProdutoLegado");

            // VENDA
            DropColumn("dbo.Venda", "idVendaLegado");
            DropColumn("dbo.Venda", "idClienteLegado");
            DropColumn("dbo.Venda", "idProdutoLegado");
        }

        public override void Down()
        {
            //
            // =====================================================
            // RECRIA COLUNAS LEGADAS
            // =====================================================
            //

            // CLIENTE
            AddColumn("dbo.Cliente", "idClienteLegado", c => c.Int(nullable: false));

            // PRODUTO
            AddColumn("dbo.Produto", "idProdutoLegado", c => c.Int(nullable: false));

            // VENDA
            AddColumn("dbo.Venda", "idProdutoLegado", c => c.Int(nullable: false));
            AddColumn("dbo.Venda", "idClienteLegado", c => c.Int(nullable: false));
            AddColumn("dbo.Venda", "idVendaLegado", c => c.Int(nullable: false));
        }
    }
}
