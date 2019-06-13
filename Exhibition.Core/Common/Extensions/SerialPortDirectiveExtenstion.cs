

namespace Exhibition.Core
{
    using System.Linq;
    public static class SerialPortDirectiveExtenstion
    {
        public static byte[] ToHexBytes(this string text)
        {
            return text.Split(' ').Select((ctx) =>
            {
                return byte.Parse(ctx, System.Globalization.NumberStyles.HexNumber);
            }).ToArray();
        }
    }
}
