using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{

    public partial class MainWindow : Window
    {
        private HttpClient HttpClient { get; set; }
        private KeyValue keyValue;

        public MainWindow()
        {
            InitializeComponent();
            HttpClient = new();
            keyValue = new KeyValue();

            cmboxCommandName.Items.Add("GET");
            cmboxCommandName.Items.Add("PUT");
            cmboxCommandName.Items.Add("POST");
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtboxValue.Text == "" || txtboxKey.Text == "")
            {
                return;
            }

            try
            {
                keyValue.Value = txtboxValue.Text;
                keyValue.Key = Convert.ToInt32(txtboxKey.Text);



                switch (cmboxCommandName.SelectedIndex)
                {
                    case 0:
                        {
                            var response = await HttpClient.GetAsync($"http://localhost:27001/?key={Key}");

                            var content = await response.Content.ReadAsStringAsync();

                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = JsonSerializer.Deserialize<KeyValue>(content);
                                Value = result.Value;
                                Key = string.Empty;
                                Key += result.Key;
                            }
                            else
                                MessageBox.Show(response.StatusCode.ToString());
                            break;
                        }
                    case 1:
                        {
                            var jsonStr = JsonSerializer.Serialize(keyValue);

                            var content = new StringContent(jsonStr);

                            var response = await HttpClient.PutAsync("http://localhost:27001/", content);

                            if (response.StatusCode == HttpStatusCode.OK)
                                MessageBox.Show("Putted Succesfully");
                            else
                                MessageBox.Show("Key doesn't exist");
                            break;
                        }
                    case 2:
                        {
                            var jsonStr = JsonSerializer.Serialize(keyValue);

                            var content = new StringContent(jsonStr);

                            var response = await HttpClient.PostAsync("http://localhost:27001/", content);

                            if (response.StatusCode == HttpStatusCode.OK)
                                MessageBox.Show("Posted Succesfully");
                            else
                                MessageBox.Show("Already Exists");
                            break;
                        }
                    default:
                        break;
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
    }
}
