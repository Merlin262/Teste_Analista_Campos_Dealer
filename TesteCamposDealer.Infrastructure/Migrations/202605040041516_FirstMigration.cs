namespace TesteCamposDealer.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            //
            // =====================================================
            // REMOVE FKS LEGADAS
            // =====================================================
            //

            DropForeignKey("dbo.Venda", "FK_Venda_Cliente");
            DropForeignKey("dbo.Venda", "FK_Venda_Produto");

            DropIndex("dbo.Venda", new[] { "idCliente" });
            DropIndex("dbo.Venda", new[] { "idProduto" });

            //
            // =====================================================
            // CLIENTE
            // =====================================================
            //

            RenameColumn("dbo.Cliente", "idCliente", "idClienteLegado");

            AddColumn(
                "dbo.Cliente",
                "idCliente",
                c => c.Guid(nullable: false, defaultValueSql: "NEWID()"));

            AlterColumn("dbo.Cliente", "nomeCliente", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Cliente", "endereco", c => c.String(nullable: false, maxLength: 30));

            DropPrimaryKey("dbo.Cliente", "PK_Cliente");
            AddPrimaryKey("dbo.Cliente", "idCliente");

            //
            // =====================================================
            // PRODUTO
            // =====================================================
            //

            RenameColumn("dbo.Produto", "idProduto", "idProdutoLegado");

            AddColumn(
                "dbo.Produto",
                "idProduto",
                c => c.Guid(nullable: false, defaultValueSql: "NEWID()"));

            AddColumn(
                "dbo.Produto",
                "vlrProduto",
                c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));

            AlterColumn("dbo.Produto", "dscProduto", c => c.String(nullable: false, maxLength: 100));

            DropPrimaryKey("dbo.Produto", "PK_Produto");
            AddPrimaryKey("dbo.Produto", "idProduto");

            //
            // =====================================================
            // VENDA
            // =====================================================
            //

            RenameColumn("dbo.Venda", "idVenda", "idVendaLegado");
            RenameColumn("dbo.Venda", "idCliente", "idClienteLegado");
            RenameColumn("dbo.Venda", "idProduto", "idProdutoLegado");
            RenameColumn("dbo.Venda", "vlrProduto", "vlrTotal");

            AddColumn(
                "dbo.Venda",
                "idVenda",
                c => c.Guid(nullable: false, defaultValueSql: "NEWID()"));

            AddColumn(
                "dbo.Venda",
                "idCliente",
                c => c.Guid(nullable: false, defaultValue: Guid.Empty));

            AlterColumn(
                "dbo.Venda",
                "dthRegistro",
                c => c.DateTime(nullable: false));

            DropPrimaryKey("dbo.Venda", "PK_Venda");
            AddPrimaryKey("dbo.Venda", "idVenda");

            //
            // PREENCHE FK CLIENTE GUID
            //

            Sql(@"
            UPDATE V
               SET V.idCliente = C.idCliente
              FROM Venda V
              INNER JOIN Cliente C
                 ON V.idClienteLegado = C.idClienteLegado
        ");

            CreateIndex("dbo.Venda", "idCliente");

            AddForeignKey(
                "dbo.Venda",
                "idCliente",
                "dbo.Cliente",
                "idCliente");

            //
            // =====================================================
            // VENDA ITEM
            // =====================================================
            //

            CreateTable(
                "dbo.VendaItem",
                c => new
                {
                    idVendaItem = c.Guid(nullable: false, defaultValueSql: "NEWID()"),
                    idVenda = c.Guid(nullable: false),
                    idProduto = c.Guid(nullable: false),
                    quantidade = c.Int(nullable: false),
                    vlrUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vlrTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.idVendaItem)
                .ForeignKey("dbo.Venda", t => t.idVenda, cascadeDelete: true)
                .ForeignKey("dbo.Produto", t => t.idProduto)
                .Index(t => t.idVenda)
                .Index(t => t.idProduto);

            //
            // MIGRA DADOS ANTIGOS VENDA -> VENDAITEM
            //

            Sql(@"
            INSERT INTO VendaItem
            (
                idVendaItem,
                idVenda,
                idProduto,
                quantidade,
                vlrUnitario,
                vlrTotal
            )
            SELECT
                NEWID(),
                V.idVenda,
                P.idProduto,
                1,
                V.vlrTotal,
                V.vlrTotal
            FROM Venda V
            INNER JOIN Produto P
                ON V.idProdutoLegado = P.idProdutoLegado
        ");

            //
            // =====================================================
            // HISTÓRICO DE PREÇO
            // =====================================================
            //

            CreateTable(
                "dbo.ProdutoPrecoHistorico",
                c => new
                {
                    idHistorico = c.Guid(nullable: false, defaultValueSql: "NEWID()"),
                    idProduto = c.Guid(nullable: false),
                    vlrAnterior = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vlrNovo = c.Decimal(nullable: false, precision: 18, scale: 2),
                    dthAlteracao = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.idHistorico)
                .ForeignKey("dbo.Produto", t => t.idProduto, cascadeDelete: true)
                .Index(t => t.idProduto);
        }

        public override void Down()
        {
            DropForeignKey("dbo.ProdutoPrecoHistorico", "idProduto", "dbo.Produto");
            DropForeignKey("dbo.VendaItem", "idProduto", "dbo.Produto");
            DropForeignKey("dbo.VendaItem", "idVenda", "dbo.Venda");
            DropForeignKey("dbo.Venda", "idCliente", "dbo.Cliente");

            DropIndex("dbo.ProdutoPrecoHistorico", new[] { "idProduto" });
            DropIndex("dbo.VendaItem", new[] { "idProduto" });
            DropIndex("dbo.VendaItem", new[] { "idVenda" });
            DropIndex("dbo.Venda", new[] { "idCliente" });

            DropTable("dbo.ProdutoPrecoHistorico");
            DropTable("dbo.VendaItem");

            DropPrimaryKey("dbo.Venda");
            DropPrimaryKey("dbo.Produto");
            DropPrimaryKey("dbo.Cliente");

            DropColumn("dbo.Venda", "idCliente");
            DropColumn("dbo.Venda", "idVenda");

            DropColumn("dbo.Produto", "vlrProduto");
            DropColumn("dbo.Produto", "idProduto");

            DropColumn("dbo.Cliente", "idCliente");
        }
    }
}
