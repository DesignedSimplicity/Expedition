using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web;

namespace Expedition.Win.NavTree.Framework
{
    public static class Extensions
    {
        // ==================================================
        // Object
        // ==================================================
        public static T ThisOrNew<T>(this T input)
        {
            if (input == null) input = (T)Activator.CreateInstance(typeof(T));
            return input;
        }
        public static T ThisOrNew<T>(this T input, object o)
        {
            if (input == null) input = (T)o;
            return input;
        }

        // ==================================================
        // IEnumerable + IList
        // ==================================================
        /// <summary>
        /// Adds item if not already in list and neither are null
        /// </summary>
		public static T AddSafe<T>(this IList<T> input, T item)
		{
			if (!((item == null) || (input == null) || input.Contains(item))) input.Add(item);
			return item;
		}

        public static string ToCommaTextString<T>(this T[] input)
        {
            string text = "";
            foreach (var i in input)
            {
                text += String.Format("{0}, ", i);
            }
            if (text.Length > 0) text = text.TrimCommaTrail();
            return text;
        }

        // ==================================================
        // Enum
        // ==================================================
        public static int ToInt(this Enum input)
        {
            return Convert.ToInt32(input);
        }
        public static string ToDisplayString(this Enum input)
        {
            return input.ToString().ToSpacePascalCase();
        }
        /*
        public static bool Has<T>(this System.Enum type, T value)
        {
            try { return (((int)(object)type & (int)(object)value) == (int)(object)value); }
            catch { return false; }
        }
        public static bool Is<T>(this System.Enum type, T value)
        {
            try { return (int)(object)type == (int)(object)value; }
            catch { return false; }
        }
        public static T Add<T>(this System.Enum type, T value)
        {
            try { return (T)(object)(((int)(object)type | (int)(object)value)); }
            catch (Exception ex) { throw new ArgumentException(String.Format("Could not append value from enumerated type '{0}'.",typeof(T).Name), ex); }
        }
        public static T Remove<T>(this System.Enum type, T value)
        {
            try { return (T)(object)(((int)(object)type & ~(int)(object)value)); }
            catch (Exception ex) { throw new ArgumentException(String.Format("Could not remove value from enumerated type '{0}'.", typeof(T).Name), ex); }
        }*/
        public static bool IsFlagSet(this System.Enum type, Int64 flags)
        {
            Int64 bit = 1 << (SafeConvert.ToInt(type) - 1);
            return bit == (flags & bit);
        }
        public static Int64 SetFlag(this System.Enum type, Int64 flags, bool on = true)
        {
            Int64 bit = 1 << (SafeConvert.ToInt(type) - 1);
            if (on)
                return flags | bit;
            else
                return flags | ~bit;
        }


        // ==================================================
        // DateTime
        // ==================================================
		public static string ToFriendyDateText(this DateTime input)
		{
			return String.Format("{0:MMMM} {1}, {0:yyyy}", input, input.Day.ToOrdinal());
		}

        // ==================================================
        // TimeSpan
        // ==================================================
        public static string ToUTCOffsetString(this TimeSpan input)
        {
            return String.Format("{0:00}:{1:00}", input.Hours, Math.Abs(input.Minutes));
        }

        // ==================================================
        // Number Types
        // ==================================================
		public static string ToOrdinal(this int input)
		{
			if (input <= 0) return input.ToString();
			switch (input % 100)
			{
				case 11:
				case 12:
				case 13:
					return input + "th";
			}
			switch (input % 10)
			{
				case 1:
					return input + "st";
				case 2:
					return input + "nd";
				case 3:
					return input + "rd";
				default:
					return input + "th";
			}
		}
        public static double ToDouble(this decimal input)
        {
            return Convert.ToDouble(input);
        }

        public static decimal ToDecimal(this double input)
        {
            return Convert.ToDecimal(input);
        }

        public static int ToSafeInt(this long input)
        {
            try { return Convert.ToInt32(input); }
            catch { return 0; }
        }

        public static string ToBinary(this long input)
        {
            char[] bits = new char[32];
            int i = 0;

            while (input != 0)
            {
                bits[i++] = (input & 1) == 1 ? '1' : '0';
                input >>= 1;
            }

            Array.Reverse(bits, 0, i);
            return new string(bits);
        }

        public static string ToBinary(this int input)
        {
            return ToBinary(Convert.ToInt64(input));
        }


        // ==================================================
        // String
        // ==================================================
        public static int TrimmedLength(this string input)
        {
            if (input.IsNullOrWhiteSpace())
                return 0;
            else
                return input.Trim().Length;
        }

