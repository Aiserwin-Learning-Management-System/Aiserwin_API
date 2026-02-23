namespace Winfocus.LMS.Application.Helpers
{
    /// <summary>
    /// EmailTemplateRenderer.
    /// </summary>
    public static class EmailTemplateRenderer
    {
        /// <summary>
        /// Renders the specified template path.
        /// </summary>
        /// <param name="templatePath">The template path.</param>
        /// <param name="replacements">The replacements.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="FileNotFoundException">Email template not found: {templatePath}</exception>
        public static string Render(
            string templatePath,
            Dictionary<string, string> replacements)
        {
            var fullPath = Path.Combine(
                AppContext.BaseDirectory,
                "EmailTemplates",
                templatePath);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(
                    $"Email template not found: {templatePath}");
            }

            var html = File.ReadAllText(fullPath);

            foreach (var item in replacements)
            {
                html = html.Replace(
                    $"{{{{{item.Key}}}}}",
                    item.Value);
            }

            return html;
        }
    }
}
