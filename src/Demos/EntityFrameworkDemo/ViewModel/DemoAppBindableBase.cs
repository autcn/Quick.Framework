using Dapper;
using EntityFrameworkDemo.Database;
using Microsoft.EntityFrameworkCore;
using Quick;
using System;

namespace EntityFrameworkDemo.ViewModel
{
    public class DemoAppBindableBase : EfAppBindableBase<EfDemoDbContext>
    {
    }
}
