
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

		
#line 239 "RawParser.rl"


		public event Action<DateTime> DateTime;

		public event Action<ArraySegment<byte>, ArraySegment<byte>> Option;

		#region Log Message Types

		public event Action LogFileStart;
		public event Action LogFileEnd;

		public event Action ServerCVarsStart;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> ServerCVarSet;
		public event Action ServerCVarsEnd;

		public event Action<ArraySegment<byte>> LoadingMap;
		public event Action<ArraySegment<byte>> StartedMap;

		public event Action<ArraySegment<byte>> WorldTrigger;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> TeamTrigger;

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

		
#line 83 "RawParser.cs"
static readonly sbyte[] _psi_actions =  new sbyte [] {
	0, 1, 0, 1, 1, 1, 2, 1, 
	4, 1, 5, 1, 6, 1, 7, 1, 
	8, 1, 9, 1, 10, 1, 11, 1, 
	12, 1, 13, 1, 14, 1, 15, 1, 
	16, 1, 17, 1, 18, 1, 19, 1, 
	22, 1, 23, 1, 24, 1, 26, 1, 
	27, 1, 28, 1, 29, 1, 31, 1, 
	32, 1, 34, 1, 35, 1, 38, 1, 
	39, 1, 40, 1, 46, 1, 49, 2, 
	1, 2, 2, 4, 5, 2, 9, 10, 
	2, 15, 16, 2, 17, 18, 2, 20, 
	21, 2, 22, 23, 2, 25, 30, 2, 
	25, 33, 2, 25, 37, 2, 25, 43, 
	2, 25, 44, 2, 25, 45, 2, 25, 
	47, 2, 25, 48, 2, 26, 27, 2, 
	28, 29, 2, 36, 37, 2, 38, 39, 
	2, 41, 42, 3, 2, 3, 6, 3, 
	19, 20, 21, 3, 24, 25, 30, 3, 
	24, 25, 33, 3, 24, 25, 37, 3, 
	24, 25, 43, 3, 24, 25, 44, 3, 
	24, 25, 45, 3, 24, 25, 47, 3, 
	24, 25, 48, 3, 35, 36, 37, 3, 
	40, 41, 42, 4, 1, 2, 3, 6
	
};

static readonly short[] _psi_key_offsets =  new short [] {
	0, 0, 1, 2, 4, 6, 7, 9, 
	11, 12, 14, 16, 18, 20, 21, 22, 
	23, 25, 27, 28, 30, 32, 33, 35, 
	37, 38, 39, 44, 45, 46, 47, 56, 
	57, 58, 59, 60, 61, 62, 63, 64, 
	65, 66, 67, 68, 69, 70, 71, 72, 
	73, 74, 75, 76, 77, 78, 80, 82, 
	83, 84, 85, 86, 88, 89, 90, 91, 
	92, 93, 94, 95, 96, 97, 98, 99, 
	100, 101, 102, 103, 104, 105, 106, 107, 
	108, 110, 111, 112, 113, 114, 115, 116, 
	117, 118, 119, 120, 121, 122, 123, 124, 
	125, 126, 127, 129, 130, 131, 132, 133, 
	134, 135, 136, 137, 138, 139, 140, 141, 
	142, 143, 144, 145, 146, 147, 148, 149, 
	150, 151, 152, 153, 154, 155, 156, 157, 
	158, 159, 160, 161, 162, 163, 164, 165, 
	166, 167, 168, 169, 170, 171, 172, 173, 
	174, 175, 176, 177, 178, 179, 180, 181, 
	182, 183, 184, 185, 186, 187, 188, 189, 
	190, 191, 192, 193, 194, 195, 196, 197, 
	198, 199, 200, 201, 202, 203, 204, 205, 
	206, 207, 208, 209, 210, 211, 212, 213, 
	214, 215, 216, 217, 218, 219, 220, 221, 
	222, 223, 224, 225, 226, 227, 228, 229, 
	230, 231, 233, 234, 235, 236, 237, 238, 
	239, 240, 241, 242, 243, 244, 245, 246, 
	247, 248, 249, 250, 251, 252, 253, 254, 
	255, 256, 258, 259, 260, 261, 262, 263, 
	264, 265, 266, 267, 268, 269, 271, 272, 
	273, 274, 275, 276, 277, 278, 279, 280, 
	281, 282, 283, 284, 285, 286, 287, 288, 
	289, 291, 292, 293, 294, 295, 296, 297, 
	298, 299, 300, 301, 302, 304, 305, 306, 
	307, 308, 309, 310, 311, 312, 313, 315, 
	316, 317, 318, 319, 320, 321, 322, 323, 
	325, 326, 327, 328, 329, 330, 331, 332, 
	333, 334, 335, 336, 337, 338, 339, 340, 
	341, 342, 343, 344, 345, 346, 347, 348, 
	349, 350, 351, 352, 353, 354, 355, 356, 
	357, 358, 359, 360, 361, 362, 363, 364, 
	365, 366, 367, 368, 369, 370, 371, 372, 
	373, 374, 375, 376, 377, 378, 379, 380, 
	381, 382, 383, 384, 386
};

