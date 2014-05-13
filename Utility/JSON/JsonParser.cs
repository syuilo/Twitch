using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Twitch.Utility
{
	public static class JsonParser
	{
		static string data;
		static int pos, len;
		static Regex reg = new Regex(@"(\\u){1}[0-9a-fA-F]{4}");

		/// <summary>
		/// JSON文字列をパースしてobjectを返します。
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		public static object Parse(string json)
		{
			data = json;
			len = json.Length;

			for (pos = 0; pos < len; pos++)
			{
				if (data[pos] == '[')
					return parseArray();
				else if (data[pos] == '{')
					return parseObject();
			}

			return null;
		}

		/// <summary>
		/// 配列のパース
		/// </summary>
		/// <returns></returns>
		static object parseArray()
		{
			List<object> array = new List<object>();

			for (pos++; pos < len; pos++)
			{
				if (data[pos] == ' ' || data[pos] == '\t' || data[pos] == '\r' || data[pos] == '\n' || data[pos] == ',')
					continue;
				else if (data[pos] == ']')
					break;
				else if (data[pos] == '[')
					array.Add(parseArray());
				else if (data[pos] == '{')
					array.Add(parseObject());
				else
					array.Add(parseValue());
			}

			return array.ToArray();
		}

		/// <summary>
		/// オブジェクトのパース
		/// </summary>
		/// <returns></returns>
		static object parseObject()
		{
			Dictionary<string, object> obj = new Dictionary<string, object>();
			string key = null;

			for (pos++; pos < len; pos++)
			{
				if (data[pos] == ' ' || data[pos] == '\t' || data[pos] == '\r' || data[pos] == '\n' || data[pos] == ',' || data[pos] == ':')
					continue;
				else if (data[pos] == '}')
					break;
				else if (data[pos] == '[')
				{
					obj[key] = parseArray();
					key = null;
				}
				else if (data[pos] == '{')
				{
					obj[key] = parseObject();
					key = null;
				}
				else
				{
					if (key == null)
						key = parseString();
					else
					{
						obj[key] = parseValue();
						key = null;
					}
				}
			}

			return obj;
		}

		/// <summary>
		/// 配列、オブジェクト以外の値のパース
		/// </summary>
		/// <returns></returns>
		static object parseValue()
		{
			if (data[pos] == '"')
			{
				return parseString();
			}
			if (data[pos] == 't')
			{
				pos += 3;
				return true;
			}
			if (data[pos] == 'f')
			{
				pos += 4;
				return false;
			}
			if (data[pos] == 'n')
			{
				pos += 3;
				return null;
			}

			string str = "";
			for (; pos < len; pos++)
			{
				if (data[pos] == ',' || data[pos] == ':' || data[pos] == '}' || data[pos] == ']')
					break;
				str += data[pos];
			}
			pos--;

			long a;
			if (long.TryParse(str, out a))
				return a;

			return double.Parse(str);

		}

		/// <summary>
		/// 文字列のパース
		/// </summary>
		/// <returns></returns>
		static string parseString()
		{
			string str = "";

			for (pos++; pos < len; pos++)
			{
				if (data[pos] == '\\' && data[pos + 1] != 'u')
				{
					str += data[pos + 1];
					pos++;
				}
				else if (data[pos] == '"')
					break;
				else
					str += data[pos];
			}

			return reg.Replace(str, (s) => Convert.ToChar(Convert.ToInt32(s.Value.Substring(2), 16)).ToString());
		}

		//#region old

		//private static string json;
		//private static int len, pos;
		//private static Regex reg = new Regex(@"(\\u){1}[0-9a-fA-F]{4}");

		///// <summary>
		///// JSON文字列をパースします。
		///// </summary>
		///// <param name="JSON">JSON</param>
		///// <returns>object</returns>
		//public static object ParseJson(string JSON)
		//{
		//	json = JSON;
		//	len = json.Length;
		//	pos = 0;

		//	return AnalyzeJson();
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//private static object AnalyzeJson()
		//{
		//	for (pos = 0; pos < len; pos++)
		//	{
		//		switch (json[pos])
		//		{
		//			case '[':
		//				return parseArray();
		//			//break;
		//			case '{':
		//				return parseObject();
		//			//break;
		//		}
		//	}

		//	//throw new FormatException("不正なJSONです。");
		//	return null;
		//}

		///// <summary>
		///// 配列のパース
		///// </summary>
		///// <returns></returns>
		//private static object parseArray()
		//{
		//	List<object> array = new List<object>();

		//	for (pos++; pos < len; pos++)
		//	{
		//		switch (json[pos])
		//		{
		//			case ']':
		//				goto END;
		//			//break;
		//			case '[':
		//				array.Add(parseArray());
		//				break;
		//			case '{':
		//				array.Add(parseObject());
		//				break;
		//			default:
		//				array.Add(parseValue());
		//				break;
		//		}
		//	}

		//END:

		//	return array.ToArray();
		//}

		///// <summary>
		///// オブジェクトのパース
		///// </summary>
		///// <returns></returns>
		//private static object parseObject()
		//{
		//	Dictionary<string, object> obj = new Dictionary<string, object>();
		//	string key = null;

		//	for (pos++; pos < len; pos++)
		//	{
		//		switch (json[pos])
		//		{
		//			case '}':
		//				goto END;
		//				//break;
		//			case '[':
		//				obj[key] = parseArray();
		//				key = null;
		//				break;
		//			case '{':
		//				obj[key] = parseObject();
		//				key = null;
		//				break;
		//			default:
		//				if (key == null)
		//					key = parseString();
		//				else
		//				{
		//					obj[key] = parseValue();
		//					key = null;
		//				}
		//				break;
		//		}
		//	}

		//END:

		//	return obj;
		//}

		///// <summary>
		///// その他のパース
		///// </summary>
		///// <returns></returns>
		//private static object parseValue()
		//{
		//	switch (json[pos])
		//	{
		//		case '"':
		//			return parseString();
		//			//break;
		//		case 't':
		//			pos += ("true".Length - 1);
		//			return true;
		//			//break;
		//		case 'f':
		//			pos += ("false".Length - 1);
		//			return false;
		//			//break;
		//		case 'n':
		//			pos += ("null".Length - 1);
		//			return null;
		//			//break;
		//	}

		//	string str = "";

		//	for (; pos < len; pos++)
		//	{
		//		if (json[pos] == ',' || json[pos] == ':' || json[pos] == '}' || json[pos] == ']')
		//			break;
		//		str += json[pos];
		//	}

		//	pos--;

		//	long a;
		//	if (long.TryParse(str, out a))
		//		return a;

		//	return double.Parse(str);
		//}

		///// <summary>
		///// 文字列のパース
		///// </summary>
		///// <returns></returns>
		//private static string parseString()
		//{
		//	string str = "";

		//	for (pos++; pos < len; pos++)
		//	{
		//		if (json[pos] == '\\' && json[pos + 1] != 'u')
		//		{
		//			str += json[pos + 1];
		//			pos++;
		//		}
		//		else if (json[pos] == '"')
		//			break;
		//		else
		//			str += json[pos];
		//	}

		//	return reg.Replace(str, (s) => Convert.ToChar(Convert.ToInt32(s.Value.Substring(2), 16)).ToString());
		//}


		//#endregion
	}
}