        public static bool HasText(this string input)
        {
            return !String.IsNullOrWhiteSpace(input) && input.Trim().Length > 0;
        }

        public static bool IsNumber(this string input)
        {
            if (input.IsNullOrWhiteSpace())
                return false;
            else
            {
                foreach (char c in input)
                {
                    if (!Char.IsDigit(c)) return false;
                }
                return true;
            }
        }

        public static bool IsNullOrWhiteSpace(this string input)
        {
            return String.IsNullOrWhiteSpace(input);
        }

        public static bool IsEqual(this string input, string match, bool ignoreCase = false)
        {
            return String.Equals(input, match, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture);
        }

        public static bool Contains(this string input, string value, bool ignoreCase = false)
        {
            return input.IndexOf(value, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture) > -1;
        }

        public static bool Contains(this string input, string[] values, bool ignoreCase = false)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (input.IndexOf(values[i], ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture) > -1) return true;
            }
            return false;
        }

        public static string Left(this string input, int length)
        {
            return input.Substring(0, length);
        }

        public static string Right(this string input, int length)
        {
            return input.Substring(input.Length - length);
        }

        public static string TrimCommaTrail(this string input)
        {
            if (input.IsNullOrWhiteSpace()) 
                return input;
            else
                return input.TrimEnd(' ', ',');
        }

        public static bool ToSafeBool(this string input)
        {
            if (input.IsNullOrWhiteSpace())
                return false;
            else if (input.IsNumber())
                return !(Convert.ToInt64(input) == 0);
            else
            {
                try { return Convert.ToBoolean(input); }
                catch { return false; }
            }
        }

        public static int ToSafeInt(this string input)
        {
            if (input.IsNullOrWhiteSpace())
                return 0;
            else if (input.IsNumber())
                return Convert.ToInt32(input);
            else
            {
                try { return Convert.ToInt32(input); }
                catch { return 0; }
            }
        }

        public static int? ToSafeIntNullable(this string input)        
        {
            int? id = null;
            if (!input.IsNullOrWhiteSpace()) id = ToSafeInt(input);
            return id;
        }

        public static string ToSafeString(this string input)
        {
            if (input.IsNullOrWhiteSpace())
                return "";
            else
                return input;
        }

		public static string ToUpperFirst(this string input)
		{
			if (string.IsNullOrWhiteSpace(input)) return string.Empty;
			StringBuilder text = new StringBuilder(input.Length);
			text.Append(Char.ToUpperInvariant(input[0]));
			for (int i = 1; i < input.Length; i++)
			{
				text.Append(input[i]);
			}
			return text.ToString();
		}

        public static string ToSpacePascalCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            StringBuilder text = new StringBuilder(input.Length);
            text.Append(input[0]);
            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]) && input[i - 1] != ' ') text.Append(' ');
                text.Append(input[i]);
            }
            return text.ToString();
        }
    }

	public class SafeConvert
	{
		//================================================================================
		#region Integer conversions

		/// <summary>
		/// Converts object to integer – defaults to 0 on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static int ToInt(object o, int d = 0)
		{
			if (o != null)
			{
				try { return Convert.ToInt32(o); }
				catch { }
			}
			return d;
		}


		/// <summary>
		/// Converts object to integer – defaults to null on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static int? ToIntNull(object o)
		{
			int? value;

			//check for null
			if (o == null)
			{
				value = null;
			}
			else
			{
				try
				{
					value = Convert.ToInt32(o);

				}
				catch //(InvalidCastException) //TODO RICH remove the InvalidCastException from eveywhere in common
				{
					value = null;
				}
			}

			return value;
		}

		#endregion
		//================================================================================

		//================================================================================
		#region DateTime conversions

		public static TimeSpan ToTimeSpan(object o)
		{
			TimeSpan value;

			try
			{
				string time = o.ToString();
				if (!time.Contains(":"))
				{
					int hours = SafeConvert.ToInt(o);
					if (hours > 23)
						time = String.Format("{0:0}:{1:0}:{2:0}", Math.Floor(hours / 24.0), hours % 24, 0);
					else
						time += ":0";
				}
				value = TimeSpan.Parse(time);
			}
			catch
			{
				value = new TimeSpan(0);
			}

			return value;
		}
		/// <summary>
		/// Converts object to DateTime – defaults to MinDate on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(object o)
		{
			DateTime value;

			//check for null
			if (o == null)
			{
				value = DateTime.MinValue;
			}
			else
			{
				try
				{
					value = Convert.ToDateTime(o);

				}
				catch //(InvalidCastException)
				{
					value = DateTime.MinValue;
				}
			}

			return value;
		}

		/// <summary>
		/// Converts object to DateTime – defaults to specified date on error.
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(object o, DateTime defaultValue)
		{
			DateTime value;

			//check for null
			if (o == null)
			{
				value = defaultValue;
			}
			else
			{
				try
				{
					value = Convert.ToDateTime(o);

				}
				catch //(InvalidCastException)
				{
					value = defaultValue;
				}
			}

			return value;
		}

		/// <summary>
		/// Converts object to DateTime – defaults to null on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static DateTime? ToDateTimeNull(object o)
		{
			DateTime? value;

			//check for null
			if (o == null)
			{
				value = null;
			}
			else
			{
				try
				{
					value = Convert.ToDateTime(o);

				}
				catch //(InvalidCastException)
				{
					value = null;
				}
			}

			return value;
		}
		#endregion
		//================================================================================

		//================================================================================
		#region Decimal conversions
		/// <summary>
		/// Converts object to decimal – defaults to 0 on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static decimal ToDecimal(object o)
		{
			decimal value;

			//check for null
			if (o == null)
			{
				value = 0;
			}
			else
			{
				try
				{
					value = Convert.ToDecimal(o);
				}
				catch //(InvalidCastException)
				{
					value = 0;
				}
			}

			return value;
		}

		/// <summary>
		/// Converts object to decimal – defaults to 0 on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static decimal? ToDecimalNull(object o)
		{
			Decimal? value;

			//check for null
			if (o == null)
			{
				value = null;
			}
			else
			{
				try
				{
					value = Convert.ToDecimal(o);

				}
				catch //(InvalidCastException)
				{
					value = null;
				}
			}

			return value;
		}
		#endregion
		//================================================================================

		//================================================================================
		#region Double conversions
		/// <summary>
		/// Converts object to double – defaults to 0 on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static double ToDouble(object o)
		{
			double value;

			//check for null
			if (o == null)
			{
				value = 0;
			}
			else
			{
				try
				{
					value = Convert.ToDouble(o);
				}
				catch //(InvalidCastException)
				{
					value = 0;
				}
			}

			return value;
		}

		/// <summary>
		/// Converts object to double – defaults to 0 on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static double? ToDoubleNull(object o)
		{
			Double? value;

			//check for null
			if (o == null)
			{
				value = null;
			}
			else
			{
				try
				{
					value = Convert.ToDouble(o);

				}
				catch //(InvalidCastException)
				{
					value = null;
				}
			}

			return value;
		}
		#endregion
		//================================================================================

		//================================================================================
		#region Currency conversions
		public static decimal ToMoney(object o)
		{
			string strValue;
			decimal dValue;

			//check for null
			if (o == null)
			{
				dValue = 0;
			}
			else
			{
				strValue = o.ToString();

				//strip legal characters
				strValue = strValue.Replace("$", "");
				strValue = strValue.Replace(",", "");

				try
				{
					dValue = Convert.ToDecimal(strValue);
				}
				catch //(InvalidCastException)
				{
					dValue = 0;
				}
			}

			return dValue;
		}

		public static decimal? ToMoneyNull(object o)
		{
			string strValue;
			decimal? dValue;

			//check for null
			if (o == null)
			{
				dValue = null;
			}
			else
			{
				strValue = o.ToString();

				//strip legal characters
				strValue = strValue.Replace("$", "");
				strValue = strValue.Replace(",", "");

				try
				{
					dValue = Convert.ToDecimal(strValue);
				}
				catch //(InvalidCastException)
				{
					dValue = null;
				}
			}

			return dValue;
		}
		#endregion
		//================================================================================

		//================================================================================
		#region Boolean conversions

		/// <summary>
		/// Returns boolean represenation of object or false on error
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool ToBoolean(object o)
		{
			try { return Convert.ToBoolean(o); }
			catch { return false; }
		}

		/// <summary>
		/// Converts a flag column to a boolean
		/// </summary>
		/// <param name="input">Assumes null on null, false on 0 and true on 1</param>
		/// <returns></returns>
		public static bool? ToBooleanNull(object o)
		{
			try
			{
				if (o == null)
					return null;
				else
					return Convert.ToBoolean(o);
			}
			catch { return null; }
		}

		#endregion
		//================================================================================

		//================================================================================
		#region String conversions

		public static string ToString(object s)
		{
			try { return Convert.ToString(s); }
			catch { return ""; }
		}

		#endregion
		//================================================================================
	}
}