static readonly char[] _psi_trans_keys =  new char [] {
	'\u004c', '\u0020', '\u0030', '\u0039', '\u0030', '\u0039', '\u002f', '\u0030', 
	'\u0039', '\u0030', '\u0039', '\u002f', '\u0030', '\u0039', '\u0030', '\u0039', 
	'\u0030', '\u0039', '\u0030', '\u0039', '\u0020', '\u002d', '\u0020', '\u0030', 
	'\u0039', '\u0030', '\u0039', '\u003a', '\u0030', '\u0039', '\u0030', '\u0039', 
	'\u003a', '\u0030', '\u0039', '\u0030', '\u0039', '\u003a', '\u0020', '\u0022', 
	'\u004c', '\u0053', '\u0054', '\u0057', '\u0022', '\u0022', '\u0020', '\u0053', 
	'\u0061', '\u0063', '\u0064', '\u0065', '\u006a', '\u006b', '\u0073', '\u0074', 
	'\u0054', '\u0045', '\u0041', '\u004d', '\u0020', '\u0055', '\u0053', '\u0045', 
	'\u0052', '\u0049', '\u0044', '\u0020', '\u0076', '\u0061', '\u006c', '\u0069', 
	'\u0064', '\u0061', '\u0074', '\u0065', '\u0064', '\u0028', '\u0020', '\u0029', 
	'\u0020', '\u0029', '\u0022', '\u0022', '\u0022', '\u0029', '\u0022', '\u0028', 
	'\u0074', '\u0074', '\u0061', '\u0063', '\u006b', '\u0065', '\u0064', '\u0020', 
	'\u0022', '\u0022', '\u0022', '\u0020', '\u0077', '\u0069', '\u0074', '\u0068', 
	'\u0020', '\u0022', '\u0022', '\u0022', '\u0068', '\u006f', '\u0061', '\u006e', 
	'\u0067', '\u0065', '\u0064', '\u0020', '\u006e', '\u0061', '\u006d', '\u0065', 
	'\u0020', '\u0074', '\u006f', '\u0020', '\u0022', '\u0022', '\u0022', '\u006d', 
	'\u006e', '\u006d', '\u0069', '\u0074', '\u0074', '\u0065', '\u0064', '\u0020', 
	'\u0073', '\u0075', '\u0069', '\u0063', '\u0069', '\u0064', '\u0065', '\u0020', 
	'\u0077', '\u0069', '\u0074', '\u0068', '\u0020', '\u0022', '\u0022', '\u0022', 
	'\u006e', '\u0065', '\u0063', '\u0074', '\u0065', '\u0064', '\u002c', '\u0020', 
	'\u0061', '\u0064', '\u0064', '\u0072', '\u0065', '\u0073', '\u0073', '\u0020', 
	'\u0022', '\u0022', '\u0022', '\u0069', '\u0073', '\u0063', '\u006f', '\u006e', 
	'\u006e', '\u0065', '\u0063', '\u0074', '\u0065', '\u0064', '\u006e', '\u0074', 
	'\u0065', '\u0072', '\u0065', '\u0064', '\u0020', '\u0074', '\u0068', '\u0065', 
	'\u0020', '\u0067', '\u0061', '\u006d', '\u0065', '\u006f', '\u0069', '\u006e', 
	'\u0065', '\u0064', '\u0020', '\u0074', '\u0065', '\u0061', '\u006d', '\u0020', 
	'\u0022', '\u0022', '\u0022', '\u0069', '\u006c', '\u006c', '\u0065', '\u0064', 
	'\u0020', '\u0022', '\u0022', '\u0022', '\u0020', '\u0077', '\u0069', '\u0074', 
	'\u0068', '\u0020', '\u0022', '\u0022', '\u0022', '\u0061', '\u0079', '\u0020', 
	'\u005f', '\u0022', '\u0022', '\u0022', '\u0074', '\u0065', '\u0061', '\u006d', 
	'\u0020', '\u0022', '\u0022', '\u0022', '\u0072', '\u0069', '\u0067', '\u0067', 
	'\u0065', '\u0072', '\u0065', '\u0064', '\u0020', '\u0022', '\u0022', '\u0022', 
	'\u0028', '\u0061', '\u0067', '\u0061', '\u0069', '\u006e', '\u0073', '\u0074', 
	'\u0020', '\u0022', '\u0022', '\u0022', '\u006f', '\u0061', '\u0067', '\u0064', 
	'\u0069', '\u006e', '\u0067', '\u0020', '\u006d', '\u0061', '\u0070', '\u0020', 
	'\u0022', '\u0022', '\u0022', '\u0020', '\u0066', '\u0069', '\u006c', '\u0065', 
	'\u0020', '\u0063', '\u0073', '\u006c', '\u006f', '\u0073', '\u0065', '\u0064', 
	'\u0074', '\u0061', '\u0072', '\u0074', '\u0065', '\u0064', '\u0065', '\u0074', 
	'\u0072', '\u0076', '\u0065', '\u0072', '\u0020', '\u0063', '\u0076', '\u0061', 
	'\u0072', '\u0020', '\u0073', '\u0022', '\u0022', '\u0020', '\u003d', '\u0020', 
	'\u0022', '\u0022', '\u0020', '\u0065', '\u0073', '\u006e', '\u0064', '\u0074', 
	'\u0061', '\u0072', '\u0074', '\u0061', '\u0072', '\u0074', '\u0065', '\u0064', 
	'\u0020', '\u006d', '\u0061', '\u0070', '\u0020', '\u0022', '\u0022', '\u0022', 
	'\u0065', '\u0061', '\u006d', '\u0020', '\u0022', '\u0022', '\u0022', '\u0020', 
	'\u0074', '\u0072', '\u0069', '\u0067', '\u0067', '\u0065', '\u0072', '\u0065', 
	'\u0064', '\u0020', '\u0022', '\u0022', '\u0022', '\u006f', '\u0072', '\u006c', 
	'\u0064', '\u0020', '\u0074', '\u0072', '\u0069', '\u0067', '\u0067', '\u0065', 
	'\u0072', '\u0065', '\u0064', '\u0020', '\u0022', '\u0022', '\u0022', '\u0020', 
	'\u0020', '\u0029', '\u0020', (char) 0
};

