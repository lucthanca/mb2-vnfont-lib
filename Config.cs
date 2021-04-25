using System;
using System.Runtime.InteropServices;

namespace VN.NewFont
{
	public class Config
	{
		public static string[] GetAllFontWasDefinedInIniFile(string strSectionName, string strIniFileName)
		{
			string lpFileName = Main.modulePath + "bin/Win64_Shipping_Client/" + strIniFileName;
			string text = new string('\0', 2048);
			Config.GetPrivateProfileSection(strSectionName, text, (uint)text.Length, lpFileName);
			return text.Trim(new char[1]).Split(new char[1]);
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern uint GetPrivateProfileSection(string lpAppName, string lpReturnedString, uint nSize, string lpFileName);
	}
}
