namespace MauiAppWithWebViewIntegration
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            MyWebView.Navigating += OnWebViewNavigating;
        }

        private void OnCounterClicked(object sender, EventArgs e)
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
                    <h1>Enter Text Below:</h1>
                    <input type='text' id='textbox' placeholder='Type here...' style='width:100%;padding:10px;font-size:16px;'>
                    <button onclick='sendToApp()' style='margin-top:10px;padding:10px;font-size:16px;'>Send to App</button>

                    <script>
                        function sendToApp() {
                            const text = document.getElementById('textbox').value;
                            window.location.href = 'invoke:' + encodeURIComponent(text); // Use a custom scheme
                        }
                    </script>
                </body>
                </html>"
            };

            MyWebView.Source = htmlSource;
        }

        private void OnWebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.StartsWith("invoke:"))
            {
                e.Cancel = true; // Prevent the WebView from actually navigating
                string message = Uri.UnescapeDataString(e.Url.Substring("invoke:".Length));
                OnMessageReceived(message);
            }
        }

        private void OnMessageReceived(string message)
        {
            DisplayAlert("Text Received from WebView", message, "OK");
        }
    }
}
