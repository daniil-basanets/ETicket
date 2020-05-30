using System;
using System.Reflection;
using System.Resources;
using ETicketMobile.UserInterface.Localization.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ETicketMobile.Localizations.Extensions
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        #region Fields

        private readonly ILocalize localize;
        private const string ResourceId = "ETicketMobile.Resources.AppResource";

        #endregion

        #region Properties

        public string Text { get; set; }

        #endregion

        public TranslateExtension()
        {
            localize = DependencyService.Get<ILocalize>();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return string.Empty;

            var resourceManager = new ResourceManager(ResourceId,
                    typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = resourceManager.GetString(Text, localize.CurrentCulture);

            return translation ?? Text;
        }
    }
}