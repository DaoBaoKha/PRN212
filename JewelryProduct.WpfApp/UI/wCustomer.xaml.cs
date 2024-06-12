using JewelryProduct.Business;
using JewelryProduct.Data;
using JewelryProduct.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JewelryProduct.WpfApp.UI
{
    public partial class wCustomer : Window
    {
        private readonly ICustomerBusiness _customerBusiness;

        public wCustomer()
        {
            InitializeComponent();
            var context = new Net1810_212_6_JewelryProductContext();
            var unitOfWork = new UnitOfWork(context);
            this._customerBusiness = new CustomerBusiness(unitOfWork);
            this.LoadGrdCustomers();
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = await _customerBusiness.GetById(wCustomerID.Text.ToString());

                if (item.Data == null)
                {
                    var customer = new Customer()
                    {
                        Id = Int32.Parse(wCustomerID.Text),
                        CustomerName = wCustomerName.Text,
                        CustomerAddress = wCustomerAddress.Text,
                        CustomerPhone = wCustomerPhone.Text,
                    };

                    var result = await _customerBusiness.Save(customer);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var customer = item.Data as Customer;
                    customer.CustomerName = wCustomerName.Text;
                    customer.CustomerPhone = wCustomerPhone.Text;
                    customer.CustomerAddress = wCustomerAddress.Text;

                    var result = await _customerBusiness.Update(customer);
                    MessageBox.Show(result.Message, "Save");
                }

                ClearForm();
                await LoadGrdCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private async void grdCustomer_ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int customerId = Convert.ToInt32(btn.CommandParameter);

            if (customerId > 0)
            {
                var result = await _customerBusiness.GetById(customerId.ToString());

                if (result.Status > 0 && result.Data != null)
                {
                    var customer = result.Data as Customer;

                    // Update customer information in TextBoxes
                    wCustomerID.Text = customer.Id.ToString();
                    wCustomerName.Text = customer.CustomerName;
                    wCustomerPhone.Text = customer.CustomerPhone;
                    wCustomerAddress.Text = customer.CustomerAddress;
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            wCustomerID.Text = string.Empty;
            wCustomerName.Text = string.Empty;
            wCustomerPhone.Text = string.Empty;
            wCustomerAddress.Text = string.Empty;
        }

        private async Task LoadGrdCustomers()
        {
            var result = await _customerBusiness.GetAll();

            if (result.Status > 0 && result.Data != null)
            {
                grdCustomer.ItemsSource = result.Data as List<Customer>;
            }
            else
            {
                grdCustomer.ItemsSource = new List<Customer>();
            }
        }

        private async void grdCustomer_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.CommandParameter != null)
            {
                if (MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    int customerId;
                    if (int.TryParse(btn.CommandParameter.ToString(), out customerId))
                    {
                        var result = await _customerBusiness.DeleteById(customerId);
                        MessageBox.Show($"{result.Message}", "Delete");
                        await LoadGrdCustomers();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Customer ID", "Error");
                    }
                }
            }
        }

        private async void grdCustomer_ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
           
        }


        private void grdCustomer_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            var selectedCustomer = (Customer)grdCustomer.SelectedItem;
            if (selectedCustomer != null)
            {
                wCustomerID.Text = selectedCustomer.Id.ToString();
                wCustomerName.Text = selectedCustomer.CustomerName;
                wCustomerPhone.Text = selectedCustomer.CustomerPhone;
                wCustomerAddress.Text = selectedCustomer.CustomerAddress;
            }
        }

        private void grdCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
