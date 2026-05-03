using Medo;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("TesteCamposDealerConnectionString")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public override int SaveChanges()
        {
            PrepareNewEntries();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            PrepareNewEntries();
            return base.SaveChangesAsync(cancellationToken);
        }


        private void PrepareNewEntries()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
            {
                var type = entry.Entity.GetType();
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // Assign UUID v7 to any unset Guid PK
                var keyProp = props.FirstOrDefault(p =>
                    p.PropertyType == typeof(Guid) &&
                    Attribute.IsDefined(p, typeof(KeyAttribute)) &&
                    !Attribute.IsDefined(p, typeof(ForeignKeyAttribute)));

                if (keyProp != null && (Guid)keyProp.GetValue(entry.Entity) == Guid.Empty)
                    keyProp.SetValue(entry.Entity, Uuid7.NewMsSqlUniqueIdentifier());

                // Auto-set any DateTime property that was not explicitly assigned
                foreach (var dateProp in props.Where(p => p.PropertyType == typeof(DateTime) && p.CanWrite))
                {
                    if ((DateTime)dateProp.GetValue(entry.Entity) == default(DateTime))
                        dateProp.SetValue(entry.Entity, DateTime.Now);
                }
            }
        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Venda> Venda { get; set; }
        public virtual DbSet<VendaItem> VendaItens { get; set; }
        public virtual DbSet<ProdutoPrecoHistorico> ProdutoPrecoHistoricos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<decimal>().Configure(c => c.HasPrecision(18, 2));

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Vendas)
                .WithRequired(v => v.Cliente)
                .HasForeignKey(v => v.idCliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Venda>()
                .HasMany(v => v.Itens)
                .WithRequired(i => i.Venda)
                .HasForeignKey(i => i.idVenda)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Produto>()
                .HasMany(p => p.VendaItens)
                .WithRequired(i => i.Produto)
                .HasForeignKey(i => i.idProduto)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Produto>()
                .HasMany(p => p.Historico)
                .WithRequired(h => h.Produto)
                .HasForeignKey(h => h.idProduto)
                .WillCascadeOnDelete(true);
        }
    }
}