static readonly sbyte[] _psi_single_lengths =  new sbyte [] {
	0, 1, 1, 0, 0, 1, 0, 0, 
	1, 0, 0, 0, 0, 1, 1, 1, 
	0, 0, 1, 0, 0, 1, 0, 0, 
	1, 1, 5, 1, 1, 1, 9, 1, 
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
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 1, 1, 1, 1, 1, 
	1, 1, 1, 2, 1
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
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0
};

static readonly short[] _psi_index_offsets =  new short [] {
	0, 0, 2, 4, 6, 8, 10, 12, 
	14, 16, 18, 20, 22, 24, 26, 28, 
	30, 32, 34, 36, 38, 40, 42, 44, 
	46, 48, 50, 56, 58, 60, 62, 72, 
	74, 76, 78, 80, 82, 84, 86, 88, 
	90, 92, 94, 96, 98, 100, 102, 104, 
	106, 108, 110, 112, 114, 116, 119, 122, 
	124, 126, 128, 130, 133, 135, 137, 139, 
	141, 143, 145, 147, 149, 151, 153, 155, 
	157, 159, 161, 163, 165, 167, 169, 171, 
	173, 176, 178, 180, 182, 184, 186, 188, 
	190, 192, 194, 196, 198, 200, 202, 204, 
	206, 208, 210, 213, 215, 217, 219, 221, 
	223, 225, 227, 229, 231, 233, 235, 237, 
	239, 241, 243, 245, 247, 249, 251, 253, 
	255, 257, 259, 261, 263, 265, 267, 269, 
	271, 273, 275, 277, 279, 281, 283, 285, 
	287, 289, 291, 293, 295, 297, 299, 301, 
	303, 305, 307, 309, 311, 313, 315, 317, 
	319, 321, 323, 325, 327, 329, 331, 333, 
	335, 337, 339, 341, 343, 345, 347, 349, 
	351, 353, 355, 357, 359, 361, 363, 365, 
	367, 369, 371, 373, 375, 377, 379, 381, 
	383, 385, 387, 389, 391, 393, 395, 397, 
	399, 401, 403, 405, 407, 409, 411, 413, 
	415, 417, 420, 422, 424, 426, 428, 430, 
	432, 434, 436, 438, 440, 442, 444, 446, 
	448, 450, 452, 454, 456, 458, 460, 462, 
	464, 466, 469, 471, 473, 475, 477, 479, 
	481, 483, 485, 487, 489, 491, 494, 496, 
	498, 500, 502, 504, 506, 508, 510, 512, 
	514, 516, 518, 520, 522, 524, 526, 528, 
	530, 533, 535, 537, 539, 541, 543, 545, 
	547, 549, 551, 553, 555, 558, 560, 562, 
	564, 566, 568, 570, 572, 574, 576, 579, 
	581, 583, 585, 587, 589, 591, 593, 595, 
	598, 600, 602, 604, 606, 608, 610, 612, 
	614, 616, 618, 620, 622, 624, 626, 628, 
	630, 632, 634, 636, 638, 640, 642, 644, 
	646, 648, 650, 652, 654, 656, 658, 660, 
	662, 664, 666, 668, 670, 672, 674, 676, 
	678, 680, 682, 684, 686, 688, 690, 692, 
	694, 696, 698, 700, 702, 704, 706, 708, 
	710, 712, 714, 716, 719
};

