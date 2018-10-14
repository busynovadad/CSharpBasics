using Acme.Common;
using static Acme.Common.LoggingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Biz
{
    /// <summary>
    /// Manages products carried in inventory
    /// </summary>
    public class Product
    {
        public const double InchesPerMeter = 39.37;
        public readonly decimal MinimumPrice;
        #region Constructors
        public Product()
        {
            //this.ProductVendor = new Vendor();
            Console.WriteLine("Product instance created");
            this.MinimumPrice = .96m;
            this.category = "Tools";
        }

        public Product(int productId
            , string productName
            , string description) : this()
        {
            this.ProductId = productId;
            this.ProductName = productName;
            this.Description = description;
            if (ProductName.StartsWith("Bulk"))
            {
                this.MinimumPrice = 9.99m;
            }
            Console.WriteLine("Product instance has a name: " + ProductName);
        }
        #endregion

        #region Properties
        private DateTime? availabilityDate;

        public DateTime? AvailabilityDate
        {
            get { return availabilityDate; }
            set { availabilityDate = value; }
        }


        private string productName;

        public string ProductName
        {
            get
            {
                var formattedValue = productName?.Trim();
                return formattedValue;
            }
            set
            {
                if (value.Length < 3)
                {
                    ValidationMessage = "Product Name must be at least 3 characters";
                }
                else if (value.Length > 20)
                {
                    ValidationMessage = "Product Name cannot be more than 20 characters";
                }
                else { 
                productName = value;
            }
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private int productId;

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        private Vendor productVendor;

        public Vendor ProductVendor
        {
            get
            {
                if (productVendor == null)
                {
                    productVendor = new Vendor();
                }
                return productVendor;
            }
            set { productVendor = value; }
        }

        internal string category {  get; set; }
        public int SequenceNumber { get; set; } = 1;
        public string ProductCode => this.category + "-" + this.SequenceNumber;

        public string ValidationMessage { get; private set; }
        public decimal Cost { get; set; }
        #endregion

        public string SayHello()
        {
            var vendor = new Vendor();
            vendor.SendWelcomeEmail("Message from SayHello");

            var emailService = new EmailService();
            var confirmation = emailService.SendMessage("New Product", this.productName, "sales@abc.com");

            var result = LogAction("saying Hello");
            //this.MinimumPrice = 123m;

            return "Hello " + ProductName +
                " (" + ProductId + "): " +
                Description +
                " Available on: " +
                AvailabilityDate?.ToShortDateString();
        }

        public override string ToString()
        {
            return this.ProductName + "(" + this.productId + ")";
        }

        /// <summary>
        /// Calculates the suggested retail price
        /// </summary>
        /// <param name="markupPercent">Percent used to mark up the cost.</param>
        /// <returns></returns>
        public decimal CalculateSuggestedPrice(decimal markupPercent) =>
            this.Cost + (this.Cost * markupPercent / 100);
    }
}
