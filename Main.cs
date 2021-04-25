using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Localization;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;

namespace VN.NewFont
{
	public class Main : MBSubModuleBase
	{
		public static string modulePath
		{
			get
			{
				return BasePath.Name + "Modules/VietnamLanguage/";
			}
		}
		private static string _fontsDir
		{
			get
			{
				return Main.modulePath + "GUI/GauntletUI/Fonts/";
			}
		}
		private void LoadFont(string fontName, string fontMap)
		{
			try
			{
				SpriteData spriteData = UIResourceManager.SpriteData;
				SpriteCategory spriteCategory = new SpriteCategory("ui_fonts", spriteData, 1);
				spriteCategory.SheetSizes = new Vec2i[]
				{
					new Vec2i(8192, 8192)
				};
				spriteCategory.Load(UIResourceManager.ResourceContext, UIResourceManager.UIResourceDepot);
				SpritePart spritePart = new SpritePart(fontName, spriteCategory, 8192, 8192)
				{
					SheetID = 1
				};
				new SpriteGeneric(fontName, spritePart);
				Font font = UIResourceManager.FontFactory.GetFont(fontName);
				if (font.Name != fontName)
				{
					UIResourceManager.FontFactory.AddFontDefinition(
						Main._fontsDir + fontName + "/",
						fontName,
						spriteData
					);
				}
				font = UIResourceManager.FontFactory.GetFont(fontName);
				font.GetType().GetProperty("FontSprite").SetValue(font, spritePart);
				TaleWorlds.Engine.Texture engineTexture = TaleWorlds.Engine.Texture.LoadTextureFromPath(fontMap, Main._fontsDir + fontName);
				font.FontSprite.Category.SpriteSheets[font.FontSprite.SheetID - 1] = new Texture(new EngineTexture(engineTexture));
			} catch (Exception e)
			{
				InformationManager.DisplayMessage(
					new InformationMessage(e.Message)
					);
			}
		}

		private void LoadCustomFonts()
		{
			foreach (string text in Config.GetAllFontWasDefinedInIniFile("NewFonts", "VN.NewFontConfig.ini"))
			{
				string fontName = text.Substring(0, text.IndexOf("="));
				string fontMap = text.Substring(text.IndexOf("=") + 1);
				this.LoadFont(fontName, fontMap);
			}
			// Load module language definition 
			string moduleLangXmlPath = Main._fontsDir + "Languages.xml";
			UIResourceManager.FontFactory.LoadLocalizationValues(moduleLangXmlPath);
		}

		protected override void OnBeforeInitialModuleScreenSetAsRoot()
		{
			TextObject greetingMsg = new TextObject("{=vadu_greeting_text}Vietnamese Language Module is enabled. \nYou can change the language of game by Options.", null);
			InformationManager.DisplayMessage(new InformationMessage(greetingMsg.ToString(), Color.FromUint(0xf48024)));
		}
		protected override void OnSubModuleLoad()
		{
			this.LoadCustomFonts();
			/*
			ResourceDepot resourceDepot = new ResourceDepot(BasePath.Name);
			resourceDepot.AddLocation("Modules/VietnamLanguage/GUI/GauntletUI/");
			resourceDepot.CollectResources();
			SpriteData spriteData = new SpriteData("GalahadVN");
			spriteData.Load(resourceDepot);
			// UIResourceManager.FontFactory.LoadAllFonts(spriteData);

			string path = Main._fontsDir + "GalahadVN/";
			UIResourceManager.FontFactory.AddFontDefinition(path, "GalahadVN", spriteData);
			string moduleLangXmlPath = Main._fontsDir + "Languages.xml";
			UIResourceManager.FontFactory.LoadLocalizationValues(moduleLangXmlPath);*/

			var currentLanguage = BannerlordConfig.Language;
			bool flag = currentLanguage.Contains("Tiếng Việt");
			Module.CurrentModule.AddInitialStateOption( new InitialStateOption(
				"TranslatorCredit",
				new TextObject("{=vadu_credit_btn_txt}Set game language to Vietnamese", null ),
				9990,
				() => {
					if (!flag)
					{
						BannerlordConfig.Language = "Tiếng Việt";
						ManagedOptions.OnManagedOptionChanged(ManagedOptions.ManagedOptionsType.Language);
						BannerlordConfig.Save();
					}
					InformationManager.DisplayMessage(
						new InformationMessage(
							new TextObject("{=vadu_clicked_btn_msg}Module created by LTC. \nLibrary source shared by Ha ThanhKS.\n Enjoy!", null).ToString()
						)
					);
				},
				new Func<bool>(() => { return false; })
				) );
		}
	}
}
