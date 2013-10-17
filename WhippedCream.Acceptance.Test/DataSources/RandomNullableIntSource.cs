using AutoPoco.Engine;
namespace WhippedCream.DataSources
{
    public class RandomNullableIntSource : DatasourceBase<int?>
    {
        public System.Random Random { get; private set; }

        public RandomNullableIntSource()
        {
            Random = new System.Random(0);
        }

        public override int? Next(IGenerationContext context)
        {
            if (Random.Next(0, 1) == 1)
                return null;
            else
                return Random.Next();
        }
    }
}