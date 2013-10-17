using System;
using System.Linq.Expressions;
namespace WhippedCream
{
	/// <summary>
	/// Extension Methods for <see cref="IRepositoryContext"/> types.
	/// </summary>
	public static class RepositoryContextExtensions
	{
		/// <summary>
		/// Loads up an entity relationship based on an Expression.
		/// 
		/// <remarks>
		///		<![CDATA[
		///		The <paramref name="selector"/> must be in the form (entity) = entity.Property
		///		]]>
		///		This method is simply here to abstract the <see cref="M:IRepositoryContext.LoadProperty"/> method
		///		and allow the developer to specify a property to load from an expression instead of a string.
		/// </remarks>
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity whos property needs to be loaded.</typeparam>
		/// <param name="context">The <see cref="IRepositoryContext"/>.</param>
		/// <param name="entity">The entity whos property needs to be loaded.</param>
		/// <param name="selector">The selected designating the property to load.</param>
		/// <exception cref="System.ArgumentNullException">Thrown when <paramref name="selector"/> is null.</exception>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="selector"/> is not in the correct form.  See Remarks.</exception>
		public static void LoadProperty<TEntity>(this IRepositoryContext context, TEntity entity, Expression<Func<TEntity, object>> selector)
		{
			if (selector == null)
				throw new System.ArgumentNullException("selector");

			if (!(selector.Body is MemberExpression))
				throw new System.ArgumentException("You must supply a selector of the form (entity) => entity.Property");

			context.LoadProperty(entity, (selector.Body as MemberExpression).Member.Name);
		}
	}
}