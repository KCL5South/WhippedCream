using AutoPoco.Engine;
namespace WhippedCream.DataSources
{
    public class RandomByteArraySource : DatasourceBase<byte[]>
    {
        public System.Random Random { get; private set; }
        public int MinLength { get; private set; }
        public int MaxLength { get; private set; }

        public RandomByteArraySource(int minLength, int maxLength)
        {
            Random = new System.Random(0);
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public override byte[] Next(IGenerationContext context)
        {
            int length = Random.Next(MinLength, MaxLength);
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
                result[i] = (byte)Random.Next((int)System.Byte.MaxValue);

            return result;
        }
    }
}