static readonly short[] _psi_trans_targs =  new short [] {
	2, 0, 3, 0, 4, 0, 5, 0, 
	6, 0, 7, 0, 8, 0, 9, 0, 
	10, 0, 11, 0, 12, 0, 13, 0, 
	14, 0, 15, 0, 16, 0, 17, 0, 
	18, 0, 19, 0, 20, 0, 21, 0, 
	22, 0, 23, 0, 24, 0, 25, 0, 
	26, 0, 27, 236, 268, 307, 328, 0, 
	29, 28, 29, 28, 30, 0, 31, 60, 
	80, 141, 152, 167, 181, 199, 213, 0, 
	32, 0, 33, 0, 34, 0, 35, 0, 
	36, 0, 37, 0, 38, 0, 39, 0, 
	40, 0, 41, 0, 42, 0, 43, 0, 
	44, 0, 45, 0, 46, 0, 47, 0, 
	48, 0, 49, 0, 50, 0, 51, 0, 
	346, 0, 53, 0, 55, 347, 54, 55, 
	347, 54, 56, 0, 58, 57, 58, 57, 
	346, 0, 56, 53, 0, 61, 0, 62, 
	0, 63, 0, 64, 0, 65, 0, 66, 
	0, 67, 0, 68, 0, 69, 0, 71, 
	70, 71, 70, 72, 0, 73, 0, 74, 
	0, 75, 0, 76, 0, 77, 0, 78, 
	0, 346, 79, 346, 79, 81, 98, 0, 
	82, 0, 83, 0, 84, 0, 85, 0, 
	86, 0, 87, 0, 88, 0, 89, 0, 
	90, 0, 91, 0, 92, 0, 93, 0, 
	94, 0, 95, 0, 96, 0, 346, 97, 
	346, 97, 99, 122, 0, 100, 0, 101, 
	0, 102, 0, 103, 0, 104, 0, 105, 
	0, 106, 0, 107, 0, 108, 0, 109, 
	0, 110, 0, 111, 0, 112, 0, 113, 
	0, 114, 0, 115, 0, 116, 0, 117, 
	0, 118, 0, 119, 0, 120, 0, 346, 
	121, 346, 121, 123, 0, 124, 0, 125, 
	0, 126, 0, 127, 0, 128, 0, 129, 
	0, 130, 0, 131, 0, 132, 0, 133, 
	0, 134, 0, 135, 0, 136, 0, 137, 
	0, 138, 0, 139, 0, 346, 140, 346, 
	140, 142, 0, 143, 0, 144, 0, 145, 
	0, 146, 0, 147, 0, 148, 0, 149, 
	0, 150, 0, 151, 0, 346, 0, 153, 
	0, 154, 0, 155, 0, 156, 0, 157, 
	0, 158, 0, 159, 0, 160, 0, 161, 
	0, 162, 0, 163, 0, 164, 0, 165, 
	0, 166, 0, 346, 0, 168, 0, 169, 
	0, 170, 0, 171, 0, 172, 0, 173, 
	0, 174, 0, 175, 0, 176, 0, 177, 
	0, 178, 0, 179, 0, 346, 180, 346, 
	180, 182, 0, 183, 0, 184, 0, 185, 
	0, 186, 0, 187, 0, 188, 0, 190, 
	189, 190, 189, 191, 0, 192, 0, 193, 
	0, 194, 0, 195, 0, 196, 0, 197, 
	0, 346, 198, 346, 198, 200, 0, 201, 
	0, 202, 205, 0, 203, 0, 346, 204, 
	346, 204, 206, 0, 207, 0, 208, 0, 
	209, 0, 210, 0, 211, 0, 346, 212, 
	346, 212, 214, 0, 215, 0, 216, 0, 
	217, 0, 218, 0, 219, 0, 220, 0, 
	221, 0, 222, 0, 223, 0, 348, 224, 
	348, 224, 53, 226, 0, 227, 0, 228, 
	0, 229, 0, 230, 0, 231, 0, 232, 
	0, 233, 0, 234, 0, 346, 235, 346, 
	235, 237, 0, 238, 250, 0, 239, 0, 
	240, 0, 241, 0, 242, 0, 243, 0, 
	244, 0, 245, 0, 246, 0, 247, 0, 
	248, 0, 346, 249, 346, 249, 251, 0, 
	252, 0, 253, 0, 254, 0, 255, 0, 
	256, 0, 257, 262, 0, 258, 0, 259, 
	0, 260, 0, 261, 0, 346, 0, 263, 
	0, 264, 0, 265, 0, 266, 0, 267, 
	0, 346, 0, 269, 294, 0, 270, 0, 
	271, 0, 272, 0, 273, 0, 274, 0, 
	275, 0, 276, 0, 277, 0, 278, 0, 
	279, 286, 0, 280, 0, 281, 280, 282, 
	0, 283, 0, 284, 0, 285, 0, 346, 
	285, 287, 0, 288, 290, 0, 289, 0, 
	346, 0, 291, 0, 292, 0, 293, 0, 
	346, 0, 295, 0, 296, 0, 297, 0, 
	298, 0, 299, 0, 300, 0, 301, 0, 
	302, 0, 303, 0, 304, 0, 305, 0, 
	346, 306, 346, 306, 308, 0, 309, 0, 
	310, 0, 311, 0, 312, 0, 314, 313, 
	314, 313, 315, 0, 316, 0, 317, 0, 
	318, 0, 319, 0, 320, 0, 321, 0, 
	322, 0, 323, 0, 324, 0, 325, 0, 
	326, 0, 346, 327, 346, 327, 329, 0, 
	330, 0, 331, 0, 332, 0, 333, 0, 
	334, 0, 335, 0, 336, 0, 337, 0, 
	338, 0, 339, 0, 340, 0, 341, 0, 
	342, 0, 343, 0, 344, 0, 346, 345, 
	346, 345, 52, 0, 59, 347, 54, 225, 
	0, 0
};

