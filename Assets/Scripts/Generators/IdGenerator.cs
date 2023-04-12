namespace Generators
{
    public class IdGenerator
    {
        private static int _cupId = 0;

        public static int GetCupId()
        {
            return ++_cupId;
        }
    }
}
