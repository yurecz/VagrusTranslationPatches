using Epic.OnlineServices.Auth;
using System.ComponentModel;
using UnityEngine;
using Vagrus.Data;

namespace VagrusTranslationPatches.Utils
{
    public static class PropertyUtils
    {
        public static string GetDescriptionTemplate(this Property property)
        {
            AirtableLocalizedData data;
            return Game.game.caravan.IsOpenUI(MenuType.VagrusCreation) ? Game.FormatText((property.descriptionVagrusCreate.Length > 0) ? property.descriptionVagrusCreate : property.description, makeCodexLinks: false) : ((!Game.GetLocalization(DataType.Property, property.ID, out data) || data.Description.Length <= 0) ? Game.FormatText(property.description, makeCodexLinks: false) : Game.FormatText(data.Description, makeCodexLinks: false));

        }

    }

}
