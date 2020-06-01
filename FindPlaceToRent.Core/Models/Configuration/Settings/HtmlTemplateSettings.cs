namespace FindPlaceToRent.Core.Models.Configuration
{
    /// <summary>
    /// Settings about the html template to send via email.
    /// </summary>
    public class HtmlTemplateSettings
    {
        /// <summary>
        /// The directory of the .html file.
        /// </summary>
        public string FileDirectory { get; set; }
        
        /// <summary>
        /// Name of the Url variable in the html template.
        /// </summary>
        public string UrlVarName { get; set; }

        /// <summary>
        /// Name of the TitleAreaPrice variable in the html template.
        /// </summary>
        public string TitleAreaPriceVarName { get; set; }

        /// <summary>
        /// Name of the Location variable in the html template.
        /// </summary>
        public string LocationVarName { get; set; }

        /// <summary>
        /// Name of the Characteristics variable in the html template.
        /// </summary>
        public string CharacteristicsVarName { get; set; }
    }
}
