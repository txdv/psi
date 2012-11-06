
#line 1 "RawParser.rl"
using System;
using System.Collections.Generic;
using System.Text;

namespace Psi
{
	public class RawParser
	{
		ArraySegment<byte> value;
		ArraySegment<byte> target;

		int tmp1 = 0;
		int tmp2 = 0;
		int tmp3 = 0;
		int tmp4 = 0;
		int tmp5 = 0;
		int tmp6 = 0;

		int value_start;

		public static int Number(ArraySegment<byte> arr)
		{
			return Number(arr.Array, arr.Offset, arr.Count);
		}

		public static int Number(byte[] data, int start, int count)
		{
			int res = 0;
			for (int i = 0; i < count; i++) {
				res *= 10;
				res += (data[start + i] - '0');
			}
			return res;
		}

		
#line 256 "RawParser.rl"


		public event Action<DateTime> DateTime;

		public event Action<ArraySegment<byte>, ArraySegment<byte>> Option;

		#region Log Message Types

		public event Action<ArraySegment<byte>, ArraySegment<byte>> Meta;

		public event Action LogFileStart;
		public event Action LogFileEnd;

		public event Action ServerCVarsStart;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> ServerCVarSet;
		public event Action ServerCVarsEnd;

		public event Action<ArraySegment<byte>> LoadingMap;
		public event Action<ArraySegment<byte>> StartedMap;

		public event Action<ArraySegment<byte>> WorldTrigger;

		public event Action<ArraySegment<byte>, ArraySegment<byte>> TeamTrigger;
		public event Action<ArraySegment<byte>, int, int> TeamScore;

		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerConnect;
		public event Action<ArraySegment<byte>> PlayerDisconnect;
		public event Action<ArraySegment<byte>> PlayerEnterGame;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerJoinTeam;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerTrigger;
		public event Action<ArraySegment<byte>, ArraySegment<byte>, ArraySegment<byte>> PlayerTriggerAgainst;
		public event Action<ArraySegment<byte>, ArraySegment<byte>, ArraySegment<byte>> PlayerAttack;
		public event Action<ArraySegment<byte>, ArraySegment<byte>, ArraySegment<byte>> PlayerKill;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerSay;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerSayTeam;
		public event Action<ArraySegment<byte>> PlayerValidate;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerNameChange;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerSuicide;

		#endregion

		public event Action End;

