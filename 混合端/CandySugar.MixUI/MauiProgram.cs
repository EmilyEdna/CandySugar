namespace CandySugar.MixUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("FontAwesome6Brands.otf", "Brand");
                fonts.AddFont("FontAwesome6Regular.otf", "Regular");
                fonts.AddFont("FontAwesome6Solid.otf", "Solid");
                fonts.AddFont("FontAwesome6Thin.otf", "Thin");
            });

            return builder.Build();
        }
    }
}