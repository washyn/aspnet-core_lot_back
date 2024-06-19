﻿using Acme.BookStore.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Washyn.UNAJ.Lot.Data;

public class LotDbContext : AbpDbContext<LotDbContext>
{

    public DbSet<Comision> Comisions { get; set; }
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Grado> Grados { get; set; }
    public DbSet<Rol> Rols { get; set; }
    public DbSet<Docente> Docentes { get; set; }
    public DbSet<Sorteo> Sorteo { get; set; }
    public DbSet<Participante> Participantes { get; set; }
    public LotDbContext(DbContextOptions<LotDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        /* Configure your own entities here */

        builder.Entity<Sorteo>(b =>
        {
            b.ConfigureByConvention();
            b.HasKey(c => new { c.RolId, c.DocenteId });
        });
    }
}
