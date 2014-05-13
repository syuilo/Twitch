using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Utility.Twitter
{
	/// <summary>
	/// 画像を扱うクラスです。
	/// </summary>
	public class Image
	{
		/// <summary>
		/// 画像を扱うクラスです。
		/// </summary>
		/// <param name="path">画像ファイルへのパス</param>
		public Image(string path = null)
		{
			this.Path = path;
		}

		/// <summary>
		/// 画像ファイルのパスを取得または設定します。
		/// </summary>
		public string Path
		{
			get;
			set;
		}

		/// <summary>
		/// 画像選択ダイアログを表示し、画像を選択させます。
		/// </summary>
		/// <param name="title">ダイアログのタイトル</param>
		/// <param name="filter">フィルタ文字列</param>
		/// <param name="restoreDirectory"></param>
		public void ShowSelectImageDialog(
			string title = "画像を選択してください",
			string filter = "画像ファイル (*.png;*.jpg;*.jpeg;*.jpe;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.jpe;*.bmp;*.gif",
			bool restoreDirectory = true)
		{
			System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog()
			{
				Title = title,
				Filter = filter,
				RestoreDirectory = restoreDirectory,
			};

			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				this.Path = ofd.FileName;
		}

		/// <summary>
		/// 画像をBase64文字列にエンコードします。
		/// </summary>
		/// <returns></returns>
		public string ToBase64String()
		{
			if (this.Path != null)
			{
				string inFileName = this.Path;
				System.IO.FileStream inFile;
				byte[] bs;

				inFile = new System.IO.FileStream(inFileName,
					System.IO.FileMode.Open, System.IO.FileAccess.Read);
				bs = new byte[inFile.Length];
				int readBytes = inFile.Read(bs, 0, (int)inFile.Length);
				inFile.Close();

				string base64String;
				base64String = System.Convert.ToBase64String(bs);

				return base64String;
			}
			else
				throw new NullReferenceException("画像ファイルへのパスが設定されていません。");
		}
	}
}
