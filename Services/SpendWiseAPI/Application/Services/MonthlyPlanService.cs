using Application.Interfaces;
using Domain;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Application.Services
{
    public class MonthlyPlanService
    {
        private IMonthlyPlanRepository _monthlyPlanRepository;
        private IBudgetPlanRepository _budgetPlanRepository;
        private ITransactionsRepository _transactionsRepository;

        public MonthlyPlanService(IMonthlyPlanRepository monthlyPlanRepository, IBudgetPlanRepository budgetPlanRepository, ITransactionsRepository transactionsRepository)
        {
            _monthlyPlanRepository = monthlyPlanRepository;
            _budgetPlanRepository = budgetPlanRepository;
            _transactionsRepository = transactionsRepository;
        }
        public async Task<bool> AddMonthlyPlans(MonthlyPlan monthlyPlan)
        {
            var result = _monthlyPlanRepository.VerifyUserHasPlanActive(monthlyPlan.user_id);
            if(result == true )
            {
                throw new Exception("User already have a current plan active");
            }
            return await _monthlyPlanRepository.AddMonthlyPlans(monthlyPlan);
        }
        public async Task<bool> CancelMonthlyPlan(Guid id)
        {
            return await _monthlyPlanRepository.CancelMonthlyPlan(id);
        }
        public List<MonthlyPlanGetNameDate> GetHistoryPlans(Guid user_id)
        {
            return _monthlyPlanRepository.GetHistoryPlans(user_id);
        }
        public List<MonthlyPlanGet> GetMonthlyPlanFromHistory(Guid monthlyPlan_id)
        {
            return _monthlyPlanRepository.GetMonthlyPlanFromHistory(monthlyPlan_id);
        }
        public List<MonthlyPlanGet> GetCurrentPlan(Guid user_id) 
        {
            var date = _monthlyPlanRepository.GetDateFromMonthlyPlanByUserID(user_id);
            if (date.Count == 0)
            {
                return new List<MonthlyPlanGet>();
            }
            if (date.Count != 1)
            {
                throw new Exception("errors, many plans in progress");
            }
            DateTime currentDate = DateTime.Now;
            TimeSpan difference = currentDate - date[0].date;
            if(Math.Abs(difference.Days) >=30 )
            {
                var result = _monthlyPlanRepository.FinishedMonthlyPlan(date[0].monthlyPlan_id);
                if (result == false)
                {
                    throw new Exception("errors, problem when finished a monthly plan");
                }
                return new List<MonthlyPlanGet>();
            }
            return _monthlyPlanRepository.GetCurrentPlan(user_id);
        }

        public MonthlyPlanDemo GetDemoMonthlyPlan(Guid plan_id)
        {
            var result = _monthlyPlanRepository.GetDemoMonthlyPlan(plan_id);
            if (result.Count==0)
            {
                throw new Exception("Errorrs, insert demo for plan_id not exist");
            }
            return result.FirstOrDefault();
        }


        public async Task<byte[]> ExportCurrentDetailsToPdf(Guid id, int year, int month)
        {
            MonthlyPlanGet currentMonthlyPlan = this._monthlyPlanRepository.GetMonthlyPlanFromHistoryByMonthAndYear(id, year, month);
            var plan = await this._budgetPlanRepository.GetPlanById(currentMonthlyPlan.plan_id);

            if (plan == null)
            {
                throw new Exception("Plan not found");
            }

            var imagepath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).ToString(), "Infrastructure", "Images", "logo.png");
            using (var memoryStream = new MemoryStream())
            {
                var writer = new PdfWriter(memoryStream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Spacer
                var spacer = new Paragraph().SetHeight(10);

                // Load the logo image from the local file system
                if (!File.Exists(imagepath))
                {
                    throw new FileNotFoundException("Logo file not found", imagepath);
                }

                byte[] logoBytes = await File.ReadAllBytesAsync(imagepath);
                ImageData imageData = ImageDataFactory.Create(logoBytes);
                Image logo = new Image(imageData);
                logo.Scale(4, 4);


                document.Add(logo);
                document.Add(spacer);


                var budgetPlanTitle = new Paragraph()
                 .Add(new Text("Budget Plan Name : ").SetBold().SetFontSize(16))
                 .Add(new Text(plan.name).SetFontSize(16));
                document.Add(budgetPlanTitle);


                var detailsParagraph = new Paragraph()
                    .Add(new Text("Description : ").SetBold().SetFontSize(16))
                    .Add(new Text(plan.description).SetFontSize(16));
                document.Add(detailsParagraph);

                var overallBudgetedAmount = new Paragraph()
                    .Add(new Text("Overall Budgeted Amount: ").SetBold().SetFontSize(16))
                    .Add(new Text(currentMonthlyPlan.totalAmount.ToString() + "$").SetFontSize(16));
                document.Add(overallBudgetedAmount);

                document.Add(new Paragraph().SetHeight(5));


                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2, 2, 2 })).UseAllAvailableWidth();





                var categories = plan.category.Split(',');
                var categoryBudgetedAmounts = currentMonthlyPlan.priceByCategory.Split(',');
                var categorySpentAmounts = currentMonthlyPlan.spentOfCategory.Split(',');

                for (int i = 0; i < categories.Length; i++)
                {
                    var trimmedCategory = categories[i].Trim();
                    var budgetedAmount = categoryBudgetedAmounts[i].Trim();
                    var spentAmount = categorySpentAmounts[i].Trim();


                    Cell categoryCell = new Cell(1, 5).Add(new Paragraph(trimmedCategory).SetFontSize(13).SetBold()).SetBackgroundColor(new DeviceRgb(200, 255, 142));
                    table.AddCell(categoryCell);



                    var transactions = this._transactionsRepository.GetTransactionsForCategoryAndMonthlyPlan(trimmedCategory, currentMonthlyPlan.monthlyPlan_id);
                    int transactionNumber = 1;

                    foreach (var transaction in transactions)
                    {

                        string formattedDate = transaction.date.ToString("yyyy-MM-dd");

                        table.AddCell(new Cell().Add(new Paragraph(transactionNumber.ToString()).SetTextAlignment(TextAlignment.CENTER)));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.name).SetTextAlignment(TextAlignment.CENTER)));

                        table.AddCell(new Cell().Add(new Paragraph(formattedDate)).SetTextAlignment(TextAlignment.CENTER));
                        table.AddCell(new Cell().Add(new Paragraph(transaction.amount.ToString() + " $").SetTextAlignment(TextAlignment.CENTER)));

                        transactionNumber++;
                    }

                    table.AddCell(new Cell(1, 5).Add(new Paragraph($"Budgeted: {budgetedAmount} $                                                                   " +
                        $"                                   Spent: {spentAmount} $")));
                }


                document.Add(table);


                var totalSpent = currentMonthlyPlan.amountSpent;
                var remainingBudget = currentMonthlyPlan.totalAmount - totalSpent;

                var summarySection = new Paragraph()
                    .Add(new Text("Summary:").SetBold().SetFontSize(16))
                    .Add(new Text($"\nTotal Spent: {totalSpent} $").SetFontSize(16))
                    .Add(new Text($"\nRemaining Budget: {remainingBudget} $").SetFontSize(16));
                document.Add(summarySection);

                // Close document
                document.Close();

                return memoryStream.ToArray();
            }
        }
    }
}
