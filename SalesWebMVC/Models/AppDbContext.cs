/**using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;

namespace SalesWebMVC.Models
{
public class AppDbContext : DbContext
{
    public DbSet<Department> Departments { get; set; }

protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
{
    //optionBuilder.UseMySql("server = localhost;database = saleswebmvcappdb;uid=root;pwd=c3por2d2;", serverVersion);
}
    }
}
*/