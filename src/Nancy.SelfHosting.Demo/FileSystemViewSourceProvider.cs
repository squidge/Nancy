namespace Nancy.SelfHosting.Demo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Nancy.ViewEngines;

    public class FileSystemViewSourceProvider : IViewSourceProvider
    {
        public ViewLocationResult LocateView(string viewName, IEnumerable<string> supportedViewEngineExtensions)
        {
            var viewFolder =
                Path.Combine(Environment.CurrentDirectory, "views");

            if (string.IsNullOrEmpty(viewFolder))
                return null;

            var filesInViewFolder =
                Directory.GetFiles(viewFolder);

            var viewsFiles =
                from file in filesInViewFolder
                from extension in supportedViewEngineExtensions
                where Path.GetFileName(file).Equals(string.Concat(viewName, ".", extension), StringComparison.OrdinalIgnoreCase)
                select new
                {
                    file,
                    extension
                };

            var selectedView =
                viewsFiles.FirstOrDefault();

            return new ViewLocationResult(
                selectedView.file,
                selectedView.extension,
                new StreamReader(selectedView.file)
            );
        }
    }
}