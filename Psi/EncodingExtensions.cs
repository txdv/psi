using System;
using System.Text;

namespace Psi
{
	public static class EncodingEtensions
	{
		public static string GetString(this Encoding enc, ArraySegment<byte> segment)
		{
			if (segment == default(ArraySegment<byte>)) {
				return null;
			}
			return enc.GetString(segment.Array, segment.Offset, segment.Count);
		}
	}
}

