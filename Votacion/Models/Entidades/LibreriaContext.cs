using Microsoft.EntityFrameworkCore;
using Votacion.Models.Entidades;

namespace Votacion.Models
{
    public class LibreriaContext : DbContext
    {
        //opciones de get set
        // Constructor protegido sin parámetros
        public LibreriaContext()
        {

        }
        //opciones de la base de datos
        public LibreriaContext(DbContextOptions<LibreriaContext> options) : base(options)
        {

        }
        //debset llama alas entidades 
        public DbSet<Candidato> Candidatos { get; set; }
        public DbSet<Eleccion> Elecciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Votaciones> Votaciones { get; set; }
        public DbSet<Votante> Votantes { get; set; }


        //modelo de creacion 
        //metodo para conectar la base de datos con el visual studio
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
    }
}

