
using CamelRegistry.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CamelRegistry.Api.Data;

public class CamelRegistryContext(DbContextOptions<CamelRegistryContext> options) : DbContext(options)
{
    public DbSet<Camel> Camels => Set<Camel>();
}
