using System;
using System.Runtime.InteropServices;

namespace wpfMapChk
{
	class ScreenSaver
	{
		private enum SPI : uint
		{
			SPI_GETSCREENSAVEACTIVE = 0x0010,
			SPI_SETSCREENSAVEACTIVE = 0x0011
		}
		private enum SPIF : uint
		{
			None = 0x00,
			SPIF_UPDATEINIFILE = 0x01,
			SPIF_SENDCHANGE = 0x02,
			SPIF_SENDWININICHANGE = 0x02
		}
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, ref uint pvParam, SPIF fWinIni);
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, uint pvParam, SPIF fWinIni);

		/// <summary> 
		/// 關閉螢幕保護程式 
		/// </summary> 
		public static void Disable()
		{
			SystemParametersInfo(SPI.SPI_SETSCREENSAVEACTIVE, 0, 0, SPIF.None);
		}

		/// <summary> 
		/// 啟動螢幕保護程式 
		/// </summary> 
		public static void Enable()
		{
			SystemParametersInfo(SPI.SPI_SETSCREENSAVEACTIVE, 1, 0, SPIF.None);
		}

		/// <summary> 
		/// 檢查是否有螢幕保護程式 
		/// </summary> 
		/// <returns>true:有,false:沒有</returns> 
		public static bool Check()
		{
			uint isActive = 0;
			SystemParametersInfo(SPI.SPI_GETSCREENSAVEACTIVE, 0, ref isActive, SPIF.None);
			return (isActive == 0) ? false : true;
		} 
	}
}
