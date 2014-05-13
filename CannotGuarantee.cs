using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch
{
	/// <summary>
	/// 対象のクラス、メンバーが、恒常的に使用できることを保証するものではない場合に使用します。
	/// </summary>
	public class CannotGuaranteeAttribute : Attribute
	{
		public CannotGuaranteeAttribute(string msg)
		{
			this.Message = msg;
		}

		public string Message { get; set; }
	}
}