static readonly byte[] _psi_trans_actions =  new byte [] {
	69, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 1, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	119, 49, 51, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	67, 0, 0, 0, 71, 179, 3, 5, 
	131, 0, 0, 0, 74, 7, 9, 0, 
	11, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 125, 
	61, 63, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 175, 65, 128, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 163, 43, 
	110, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 167, 
	43, 113, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 139, 43, 92, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 53, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 55, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 143, 43, 95, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 116, 
	45, 47, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 151, 43, 101, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 155, 43, 
	104, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 159, 43, 
	107, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 57, 0, 147, 43, 
	98, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 171, 59, 122, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 77, 17, 19, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 15, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 13, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 23, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 25, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	27, 0, 0, 0, 0, 0, 0, 0, 
	21, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	89, 39, 41, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 83, 33, 
	35, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 135, 37, 86, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 0, 0, 
	0, 0, 0, 0, 0, 0, 80, 29, 
	31, 0, 0, 0, 5, 131, 0, 0, 
	0, 0
};

const int psi_start = 1;
const int psi_first_final = 346;
const int psi_error = 0;

const int psi_en_main = 1;


#line 281 "RawParser.rl"

		public void Execute(ArraySegment<byte> buf)
		{
			int start = buf.Offset;
			
#line 552 "RawParser.cs"
	{
	cs = psi_start;
	}

#line 286 "RawParser.rl"
			byte[] data = buf.Array;
			int p = buf.Offset;
			int pe = buf.Offset + buf.Count;
			
#line 562 "RawParser.cs"
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
#line 116 "RawParser.rl"
	{ tmp3 = p; }
	break;
	case 20:
#line 116 "RawParser.rl"
	{ tmp4 = p; }
	break;
	case 21:
#line 116 "RawParser.rl"
	{
			if (TeamTrigger != null) {
				TeamTrigger(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
				            new ArraySegment<byte>(data, tmp3, tmp4 - tmp3));
			}
		}
	break;
	case 22:
#line 123 "RawParser.rl"
	{ tmp1 = p; }
	break;
	case 23:
#line 123 "RawParser.rl"
	{
			if (StartedMap != null) {
				StartedMap(new ArraySegment<byte>(data, tmp1, p - tmp1));
			}
		}
	break;
	case 24:
#line 129 "RawParser.rl"
	{ value_start = p; }
	break;
	case 25:
#line 129 "RawParser.rl"
	{
			value = new ArraySegment<byte>(data, value_start, p - value_start);
			value_start = 0;
		}
	break;
	case 26:
#line 134 "RawParser.rl"
	{ value_start = p; }
	break;
	case 27:
#line 134 "RawParser.rl"
	{
			target = new ArraySegment<byte>(data, value_start, p - value_start);
			value_start = 0;
		}
	break;
	case 28:
#line 139 "RawParser.rl"
	{ tmp1 = p; }
	break;
	case 29:
#line 139 "RawParser.rl"
	{ tmp2 = p; }
	break;
	case 30:
#line 140 "RawParser.rl"
	{
				if (PlayerConnect != null) {
					PlayerConnect(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					              value);
				}
			}
	break;
	case 31:
#line 146 "RawParser.rl"
	{
				if (PlayerDisconnect != null) {
					PlayerDisconnect(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1));
				}
			}
	break;
	case 32:
