using Volo.Abp.Settings;

namespace Washyn.UNAJ.Lot.Settings
{
    public class EmailSettingProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            // context.Add(
            //     new SettingDefinition("Smtp.Host", "127.0.0.1"),
            //     new SettingDefinition("Smtp.Port", "25"),
            //     new SettingDefinition("Smtp.UserName"),
            //     new SettingDefinition("Smtp.Password", isEncrypted: true),
            //     new SettingDefinition("Smtp.EnableSsl", "false")
            // );

            var test = Volo.Abp.Localization.LocalizationSettingNames.DefaultLanguage;

            context.Add(new SettingDefinition("YearName"));

        }
    }

}