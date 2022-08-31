using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace linqPlusPlus
{
    public static class EFCoreExtensions
    {
        public static bool IsBeingTracked<TContext, TEntity>(this TContext context, TEntity entity)
            where TContext : DbContext
            where TEntity : class
        {
            return context.Set<TEntity>().Local.Any(e => e == entity);
        }

        public static bool SafeAttach<TContext, TEntity>(this TContext context, TEntity entity)
            where TContext : DbContext
            where TEntity : class
        {
            return context.IsBeingTracked(entity) ? true : context.Attach(entity) is not null;
        }
        public static IQueryable<TEntity> Track<TEntity>(this IQueryable<TEntity> query, bool track = true) where TEntity : class => track ? query : query.AsNoTracking();
    }
}