		int cs;

		
#line 87 "RawParser.cs"
static readonly sbyte[] _psi_actions =  new sbyte [] {
	0, 1, 0, 1, 1, 1, 2, 1, 
	4, 1, 5, 1, 6, 1, 7, 1, 
	8, 1, 9, 1, 10, 1, 11, 1, 
	12, 1, 13, 1, 14, 1, 15, 1, 
	16, 1, 17, 1, 18, 1, 19, 1, 
	22, 1, 23, 1, 24, 1, 25, 1, 
	26, 1, 27, 1, 28, 1, 29, 1, 
	30, 1, 31, 1, 32, 1, 34, 1, 
	35, 1, 36, 1, 37, 1, 39, 1, 
	40, 1, 42, 1, 43, 1, 46, 1, 
	47, 1, 48, 1, 54, 1, 57, 2, 
	1, 2, 2, 4, 5, 2, 9, 10, 
	2, 15, 16, 2, 17, 18, 2, 20, 
	21, 2, 22, 23, 2, 24, 25, 2, 
	27, 28, 2, 29, 30, 2, 33, 38, 
	2, 33, 41, 2, 33, 45, 2, 33, 
	51, 2, 33, 52, 2, 33, 53, 2, 
	33, 55, 2, 33, 56, 2, 34, 35, 
	2, 36, 37, 2, 44, 45, 2, 46, 
	47, 2, 49, 50, 3, 2, 3, 6, 
	3, 19, 20, 21, 3, 32, 33, 38, 
	3, 32, 33, 41, 3, 32, 33, 45, 
	3, 32, 33, 51, 3, 32, 33, 52, 
	3, 32, 33, 53, 3, 32, 33, 55, 
	3, 32, 33, 56, 3, 43, 44, 45, 
	3, 48, 49, 50, 4, 1, 2, 3, 
	6
};

static readonly short[] _psi_key_offsets =  new short [] {
	0, 0, 1, 2, 4, 6, 7, 9, 
	11, 12, 14, 16, 18, 20, 21, 22, 
	23, 25, 27, 28, 30, 32, 33, 35, 
	37, 38, 39, 45, 46, 47, 48, 57, 
	58, 59, 60, 61, 62, 63, 64, 65, 
	66, 67, 68, 69, 70, 71, 72, 73, 
	74, 75, 76, 77, 78, 79, 81, 83, 
	84, 85, 86, 87, 89, 90, 91, 92, 
	93, 94, 95, 96, 97, 98, 99, 100, 
	101, 102, 103, 104, 105, 106, 107, 108, 
	109, 111, 112, 113, 114, 115, 116, 117, 
	118, 119, 120, 121, 122, 123, 124, 125, 
	126, 127, 128, 130, 131, 132, 133, 134, 
	135, 136, 137, 138, 139, 140, 141, 142, 
	143, 144, 145, 146, 147, 148, 149, 150, 
	151, 152, 153, 154, 155, 156, 157, 158, 
	159, 160, 161, 162, 163, 164, 165, 166, 
	167, 168, 169, 170, 171, 172, 173, 174, 
	175, 176, 177, 178, 179, 180, 181, 182, 
	183, 184, 185, 186, 187, 188, 189, 190, 
	191, 192, 193, 194, 195, 196, 197, 198, 
	199, 200, 201, 202, 203, 204, 205, 206, 
	207, 208, 209, 210, 211, 212, 213, 214, 
	215, 216, 217, 218, 219, 220, 221, 222, 
	223, 224, 225, 226, 227, 228, 229, 230, 
	231, 232, 234, 235, 236, 237, 238, 239, 
	240, 241, 242, 243, 244, 245, 246, 247, 
	248, 249, 250, 251, 252, 253, 254, 255, 
	256, 257, 259, 260, 261, 262, 263, 264, 
	265, 266, 267, 268, 269, 270, 272, 273, 
	274, 275, 276, 277, 278, 279, 280, 281, 
	282, 283, 284, 285, 286, 287, 288, 289, 
	290, 292, 293, 294, 295, 296, 297, 298, 
	299, 300, 301, 302, 303, 305, 306, 307, 
	308, 309, 310, 311, 312, 313, 314, 316, 
	317, 318, 319, 320, 321, 322, 323, 324, 
	326, 327, 328, 329, 330, 331, 332, 333, 
	334, 335, 336, 337, 338, 339, 340, 341, 
	342, 343, 344, 345, 346, 347, 348, 349, 
	350, 351, 352, 353, 355, 356, 357, 358, 
	359, 360, 361, 362, 365, 366, 367, 368, 
	369, 370, 371, 372, 375, 376, 377, 378, 
	379, 380, 381, 382, 383, 386, 389, 390, 
	391, 392, 393, 394, 395, 396, 397, 398, 
	399, 400, 401, 402, 403, 404, 405, 406, 
	407, 408, 409, 410, 411, 412, 413, 414, 
	415, 416, 417, 418, 419, 420, 421, 422, 
	423, 424, 425, 427, 429, 431, 432, 434, 
	435, 437, 440, 443, 446
};

static readonly char[] _psi_trans_keys =  new char [] {
	'\u004c', '\u0020', '\u0030', '\u0039', '\u0030', '\u0039', '\u002f', '\u0030', 
	'\u0039', '\u0030', '\u0039', '\u002f', '\u0030', '\u0039', '\u0030', '\u0039', 
	'\u0030', '\u0039', '\u0030', '\u0039', '\u0020', '\u002d', '\u0020', '\u0030', 
	'\u0039', '\u0030', '\u0039', '\u003a', '\u0030', '\u0039', '\u0030', '\u0039', 
	'\u003a', '\u0030', '\u0039', '\u0030', '\u0039', '\u003a', '\u0020', '\u0022', 
	'\u004c', '\u0053', '\u0054', '\u0057', '\u005b', '\u0022', '\u0022', '\u0020', 
	'\u0053', '\u0061', '\u0063', '\u0064', '\u0065', '\u006a', '\u006b', '\u0073', 
	'\u0074', '\u0054', '\u0045', '\u0041', '\u004d', '\u0020', '\u0055', '\u0053', 
	'\u0045', '\u0052', '\u0049', '\u0044', '\u0020', '\u0076', '\u0061', '\u006c', 
	'\u0069', '\u0064', '\u0061', '\u0074', '\u0065', '\u0064', '\u0028', '\u0020', 
	'\u0029', '\u0020', '\u0029', '\u0022', '\u0022', '\u0022', '\u0029', '\u0022', 
	'\u0028', '\u0074', '\u0074', '\u0061', '\u0063', '\u006b', '\u0065', '\u0064', 
	'\u0020', '\u0022', '\u0022', '\u0022', '\u0020', '\u0077', '\u0069', '\u0074', 
	'\u0068', '\u0020', '\u0022', '\u0022', '\u0022', '\u0068', '\u006f', '\u0061', 
	'\u006e', '\u0067', '\u0065', '\u0064', '\u0020', '\u006e', '\u0061', '\u006d', 
	'\u0065', '\u0020', '\u0074', '\u006f', '\u0020', '\u0022', '\u0022', '\u0022', 
	'\u006d', '\u006e', '\u006d', '\u0069', '\u0074', '\u0074', '\u0065', '\u0064', 
	'\u0020', '\u0073', '\u0075', '\u0069', '\u0063', '\u0069', '\u0064', '\u0065', 
	'\u0020', '\u0077', '\u0069', '\u0074', '\u0068', '\u0020', '\u0022', '\u0022', 
	'\u0022', '\u006e', '\u0065', '\u0063', '\u0074', '\u0065', '\u0064', '\u002c', 
	'\u0020', '\u0061', '\u0064', '\u0064', '\u0072', '\u0065', '\u0073', '\u0073', 
	'\u0020', '\u0022', '\u0022', '\u0022', '\u0069', '\u0073', '\u0063', '\u006f', 
	'\u006e', '\u006e', '\u0065', '\u0063', '\u0074', '\u0065', '\u0064', '\u006e', 
	'\u0074', '\u0065', '\u0072', '\u0065', '\u0064', '\u0020', '\u0074', '\u0068', 
	'\u0065', '\u0020', '\u0067', '\u0061', '\u006d', '\u0065', '\u006f', '\u0069', 
	'\u006e', '\u0065', '\u0064', '\u0020', '\u0074', '\u0065', '\u0061', '\u006d', 
	'\u0020', '\u0022', '\u0022', '\u0022', '\u0069', '\u006c', '\u006c', '\u0065', 
	'\u0064', '\u0020', '\u0022', '\u0022', '\u0022', '\u0020', '\u0077', '\u0069', 
	'\u0074', '\u0068', '\u0020', '\u0022', '\u0022', '\u0022', '\u0061', '\u0079', 
	'\u0020', '\u005f', '\u0022', '\u0022', '\u0022', '\u0074', '\u0065', '\u0061', 
	'\u006d', '\u0020', '\u0022', '\u0022', '\u0022', '\u0072', '\u0069', '\u0067', 
	'\u0067', '\u0065', '\u0072', '\u0065', '\u0064', '\u0020', '\u0022', '\u0022', 
	'\u0022', '\u0028', '\u0061', '\u0067', '\u0061', '\u0069', '\u006e', '\u0073', 
	'\u0074', '\u0020', '\u0022', '\u0022', '\u0022', '\u006f', '\u0061', '\u0067', 
	'\u0064', '\u0069', '\u006e', '\u0067', '\u0020', '\u006d', '\u0061', '\u0070', 
	'\u0020', '\u0022', '\u0022', '\u0022', '\u0020', '\u0066', '\u0069', '\u006c', 
	'\u0065', '\u0020', '\u0063', '\u0073', '\u006c', '\u006f', '\u0073', '\u0065', 
	'\u0064', '\u0074', '\u0061', '\u0072', '\u0074', '\u0065', '\u0064', '\u0065', 
	'\u0074', '\u0072', '\u0076', '\u0065', '\u0072', '\u0020', '\u0063', '\u0076', 
	'\u0061', '\u0072', '\u0020', '\u0073', '\u0022', '\u0022', '\u0020', '\u003d', 
	'\u0020', '\u0022', '\u0022', '\u0020', '\u0065', '\u0073', '\u006e', '\u0064', 
	'\u0074', '\u0061', '\u0072', '\u0074', '\u0061', '\u0072', '\u0074', '\u0065', 
	'\u0064', '\u0020', '\u006d', '\u0061', '\u0070', '\u0020', '\u0022', '\u0022', 
	'\u0022', '\u0065', '\u0061', '\u006d', '\u0020', '\u0022', '\u0022', '\u0022', 
	'\u0020', '\u0073', '\u0074', '\u0063', '\u006f', '\u0072', '\u0065', '\u0064', 
	'\u0020', '\u0022', '\u0022', '\u0030', '\u0039', '\u0020', '\u0077', '\u0069', 
	'\u0074', '\u0068', '\u0020', '\u0022', '\u0022', '\u0030', '\u0039', '\u0020', 
	'\u0070', '\u006c', '\u0061', '\u0079', '\u0065', '\u0072', '\u0073', '\u0022', 
	'\u0030', '\u0039', '\u0022', '\u0030', '\u0039', '\u0072', '\u0069', '\u0067', 
	'\u0067', '\u0065', '\u0072', '\u0065', '\u0064', '\u0020', '\u0022', '\u0022', 
	'\u0022', '\u006f', '\u0072', '\u006c', '\u0064', '\u0020', '\u0074', '\u0072', 
	'\u0069', '\u0067', '\u0067', '\u0065', '\u0072', '\u0065', '\u0064', '\u0020', 
	'\u0022', '\u0022', '\u0022', '\u004d', '\u0045', '\u0054', '\u0041', '\u005d', 
	'\u0020', '\u0020', '\u003a', '\u0020', '\u003a', '\u0020', '\u003a', '\u0020', 
	'\u0020', '\u0029', '\u0020', '\u0020', '\u0022', '\u0020', '\u0022', '\u0028', 
	'\u0020', '\u0022', '\u0029', '\u0020', '\u0022', '\u0029', '\u0020', '\u0022', 
	'\u0028', (char) 0
};

static readonly sbyte[] _psi_single_lengths =  new sbyte [] {
	0, 1, 1, 0, 0, 1, 0, 0, 
	1, 0, 0, 0, 0, 1, 1, 1, 
	0, 0, 1, 0, 0, 1, 0, 0, 
	1, 1, 6, 1, 1, 1, 9, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 2, 2, 1, 
	1, 1, 1, 2, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	2, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 2, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 2, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 2, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 2, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	2, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 2, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 2, 1, 
	1, 1, 1, 1, 1, 1, 1, 2, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 2, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 2, 2, 2, 1, 2, 1, 
	2, 3, 3, 3, 3
};

static readonly sbyte[] _psi_range_lengths =  new sbyte [] {
	0, 0, 0, 1, 1, 0, 1, 1, 
	0, 1, 1, 1, 1, 0, 0, 0, 
	1, 1, 0, 1, 1, 0, 1, 1, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 1, 0, 0, 0, 0, 
	0, 0, 0, 1, 0, 0, 0, 0, 
	0, 0, 0, 0, 1, 1, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0
};

static readonly short[] _psi_index_offsets =  new short [] {
	0, 0, 2, 4, 6, 8, 10, 12, 
	14, 16, 18, 20, 22, 24, 26, 28, 
	30, 32, 34, 36, 38, 40, 42, 44, 
	46, 48, 50, 57, 59, 61, 63, 73, 
	75, 77, 79, 81, 83, 85, 87, 89, 
	91, 93, 95, 97, 99, 101, 103, 105, 
	107, 109, 111, 113, 115, 117, 120, 123, 
	125, 127, 129, 131, 134, 136, 138, 140, 
	142, 144, 146, 148, 150, 152, 154, 156, 
	158, 160, 162, 164, 166, 168, 170, 172, 
	174, 177, 179, 181, 183, 185, 187, 189, 
	191, 193, 195, 197, 199, 201, 203, 205, 
	207, 209, 211, 214, 216, 218, 220, 222, 
	224, 226, 228, 230, 232, 234, 236, 238, 
	240, 242, 244, 246, 248, 250, 252, 254, 
	256, 258, 260, 262, 264, 266, 268, 270, 
	272, 274, 276, 278, 280, 282, 284, 286, 
	288, 290, 292, 294, 296, 298, 300, 302, 
	304, 306, 308, 310, 312, 314, 316, 318, 
	320, 322, 324, 326, 328, 330, 332, 334, 
	336, 338, 340, 342, 344, 346, 348, 350, 
	352, 354, 356, 358, 360, 362, 364, 366, 
	368, 370, 372, 374, 376, 378, 380, 382, 
	384, 386, 388, 390, 392, 394, 396, 398, 
	400, 402, 404, 406, 408, 410, 412, 414, 
	416, 418, 421, 423, 425, 427, 429, 431, 
	433, 435, 437, 439, 441, 443, 445, 447, 
	449, 451, 453, 455, 457, 459, 461, 463, 
	465, 467, 470, 472, 474, 476, 478, 480, 
	482, 484, 486, 488, 490, 492, 495, 497, 
	499, 501, 503, 505, 507, 509, 511, 513, 
	515, 517, 519, 521, 523, 525, 527, 529, 
	531, 534, 536, 538, 540, 542, 544, 546, 
	548, 550, 552, 554, 556, 559, 561, 563, 
	565, 567, 569, 571, 573, 575, 577, 580, 
	582, 584, 586, 588, 590, 592, 594, 596, 
	599, 601, 603, 605, 607, 609, 611, 613, 
	615, 617, 619, 621, 623, 625, 627, 629, 
	631, 633, 635, 637, 639, 641, 643, 645, 
	647, 649, 651, 653, 656, 658, 660, 662, 
	664, 666, 668, 670, 673, 675, 677, 679, 
	681, 683, 685, 687, 690, 692, 694, 696, 
	698, 700, 702, 704, 706, 709, 712, 714, 
	716, 718, 720, 722, 724, 726, 728, 730, 
	732, 734, 736, 738, 740, 742, 744, 746, 
	748, 750, 752, 754, 756, 758, 760, 762, 
	764, 766, 768, 770, 772, 774, 776, 778, 
	780, 782, 784, 787, 790, 793, 795, 798, 
	800, 803, 807, 811, 815
};

static readonly short[] _psi_trans_targs =  new short [] {
	2, 0, 3, 0, 4, 0, 5, 0, 
	6, 0, 7, 0, 8, 0, 9, 0, 
	10, 0, 11, 0, 12, 0, 13, 0, 
	14, 0, 15, 0, 16, 0, 17, 0, 
	18, 0, 19, 0, 20, 0, 21, 0, 
	22, 0, 23, 0, 24, 0, 25, 0, 
	26, 0, 27, 236, 268, 307, 354, 372, 
	0, 29, 28, 29, 28, 30, 0, 31, 
	60, 80, 141, 152, 167, 181, 199, 213, 
	0, 32, 0, 33, 0, 34, 0, 35, 
	0, 36, 0, 37, 0, 38, 0, 39, 
	0, 40, 0, 41, 0, 42, 0, 43, 
	0, 44, 0, 45, 0, 46, 0, 47, 
	0, 48, 0, 49, 0, 50, 0, 51, 
	0, 381, 0, 53, 0, 55, 382, 54, 
	55, 382, 54, 56, 0, 58, 57, 58, 
	57, 381, 0, 56, 53, 0, 61, 0, 
	62, 0, 63, 0, 64, 0, 65, 0, 
	66, 0, 67, 0, 68, 0, 69, 0, 
	71, 70, 71, 70, 72, 0, 73, 0, 
	74, 0, 75, 0, 76, 0, 77, 0, 
	78, 0, 381, 79, 381, 79, 81, 98, 
	0, 82, 0, 83, 0, 84, 0, 85, 
	0, 86, 0, 87, 0, 88, 0, 89, 
	0, 90, 0, 91, 0, 92, 0, 93, 
	0, 94, 0, 95, 0, 96, 0, 381, 
	97, 381, 97, 99, 122, 0, 100, 0, 
	101, 0, 102, 0, 103, 0, 104, 0, 
	105, 0, 106, 0, 107, 0, 108, 0, 
	109, 0, 110, 0, 111, 0, 112, 0, 
	113, 0, 114, 0, 115, 0, 116, 0, 
	117, 0, 118, 0, 119, 0, 120, 0, 
	381, 121, 381, 121, 123, 0, 124, 0, 
	125, 0, 126, 0, 127, 0, 128, 0, 
	129, 0, 130, 0, 131, 0, 132, 0, 
	133, 0, 134, 0, 135, 0, 136, 0, 
	137, 0, 138, 0, 139, 0, 381, 140, 
	381, 140, 142, 0, 143, 0, 144, 0, 
	145, 0, 146, 0, 147, 0, 148, 0, 
	149, 0, 150, 0, 151, 0, 381, 0, 
	153, 0, 154, 0, 155, 0, 156, 0, 
	157, 0, 158, 0, 159, 0, 160, 0, 
	161, 0, 162, 0, 163, 0, 164, 0, 
	165, 0, 166, 0, 381, 0, 168, 0, 
	169, 0, 170, 0, 171, 0, 172, 0, 
	173, 0, 174, 0, 175, 0, 176, 0, 
	177, 0, 178, 0, 179, 0, 381, 180, 
	381, 180, 182, 0, 183, 0, 184, 0, 
	185, 0, 186, 0, 187, 0, 188, 0, 
	190, 189, 190, 189, 191, 0, 192, 0, 
	193, 0, 194, 0, 195, 0, 196, 0, 
	197, 0, 381, 198, 381, 198, 200, 0, 
	201, 0, 202, 205, 0, 203, 0, 381, 
	204, 381, 204, 206, 0, 207, 0, 208, 
	0, 209, 0, 210, 0, 211, 0, 381, 
	212, 381, 212, 214, 0, 215, 0, 216, 
	0, 217, 0, 218, 0, 219, 0, 220, 
	0, 221, 0, 222, 0, 223, 0, 383, 
	224, 383, 224, 53, 226, 0, 227, 0, 
	228, 0, 229, 0, 230, 0, 231, 0, 
	232, 0, 233, 0, 234, 0, 381, 235, 
	381, 235, 237, 0, 238, 250, 0, 239, 
	0, 240, 0, 241, 0, 242, 0, 243, 
	0, 244, 0, 245, 0, 246, 0, 247, 
	0, 248, 0, 381, 249, 381, 249, 251, 
	0, 252, 0, 253, 0, 254, 0, 255, 
	0, 256, 0, 257, 262, 0, 258, 0, 
	259, 0, 260, 0, 261, 0, 381, 0, 
	263, 0, 264, 0, 265, 0, 266, 0, 
	267, 0, 381, 0, 269, 294, 0, 270, 
	0, 271, 0, 272, 0, 273, 0, 274, 
	0, 275, 0, 276, 0, 277, 0, 278, 
	0, 279, 286, 0, 280, 0, 281, 280, 
	282, 0, 283, 0, 284, 0, 285, 0, 
	381, 285, 287, 0, 288, 290, 0, 289, 
	0, 381, 0, 291, 0, 292, 0, 293, 
	0, 381, 0, 295, 0, 296, 0, 297, 
	0, 298, 0, 299, 0, 300, 0, 301, 
	0, 302, 0, 303, 0, 304, 0, 305, 
	0, 381, 306, 381, 306, 308, 0, 309, 
	0, 310, 0, 311, 0, 312, 0, 314, 
	313, 314, 313, 315, 0, 316, 342, 0, 
	317, 0, 318, 0, 319, 0, 320, 0, 
	321, 0, 322, 0, 323, 0, 324, 341, 
	0, 325, 0, 326, 0, 327, 0, 328, 
	0, 329, 0, 330, 0, 331, 0, 332, 
	340, 0, 333, 0, 334, 0, 335, 0, 
	336, 0, 337, 0, 338, 0, 339, 0, 
	381, 0, 332, 340, 0, 324, 341, 0, 
	343, 0, 344, 0, 345, 0, 346, 0, 
	347, 0, 348, 0, 349, 0, 350, 0, 
	351, 0, 352, 0, 381, 353, 381, 353, 
	355, 0, 356, 0, 357, 0, 358, 0, 
	359, 0, 360, 0, 361, 0, 362, 0, 
	363, 0, 364, 0, 365, 0, 366, 0, 
	367, 0, 368, 0, 369, 0, 370, 0, 
	381, 371, 381, 371, 373, 0, 374, 0, 
	375, 0, 376, 0, 377, 0, 378, 0, 
	0, 380, 379, 0, 380, 379, 384, 380, 
	379, 52, 0, 59, 382, 54, 225, 0, 
	385, 0, 384, 385, 0, 386, 384, 388, 
	54, 387, 387, 388, 54, 387, 387, 385, 
	56, 386, 384, 0
};

static readonly byte[] _psi_trans_actions =  new byte [] {
	85, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 1, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 144, 65, 67, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 83, 0, 0, 0, 87, 204, 3, 
	5, 156, 0, 0, 0, 90, 7, 9, 
	0, 11, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	150, 77, 79, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 200, 81, 153, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 188, 
	59, 135, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	192, 59, 138, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 164, 59, 
	117, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 69, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 71, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 168, 59, 
	120, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	141, 61, 63, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 176, 59, 126, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 180, 
	59, 129, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 184, 
	59, 132, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 73, 0, 172, 
	59, 123, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 196, 75, 
	147, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 93, 17, 19, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 15, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 13, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 23, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	25, 0, 0, 0, 0, 0, 0, 0, 
	0, 27, 0, 0, 0, 0, 0, 0, 
	0, 21, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 111, 49, 51, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 99, 
	33, 35, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 105, 39, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 108, 
	43, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	47, 0, 45, 0, 0, 41, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 160, 37, 102, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	96, 29, 31, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 114, 53, 0, 55, 0, 57, 55, 
	0, 0, 0, 5, 156, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 87, 
	3, 204, 3, 5, 0, 156, 0, 0, 
	0, 0, 0, 0
};

const int psi_start = 1;
const int psi_first_final = 381;
const int psi_error = 0;

const int psi_en_main = 1;


#line 302 "RawParser.rl"