#line 151 "RawParser.rl"
	{
				if (PlayerEnterGame != null) {
					PlayerEnterGame(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1));
				}
			}
	break;
	case 33:
#line 156 "RawParser.rl"
	{
				if (PlayerJoinTeam != null) {
					PlayerJoinTeam(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					               value);
				}
			}
	break;
	case 34:
#line 163 "RawParser.rl"
	{ tmp3 = 0; tmp4 = 0; }
	break;
	case 35:
#line 163 "RawParser.rl"
	{ tmp3 = p; }
	break;
	case 36:
#line 163 "RawParser.rl"
	{ tmp4 = p; }
	break;
	case 37:
#line 164 "RawParser.rl"
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
	case 38:
#line 178 "RawParser.rl"
	{ tmp3 = p; }
	break;
	case 39:
#line 178 "RawParser.rl"
	{ tmp4 = p; }
	break;
	case 40:
#line 178 "RawParser.rl"
	{ tmp5 = p; }
	break;
	case 41:
#line 178 "RawParser.rl"
	{ tmp6 = p; }
	break;
	case 42:
#line 178 "RawParser.rl"
	{
				if (PlayerAttack != null) {
					PlayerAttack(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					             new ArraySegment<byte>(data, tmp3, tmp4 - tmp3),
					             new ArraySegment<byte>(data, tmp5, tmp6 - tmp5));
				}
			}
	break;
	case 43:
#line 185 "RawParser.rl"
	{
				if (PlayerKill != null) {
					PlayerKill(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					           target,
					           value);
				}
			}
	break;
	case 44:
#line 192 "RawParser.rl"
	{
				if (PlayerSay != null) {
					PlayerSay(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
						value);
				}
			}
	break;
	case 45:
#line 198 "RawParser.rl"
	{
				if (PlayerSayTeam != null) {
					PlayerSayTeam(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					        value);
				}
			}
	break;
	case 46:
#line 204 "RawParser.rl"
	{
				if (PlayerValidate != null) {
					PlayerValidate(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1));
				}
			}
	break;
	case 47:
#line 209 "RawParser.rl"
	{
				if (PlayerNameChange != null) {
					PlayerNameChange(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					           value);
				}
			}
	break;
	case 48:
#line 215 "RawParser.rl"
	{
				if (PlayerSuicide != null) {
					PlayerSuicide(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					        value);
				}
			}
	break;
	case 49:
#line 234 "RawParser.rl"
	{
				if (End != null) {
					End();
				}
			}
	break;
#line 973 "RawParser.cs"
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

#line 290 "RawParser.rl"
		}
	}
}
