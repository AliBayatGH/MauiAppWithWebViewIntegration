namespace MauiAppWithWebViewIntegration
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            MyWebView.Navigating += OnWebViewNavigating;
        }

        private void OnWebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.StartsWith("call:"))
            {
                e.Cancel = true; // Prevent the WebView from actually navigating
                string message = Uri.UnescapeDataString(e.Url.Substring("call:".Length));
                Call(message);
            }
            else if (e.Url.StartsWith("map:"))
            {
                e.Cancel = true; // Prevent the WebView from actually navigating
                string message = Uri.UnescapeDataString(e.Url.Substring("map:".Length));
                OpenMap(message);
            }
        }

        private void OpenMap(string location)
        {
            try
            {
                var encodedLocation = Uri.EscapeDataString(location);
                var mapsUri = $"https://www.google.com/maps/search/?api=1&query={encodedLocation}";
                Launcher.OpenAsync(new Uri(mapsUri));
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Unable to open Google Maps: {ex.Message}", "OK");
            }
        }

        private void Call(string phoneNumber)
        {
            //DisplayAlert("Text Received from WebView", phoneNumber, "OK");
            if (PhoneDialer.IsSupported)
            {
                try
                {
                    PhoneDialer.Open(phoneNumber);
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", $"Unable to make a call: {ex.Message}", "OK");
                }
            }
            else
            {
                DisplayAlert("Error", "Phone Dialer is not supported on this device.", "OK");
            }

        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            var htmlSource = new HtmlWebViewSource
            {
                Html = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>WebView Example</title>
                </head>
                <body>
                    <input type='text' id='textbox' placeholder='Type here...' style='width:100%;padding:10px;font-size:16px;'>
                    <button onclick='sendToApp()' style='margin-top:10px;padding:10px;font-size:16px;'>Call</button>
                    <button onclick='openMap()' style='margin-top:10px;padding:10px;font-size:16px;'>Open Google Map</button>

                    <script>
                        function sendToApp() {
                            const text = document.getElementById('textbox').value;
                            window.location.href = 'call:' + encodeURIComponent(text);
                        }
                function openMap() {
                    const text = document.getElementById('textbox').value;
                    window.location.href = 'map:' + encodeURIComponent(text);
                }
                    </script>
                </body>
                </html>"
            };

            MyWebView.Source = htmlSource;
        }
    }
}
