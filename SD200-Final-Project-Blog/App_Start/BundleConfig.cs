﻿using System.Web.Optimization;

namespace SD200_Final_Project_Blog
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            // MY BUNDLES
            bundles.Add(new ScriptBundle(@"~/TemplateContent/js").Include(
                @"~/TemplateContent/vendor/@fancyapps/fancybox/jquery.fancybox.min.js",
                @"~/TemplateContent/vendor/jquery.cookie/jquery.cookie.js",
                @"~/TemplateContent/js/front.js",
                @"~/TemplateContent/js/MyJavaScript.js"));

            bundles.Add(new StyleBundle(@"~/TemplateContent/css").Include(
                @"~/TemplateContent/vendor/font-awesome/css/font-awesome.min.css",
                @"~/TemplateContent/css/fontastic.css",
                @"~/TemplateContent/vendor/@fancyapps/fancybox/jquery.fancybox.min.css",
                @"~/TemplateContent/css/style.blue.css",
                @"~/TemplateContent/css/custom.css"));

            bundles.Add(new ScriptBundle("~/tinymce").Include(
                @"~/Scripts/tinymce/tinymce.min.js",
                @"~/Scripts/tinymce/jquery.tinymce.min.js",
                @"~/Scripts/myScripts/tinyMCE.js",
                @"~/Scripts/myScripts/styleFileBtn.js"));

            bundles.Add(new StyleBundle(@"~/createEditPost").Include(
                @"~/Content/myStyles/createEditPost.css"));

            bundles.Add(new StyleBundle(@"~/index").Include(
                @"~/Content/myStyles/index.css"));
        }
    }
}
