using System;
using AutoPoco.Engine;
namespace WhippedCream.DataSources
{
    public class IdSameAsParentSource<TParent, TId> : DatasourceBase<TId>
        where TParent : class
    {
        public Func<TParent, TId> IdExpression { get; private set; }

        public IdSameAsParentSource(Func<TParent, TId> ex)
        {
            IdExpression = ex;
        }

        public override TId Next(IGenerationContext context)
        {
            TParent parent = FindParent(context.Node);
            if (parent == null)
                return default(TId);
            else
                return IdExpression(parent);
        }

        private TParent FindParent(IGenerationContextNode node)
        {
            if (node == null)
                return null;

            TypeGenerationContextNode tgNode = node as TypeGenerationContextNode;
            if (tgNode == null)
                return FindParent(node.Parent);
            else
            {
                if (tgNode.Target is TParent)
                    return (TParent)tgNode.Target;
                else
                    return FindParent(node.Parent);
            }
        }
    }
}