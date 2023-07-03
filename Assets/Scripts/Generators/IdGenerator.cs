namespace Generators
{
    public class IdGenerator
    {
        private static int _cupId;

        public static int GetCupId()
        {
            return ++_cupId;
        }
    }
}