		public bool Execute(ArraySegment<byte> buf)
		{
			int start = buf.Offset;
			
#line 611 "RawParser.cs"
	{
	cs = psi_start;
	}

#line 307 "RawParser.rl"
			byte[] data = buf.Array;
			int p = buf.Offset;
			int pe = buf.Offset + buf.Count;
			int eof = pe;
			
#line 622 "RawParser.cs"
	{
	sbyte _klen;
	short _trans;
	int _acts;
	int _nacts;
	short _keys;

	if ( p == pe )
		goto _test_eof;
	if ( cs == 0 )
		goto _out;
_resume:
	_keys = _psi_key_offsets[cs];
	_trans = (short)_psi_index_offsets[cs];

	_klen = _psi_single_lengths[cs];
	if ( _klen > 0 ) {
		short _lower = _keys;
		short _mid;
		short _upper = (short) (_keys + _klen - 1);
		while (true) {
			if ( _upper < _lower )
				break;

			_mid = (short) (_lower + ((_upper-_lower) >> 1));
			if ( data[p] < _psi_trans_keys[_mid] )
				_upper = (short) (_mid - 1);
			else if ( data[p] > _psi_trans_keys[_mid] )
				_lower = (short) (_mid + 1);
			else {
				_trans += (short) (_mid - _keys);
				goto _match;
			}
		}
		_keys += (short) _klen;
		_trans += (short) _klen;
	}

	_klen = _psi_range_lengths[cs];
	if ( _klen > 0 ) {
		short _lower = _keys;
		short _mid;
		short _upper = (short) (_keys + (_klen<<1) - 2);
		while (true) {
			if ( _upper < _lower )
				break;

			_mid = (short) (_lower + (((_upper-_lower) >> 1) & ~1));
			if ( data[p] < _psi_trans_keys[_mid] )
				_upper = (short) (_mid - 2);
			else if ( data[p] > _psi_trans_keys[_mid+1] )
				_lower = (short) (_mid + 2);
			else {
				_trans += (short)((_mid - _keys)>>1);
				goto _match;
			}
		}
		_trans += (short) _klen;
	}

_match:
	cs = _psi_trans_targs[_trans];

	if ( _psi_trans_actions[_trans] == 0 )
		goto _again;

	_acts = _psi_trans_actions[_trans];
	_nacts = _psi_actions[_acts++];
	while ( _nacts-- > 0 )
	{
		switch ( _psi_actions[_acts++] )
		{
	case 0:
#line 43 "RawParser.rl"
	{
			if (DateTime != null) {
				var t = new DateTime(
					Number(data, p - 13, 4),
					Number(data, p - 19, 2),
					Number(data, p - 16, 2),
					Number(data, p - 6,  2),
					Number(data, p - 3,  2),
					Number(data, p,      2)
				);
				DateTime(t);
			}
		}
	break;
	case 1:
#line 60 "RawParser.rl"
	{ tmp1 = p; }
	break;
	case 2:
#line 60 "RawParser.rl"
	{ tmp2 = p; }
	break;
	case 3:
#line 60 "RawParser.rl"
	{ tmp3 = 0; tmp4 = 0; }
	break;
	case 4:
#line 60 "RawParser.rl"
	{ tmp3 = p; }
	break;
	case 5:
#line 60 "RawParser.rl"
	{ tmp4 = p; }
	break;
	case 6:
#line 60 "RawParser.rl"
	{
			if (Option != null) {
				Option(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
				       (tmp3 == tmp4 && tmp4 == 0 ?
				           default(ArraySegment<byte>) :
				           new ArraySegment<byte>(data, tmp3, tmp4 - tmp3)));
			}
		}
	break;
	case 7:
#line 69 "RawParser.rl"
	{
			if (LogFileStart != null) {
				LogFileStart();
			}
		}
	break;
	case 8:
#line 75 "RawParser.rl"
	{
			if (LogFileEnd != null) {
				LogFileEnd();
			}
		}
	break;
	case 9:
#line 81 "RawParser.rl"
	{ tmp1 = p; }
	break;
	case 10:
#line 81 "RawParser.rl"
	{
			if (LoadingMap != null) {
				LoadingMap(new ArraySegment<byte>(data, tmp1, p - tmp1));
			}
		}
	break;
	case 11:
#line 87 "RawParser.rl"
	{
			if (ServerCVarsStart != null) {
				ServerCVarsStart();
			}
		}
	break;
	case 12:
#line 93 "RawParser.rl"
	{ tmp1 = p; }
	break;
	case 13:
#line 93 "RawParser.rl"
	{
			if (ServerCVarSet != null) {
				int name_start = start + 38;
				int name_len = tmp1 - name_start;
				int value_start = name_start + name_len + 5;
				int value_len = p - value_start;
				ServerCVarSet(new ArraySegment<byte>(data, name_start, name_len),
				              new ArraySegment<byte>(data, value_start, value_len));
			}
		}
	break;
	case 14:
#line 104 "RawParser.rl"
	{
			if (ServerCVarsEnd != null) {
				ServerCVarsEnd();
			}
		}
	break;
	case 15:
#line 110 "RawParser.rl"
	{ tmp1 = p; }
	break;
	case 16:
#line 110 "RawParser.rl"
	{
			if (WorldTrigger != null) {
				WorldTrigger(new ArraySegment<byte>(data, tmp1, p - tmp1));
			}
		}
	break;
	case 17:
#line 116 "RawParser.rl"
	{ tmp1 = p; }
	break;
	case 18:
#line 116 "RawParser.rl"
	{ tmp2 = p; }
	break;
	case 19:
#line 117 "RawParser.rl"
	{ tmp3 = p; }
	break;
	case 20:
#line 117 "RawParser.rl"
	{ tmp4 = p; }
	break;
	case 21:
#line 117 "RawParser.rl"
	{
			if (TeamTrigger != null) {
				TeamTrigger(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
				            new ArraySegment<byte>(data, tmp3, tmp4 - tmp3));
			}
		}
	break;
	case 22:
#line 123 "RawParser.rl"
	{ tmp3 = p; }
	break;
	case 23:
#line 123 "RawParser.rl"
	{ tmp4 = p; }
	break;
	case 24:
#line 123 "RawParser.rl"
	{ tmp5 = p; }
	break;
	case 25:
#line 123 "RawParser.rl"
	{ tmp6 = p; }
	break;
	case 26:
#line 123 "RawParser.rl"
	{
			if (TeamScore != null) {
				TeamScore(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
				          Number(data, tmp3, tmp4 - tmp3),
				          Number(data, tmp5, tmp6 - tmp5));
			}
		}
	break;
	case 27:
#line 131 "RawParser.rl"
	{ tmp1 = p; }
	break;
	case 28:
#line 131 "RawParser.rl"
	{
			if (StartedMap != null) {
				StartedMap(new ArraySegment<byte>(data, tmp1, p - tmp1));
			}
		}
	break;
	case 29:
#line 137 "RawParser.rl"
	{ tmp1 = p; }
	break;
	case 30:
#line 137 "RawParser.rl"
	{ tmp2 = p; }
	break;
	case 31:
#line 137 "RawParser.rl"
	{
			if (Meta != null) {
				Meta(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					 new ArraySegment<byte>(data, p + 1, pe - p - 1));
			}
		
		}
	break;
	case 32:
#line 145 "RawParser.rl"
	{ value_start = p; }
	break;
	case 33:
#line 145 "RawParser.rl"
	{
			value = new ArraySegment<byte>(data, value_start, p - value_start);
			value_start = 0;
		}
	break;
	case 34:
#line 150 "RawParser.rl"
	{ value_start = p; }
	break;
	case 35:
#line 150 "RawParser.rl"
	{
			target = new ArraySegment<byte>(data, value_start, p - value_start);
			value_start = 0;
		}
	break;
	case 36:
#line 155 "RawParser.rl"
	{ tmp1 = p; }
	break;
	case 37:
#line 155 "RawParser.rl"
	{ tmp2 = p; }
	break;
	case 38:
#line 156 "RawParser.rl"
	{
				if (PlayerConnect != null) {
					PlayerConnect(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					              value);
				}
			}
	break;
	case 39:
#line 162 "RawParser.rl"
	{
				if (PlayerDisconnect != null) {
					PlayerDisconnect(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1));
				}
			}
	break;
	case 40:
#line 167 "RawParser.rl"
	{
				if (PlayerEnterGame != null) {
					PlayerEnterGame(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1));
				}
			}
	break;
	case 41:
#line 172 "RawParser.rl"
	{
				if (PlayerJoinTeam != null) {
					PlayerJoinTeam(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					               value);
				}
			}
	break;
	case 42:
#line 179 "RawParser.rl"
	{ tmp3 = 0; tmp4 = 0; }
	break;
	case 43:
#line 179 "RawParser.rl"
	{ tmp3 = p; }
	break;
	case 44:
#line 179 "RawParser.rl"
	{ tmp4 = p; }
	break;
	case 45:
#line 180 "RawParser.rl"
	{
				if (tmp3 == 0) {
					if (PlayerTrigger != null) {
						PlayerTrigger(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
									  value);
					}
				} else {
					if (PlayerTriggerAgainst != null) {
						PlayerTriggerAgainst(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
						                     value,
						                     new ArraySegment<byte>(data, tmp3, tmp4 - tmp3));
					}
				}
			}
	break;
	case 46:
#line 194 "RawParser.rl"
	{ tmp3 = p; }
	break;
	case 47:
#line 194 "RawParser.rl"
	{ tmp4 = p; }
	break;
	case 48:
#line 194 "RawParser.rl"
	{ tmp5 = p; }
	break;
	case 49:
#line 194 "RawParser.rl"
	{ tmp6 = p; }
	break;
	case 50:
#line 194 "RawParser.rl"
	{
				if (PlayerAttack != null) {
					PlayerAttack(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					             new ArraySegment<byte>(data, tmp3, tmp4 - tmp3),
					             new ArraySegment<byte>(data, tmp5, tmp6 - tmp5));
				}
			}
	break;
	case 51:
#line 201 "RawParser.rl"
	{
				if (PlayerKill != null) {
					PlayerKill(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					           target,
					           value);
				}
			}
	break;
	case 52:
#line 208 "RawParser.rl"
	{
				if (PlayerSay != null) {
					PlayerSay(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
						value);
				}
			}
	break;
	case 53:
#line 214 "RawParser.rl"
	{
				if (PlayerSayTeam != null) {
					PlayerSayTeam(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					        value);
				}
			}
	break;
	case 54:
#line 220 "RawParser.rl"
	{
				if (PlayerValidate != null) {
					PlayerValidate(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1));
				}
			}
	break;
	case 55:
#line 225 "RawParser.rl"
	{
				if (PlayerNameChange != null) {
					PlayerNameChange(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					           value);
				}
			}
	break;
	case 56:
#line 231 "RawParser.rl"
	{
				if (PlayerSuicide != null) {
					PlayerSuicide(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					        value);
				}
			}
	break;
	case 57:
#line 251 "RawParser.rl"
	{
				if (End != null) {
					End();
				}
			}
	break;
#line 1077 "RawParser.cs"
		default: break;
		}
	}

_again:
	if ( cs == 0 )
		goto _out;
	if ( ++p != pe )
		goto _resume;
	_test_eof: {}
	_out: {}
	}

#line 312 "RawParser.rl"
			return p == pe;
		}
	}
}
