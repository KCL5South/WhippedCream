using AutoPoco.Engine;
namespace WhippedCream.DataSources
{
    public class RandomDateTimeSource : DatasourceBase<System.DateTime>
    {
        public System.Random Random { get; private set; }

        public RandomDateTimeSource()
        {
            Random = new System.Random(0);
        }

        public override System.DateTime Next(IGenerationContext context)
        {
            return new System.DateTime(Random.Next(1900, 10000),
                                        Random.Next(1, 13),
                                        Random.Next(1, 21),
                                        Random.Next(0, 24),
                                        Random.Next(0, 60),
                                        Random.Next(0, 60),
                                        Random.Next(0, 1000));
        }
    }
}