using AppForSEII2526.UIT.CU_CompraDispositivo;
using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CUContratarReparacion
{
    public class CUContractRepair_UIT : IDisposable
    {
        IWebDriver _driver;
        string _URI;
        private readonly ITestOutputHelper _output;

        public CUContractRepair_UIT(ITestOutputHelper output)
        {
            UtilitiesUIT.SetUp_UIT(out _driver, out _URI);
            _output = output;
        }

        public void Dispose()
        {
            try {
                _driver.Quit(); // Quit closes all windows and ends the session safely
                _driver.Dispose();
            } catch (Exception) { 
                // Ignore errors during disposal
            }
            // Give system time to release socket
            System.Threading.Thread.Sleep(1000);
            GC.SuppressFinalize(this);
        }

        private void InitialNavigation()
        {
            _driver.Navigate().GoToUrl(_URI + "repairs/selectrepairs");
        }

        [Fact]
        public void UC1_1_Esc1_ProcessMultipleRepairs()
        {
            // Esc-1: Screen repair (Medium, 50€), Battery repair (Low, 80€) -> Total 130€
            var selectPO = new SelectRepairsForReceipt_PO(_driver, _output);
            var createPO = new CreateRepair_PO(_driver, _output);
            var detailPO = new DetailReceipt_PO(_driver, _output);

            InitialNavigation();

            // 1. Select Repairs
            selectPO.SelectRepairs(new List<string> { "Screen repair", "Battery repair" });
            selectPO.ProceedToContract();

            // 2. Fill Data
            createPO.SetName("Carlos");
            createPO.SetSurname("García");
            createPO.SetAddress("Calle Luna 7");
            createPO.SetPaymentMethod("CreditCard");

            // Set models (Order depends on selection/list order. Assuming Screen is first, Battery second)
            createPO.SetModelByIndex(0, "iPhone 14");
            createPO.SetModelByIndex(1, "Samsung Galaxy S23");

            createPO.SubmitContract();

            // 3. Validate Receipt
            // Expected: Carlos García, Calle Luna 7, Total 130€
            // Razor DetailPrint: @item.Cost € (With space)
            var expectedItems = new List<string[]> {
                new string[] { "Screen repair", "Medium", "iPhone 14", "50 €" },
                new string[] { "Battery repair", "Low", "Samsung Galaxy S23", "80 €" }
            };
            
            Assert.True(detailPO.CheckReceiptItems(expectedItems));
            Assert.Contains("130", _driver.PageSource); // Total Check (Flexible for 130 or 130,00)
            Assert.Contains("Carlos", _driver.PageSource);
            Assert.Contains("Luna 7", _driver.PageSource);
        }

        [Fact]
        public void UC1_2_Esc2_FilterAndContract()
        {
            // Esc-2: Filter Scale="Low", Select "Battery repair"
            var selectPO = new SelectRepairsForReceipt_PO(_driver, _output);
            var createPO = new CreateRepair_PO(_driver, _output);
            var detailPO = new DetailReceipt_PO(_driver, _output);

            InitialNavigation();

            // Filter
            selectPO.FilterRepairs(null, "Low");
            
            // Check filtered list contains Battery repair 
            // SelectRepairs Razor: @repair.Cost€ (No space)
            var expectedList = new List<string[]> {
                new string[] { "Battery repair", "Low", "Battery replacement", "80€" }
            };
            Assert.True(selectPO.CheckRepairsList(expectedList));

            // Select
            selectPO.SelectRepairs(new List<string> { "Battery repair" });
            selectPO.ProceedToContract();

            // Fill
            createPO.SetName("Laura");
            createPO.SetSurname("Martínez");
            createPO.SetAddress("Calle Sol 23");
            createPO.SetPaymentMethod("PayPal");
            createPO.SetModelByIndex(0, "Samsung Galaxy S23"); // Only 1 item

            createPO.SubmitContract();

            // Validate
            var expectedDetail = new List<string[]> {
                new string[] { "Battery repair", "Low", "Samsung Galaxy S23", "80 €" }
            };
            Assert.True(detailPO.CheckReceiptItems(expectedDetail));
            Assert.True(detailPO.CheckClientInfo("Laura", "Martínez", "Calle Sol 23"));
        }

        [Fact]
        public void UC1_3_Esc3_RemoveLoop()
        {
            // Esc-3: Initial: Screen, Battery, Hardware. Remove Hardware.
            var selectPO = new SelectRepairsForReceipt_PO(_driver, _output);
            var createPO = new CreateRepair_PO(_driver, _output);
            var detailPO = new DetailReceipt_PO(_driver, _output);

            InitialNavigation();

            // Select 3 repairs
            selectPO.SelectRepairs(new List<string> { "Screen repair", "Battery repair", "Hardware repair" });
            
            // Remove Hardware repair
            selectPO.RemoveRepairFromCart("Hardware repair");

            selectPO.ProceedToContract();

            // Fill Data
            createPO.SetName("Carlos");
            createPO.SetSurname("García");
            createPO.SetAddress("Calle Mayor 1");
            createPO.SetPaymentMethod("Cash");

            createPO.SetModelByIndex(0, "iPhone 14");
            createPO.SetModelByIndex(1, "Samsung Galaxy S23");

            createPO.SubmitContract();

            // Validate Total 130€ (Screen 50 + Battery 80), Hardware removed
            Assert.Contains("130", _driver.PageSource);
            
            var expectedItems = new List<string[]> {
                new string[] { "Screen repair", "Medium", "iPhone 14", "50 €" },
                new string[] { "Battery repair", "Low", "Samsung Galaxy S23", "80 €" }
            };
            Assert.True(detailPO.CheckReceiptItems(expectedItems));
        }

        [Fact]
        public void UC1_4_Esc4_GoBackAndPersist()
        {
            // Esc-4: Screen repair -> Fill data -> Go Back -> Verify data persists
            var selectPO = new SelectRepairsForReceipt_PO(_driver, _output);
            var createPO = new CreateRepair_PO(_driver, _output);

            InitialNavigation();
            selectPO.SelectRepairs(new List<string> { "Screen repair" });
            selectPO.ProceedToContract();

            // Fill Data
            createPO.SetName("Laura");
            createPO.SetSurname("Martínez");
            createPO.SetAddress("Calle Sol 23");
            createPO.SetPaymentMethod("PayPal");

            // Go Modify
            createPO.GoBackToModify();

            // Verify we are back (check if contract button exists)
            Assert.True(selectPO.IsContractButtonEnabled());

            // User can add more repairs... let's just go forward again to check persistence
            selectPO.ProceedToContract();

            // Check if fields still have values
            Assert.Equal("Laura", createPO.GetNameValue());
            Assert.Equal("Calle Sol 23", createPO.GetAddressValue());
        }

        [Fact]
        public void UC1_5_Esc5_NoRepairsAvailable()
        {
            // Esc-5: Filter for something impossible creates "No repairs available"
            var selectPO = new SelectRepairsForReceipt_PO(_driver, _output);
            
            InitialNavigation();
            selectPO.FilterRepairs("NonExistentRepairNameXYZ", null);

            Assert.Contains("There are no repairs available", _driver.PageSource);
        }

        [Fact]
        public void UC1_6_Esc6_EmptyCart()
        {
            // Esc-6: Empty cart -> Contract button disabled/Hidden, Total 0
            var selectPO = new SelectRepairsForReceipt_PO(_driver, _output);

            InitialNavigation();

            Assert.False(selectPO.IsContractButtonEnabled());
            // Need to verify if total 0 is visible or inferred by button state
             // Assuming default is 0
        }

        [Fact]
        public void UC1_7_Esc7_ValidationErrors()
        {
            // Esc-7: Screen repair -> Fill only Name and Model -> Submit -> Errors
            var selectPO = new SelectRepairsForReceipt_PO(_driver, _output);
            var createPO = new CreateRepair_PO(_driver, _output);

            InitialNavigation();
            selectPO.SelectRepairs(new List<string> { "Screen repair" });
            selectPO.ProceedToContract();

            createPO.SetName("Carlos");
            createPO.SetModelByIndex(0, "iPhone 14");
            // Surname, Address, Payment left default/empty
            
            createPO.SubmitContract();

            // Check Errors (Using Contains loosely as validation text varies)
            // Wait for post-back or validation execution
            System.Threading.Thread.Sleep(500);
            Assert.Contains("Surname field is required", _driver.PageSource); 
            Assert.Contains("Address", _driver.PageSource); 
            
            // System stays on form (Title still visible)
            Assert.Contains("Contract Repair", _driver.PageSource); 
        }






        [Fact]
        public void sprint3()
        {
            var selectPO = new SelectRepairsForReceipt_PO(_driver, _output);
            var createPO = new CreateRepair_PO(_driver, _output);
            var detailPO = new DetailReceipt_PO(_driver, _output);

            InitialNavigation();

            // 1. Select Repairs
            selectPO.SelectRepairs(new List<string> { "Screen repair" });
            selectPO.FilterRepairs(null, "Low");

            
            // Select
            selectPO.SelectRepairs(new List<string> { "Battery repair" });
            Thread.Sleep(500);
            selectPO.RemoveRepairFromCart("Screen repair");


            selectPO.ProceedToContract();


            // 2. Fill Data
            createPO.SetName("Carlos");
            createPO.SetSurname("García");
            createPO.SetAddress("Calle Luna 7");
            createPO.SetPaymentMethod("CreditCard");

           
            createPO.SetModelByIndex(0, "Samsung Galaxy S23");
            

            createPO.SubmitContract();

            // 3. Validate Receipt
            // Expected: Carlos García, Calle Luna 7, Total 130€
            // Razor DetailPrint: @item.Cost € (With space)
            var expectedItems = new List<string[]> {
                new string[] { "Battery repair", "Low", "Samsung Galaxy S23", "80 €" }
            };

            Assert.True(detailPO.CheckReceiptItems(expectedItems));
            Assert.Contains("80", _driver.PageSource); // Total Check (Flexible for 130 or 130,00)
            Assert.Contains("Carlos", _driver.PageSource);
            Assert.Contains("Luna 7", _driver.PageSource);
        }






        }
}
