using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using ababyc.Models;
using ababyc.Services;

namespace ababyc
{
	public class ResourcesService
	{
		static string s_defaultSSML;
		static ResourceManager s_resourceManager = new ResourceManager("ababyc.Data.Languages.ResourcesBabySmash", typeof(ILanguageService).GetTypeInfo().Assembly);

		public ResourcesService()
		{
			GetDefaultSSML();
		}

		public string GetLanguageTextForLetter(string letter)
		{
			return string.Format(s_defaultSSML, letter);
		}

		public string GetLanguageTextForShape(ShapeType shape, string locale)
		{
			var key = shape.ToString().ToLower();
			return GetResourceText(key, locale);
		}

		public string GetResourceText(string key, string locale)
		{
			string result = s_resourceManager.GetString(key, new CultureInfo(locale));
			return result;
		}

		static void GetDefaultSSML()
		{
			var assembly = typeof(ILanguageService).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("ababyc.Data.ssml_character_default.xml");
			using (var reader = new StreamReader(stream))
			{
				s_defaultSSML = reader.ReadToEnd();
			}
		}
	}
}
