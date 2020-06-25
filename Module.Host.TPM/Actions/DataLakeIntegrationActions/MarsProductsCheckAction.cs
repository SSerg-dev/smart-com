using Core.Dependency;
using Core.Settings;
using Interfaces.Implementation.Action;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Looper.Parameters;
using System.IO;
using System.Collections.Concurrent;
using Persist.Model;
using Persist.Model.Settings;
using Looper.Core;
using Castle.Core.Internal;
using System.Web.Http.OData;
using Module.Frontend.TPM.Controllers;
using System.Threading.Tasks;
using Module.Frontend.TPM.Util;

namespace Module.Host.TPM.Actions.DataLakeIntegrationActions
{
    /// <summary>
    /// Класс для проверки наличия новых или обновленных продуктов из DataLake и создания инцидента для отправки нотификации
    /// </summary>
    public class MarsProductsCheckAction : BaseAction
    {
        private static Encoding defaultEncoding = Encoding.GetEncoding("UTF-8");
        private string handlerId;
        private Guid? roleId;
        private Guid? userId;

        public MarsProductsCheckAction(string id, Guid? userId, Guid? roleId)
        {
            this.handlerId = id;
            this.roleId = roleId;
            this.userId = userId;
        }

        public override void Execute()
        {
            try
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
                    var lastSuccessDate = settingsManager.GetSetting<string>("MATERIALS_LAST_SUCCESSFUL_EXECUTION_DATE", "2020-03-12 10:00:00.000");
                    var exceptedZREPs = settingsManager.GetSetting<string>("APP_MIX_EXCEPTED_ZREPS", null);
                    var successMessages = new ConcurrentBag<string>();
                    var errorMeassages = new ConcurrentBag<string>();
                    var warningMessages = new ConcurrentBag<string>();
                    int sourceRecordCount;

                    var notifyErrors = new Dictionary<string, string>(); // ZREP, Error message

                    DateTimeOffset today = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow).Value;

                    var newRecords = context.Database.SqlQuery<MARS_UNIVERSAL_PETCARE_MATERIALS>($@"
						SELECT  materials.[VKORG],
								materials.[MATNR], 
						        materials.[VMSTD], 
						        materials.[0CREATEDON] AS CREATEDON,
						        materials.[0DIVISION] AS DIVISION,
						        materials.[0DIVISION___T] AS DIVISION___T,
						        materials.[MATERIAL],
						        materials.[SKU],
						        materials.[0MATL_TYPE___T] AS MATL_TYPE___T,
						        materials.[Brand],
						        materials.[Segmen],
						        materials.[Technology],
						        materials.[BrandTech],
						        materials.[BrandTech_code],
						        materials.[Brand_code],
						        materials.[Tech_code],
						        materials.[ZREP],
						        materials.[EAN_Case],
						        materials.[EAN_PC],
						        materials.[Brand_Flag_abbr],
						        materials.[Brand_Flag],
						        materials.[Submark_Flag],
						        materials.[Ingredient_variety],
						        materials.[Product_Category],
						        materials.[Product_Type],
						        materials.[Supply_Segment],
						        materials.[Functional_variety],
						        materials.[Size],
						        materials.[Brand_essence],
						        materials.[Pack_Type],
						        materials.[Traded_unit_format],
						        materials.[Consumer_pack_format],
						        materials.[UOM_PC2Case],
						        materials.[Segmen_code],
						        materials.[BrandsegTech_code],
						        materials.[Brandsegtech],
						        materials.[BrandSegTechSub_code],
						        materials.[BrandSegTechSub],
								materials.[SubBrand_code],
						        materials.[SubBrand]
		
						FROM MARS_UNIVERSAL_PETCARE_MATERIALS materials
	
                        WHERE materials.ZREP IS NOT NULL AND (DATEDIFF(MINUTE, '{lastSuccessDate}', materials.VMSTD) >= 0)
					").AsEnumerable();

                    sourceRecordCount = newRecords.Count();
                    if (sourceRecordCount == 0)
                    {
                        Warnings.Add("No new materials for current date");
                    }
                    else
                    {
                        Results.Add(String.Format("Founded {0} new materials for checking.", sourceRecordCount), null);
                    }

                    // Проверка масок VKORG, 0DIVISION, 0DIVISION___T, 0MATL_TYPE___T, MATNR, VMSTD, 0CREATEDON, ZREP, EAN_PC, UOM_PC2Case
                    // и проверка на наличие исключенных ZREPов
                    var step1Records = newRecords.Where(r =>
                                    IsExactNumeric(r.VKORG, 261) &&
                                    IsExactNumeric(r.DIVISION, 5) &&
                                    r.DIVISION___T == "Petcare" &&
                                    r.MATL_TYPE___T == "Finished products" &&
                                    IsLetterOrDigit(r.MATNR) &&
                                    IsValidDate(r.VMSTD.ToString("yyyy'-'MM'-'dd")) &&
                                    IsValidDate(r.CREATEDON) &&
                                    //IsValidDatePeriod(r.StartDate.ToString("yyyy'-'MM'-'dd"), r.EndDate.ToString("yyyy'-'MM'-'dd")) &&
                                    IsNumeric(r.ZREP.TrimStart('0'), 6) &&
                                    r.EAN_PC.HasValue && DecimalToString(r.EAN_PC).Length == 13 &&
                                    IsNumeric(r.UOM_PC2Case));

                    var step1Log = newRecords.Where(r =>
                                    !(IsExactNumeric(r.VKORG, 261) &&
                                    IsExactNumeric(r.DIVISION, 5) &&
                                    r.DIVISION___T == "Petcare" &&
                                    r.MATL_TYPE___T == "Finished products" &&
                                    IsLetterOrDigit(r.MATNR) &&
                                    IsValidDate(r.VMSTD.ToString("yyyy'-'MM'-'dd")) &&
                                    IsValidDate(r.CREATEDON) &&
                                    //IsValidDatePeriod(r.StartDate.ToString("yyyy'-'MM'-'dd"), r.EndDate.ToString("yyyy'-'MM'-'dd")) &&
                                    IsNumeric(r.ZREP.TrimStart('0'), 6) &&
                                    r.EAN_PC.HasValue && DecimalToString(r.EAN_PC).Length == 13 &&
                                    IsNumeric(r.UOM_PC2Case))).GroupBy(x => x.ZREP);
                    foreach (var group in step1Log)
                    {
                        warningMessages.Add(String.Format("{0} GRD with ZREP {1} has inappropriate value for one or more of VKORG, 0DIVISION, 0DIVISION___T, 0MATL_TYPE___T, MATNR, VMSTD, 0CREATEDON, ZREP, EAN_PC, UOM_PC2Case fields.", group.Count(), group.Key.TrimStart('0')));
                        notifyErrors[group.Key.TrimStart('0')] = "ZREP has inappropriate value for one or more of VKORG, 0DIVISION, 0DIVISION___T, 0MATL_TYPE___T, MATNR, VMSTD, 0CREATEDON, ZREP, EAN_PC, UOM_PC2Case fields.";
                    }

                    // Проверка заполненности полей MATERIAL, SKU, UOM_PC2Case, Segmen_code, Tech_code, Brand_Flag_abbr, Brand_Flag, Size, BrandsegTech_code, BrandSegTechSub_code, Brand_code 
                    var step2Records = step1Records.Where(r =>
                                    IsNotEmptyOrNotApplicable(r.MATERIAL) &&
                                    IsNotEmptyOrNotApplicable(r.SKU) &&
                                    IsNotEmptyOrNotApplicable(r.UOM_PC2Case) &&
                                    IsNotEmptyOrNotApplicable(r.Segmen_code) &&
                                    IsNotEmptyOrNotApplicable(r.Tech_code) &&
                                    IsNotEmptyOrNotApplicable(r.Brand_Flag_abbr) &&
                                    IsNotEmptyOrNotApplicable(r.Brand_Flag) &&
                                    IsNotEmptyOrNotApplicable(r.Size) &&
                                    IsNotEmptyOrNotApplicable(r.BrandsegTech_code) &&
                                    IsNotEmptyOrNotApplicable(r.BrandSegTechSub_code) &&
                                    IsNotEmptyOrNotApplicable(r.Brand_code));

                    var step2Log = step1Records.Where(r =>
                                    !(IsNotEmptyOrNotApplicable(r.MATERIAL) &&
                                    IsNotEmptyOrNotApplicable(r.SKU) &&
                                    IsNotEmptyOrNotApplicable(r.UOM_PC2Case) &&
                                    IsNotEmptyOrNotApplicable(r.Segmen_code) &&
                                    IsNotEmptyOrNotApplicable(r.Tech_code) &&
                                    IsNotEmptyOrNotApplicable(r.Brand_Flag_abbr) &&
                                    IsNotEmptyOrNotApplicable(r.Brand_Flag) &&
                                    IsNotEmptyOrNotApplicable(r.Size) &&
                                    IsNotEmptyOrNotApplicable(r.BrandsegTech_code) &&
                                    IsNotEmptyOrNotApplicable(r.BrandSegTechSub_code) &&
                                    IsNotEmptyOrNotApplicable(r.Brand_code))).GroupBy(x => x.ZREP);
                    foreach (var group in step2Log)
                    {
                        warningMessages.Add(String.Format("{0} GRD with ZREP {1} has one of MATERIAL, SKU, UOM_PC2Case, Segmen_code, Tech_code, Brand_Flag_abbr, Brand_Flag, Size, BrandsegTech_code, BrandSegTechSub_code, Brand_code fields not applicable or empty.", group.Count(), group.Key.TrimStart('0')));
                        notifyErrors[group.Key.TrimStart('0')] = "ZREP has one of MATERIAL, SKU, UOM_PC2Case, Segmen_code, Tech_code, Brand_Flag_abbr, Brand_Flag, Size, BrandsegTech_code, BrandSegTechSub_code, Brand_code fields not applicable or empty.";
                    }

                    // Проверка на заполненность хотя бы одного поля из Submark_Flag, Ingredient_variety, Product_Category, Product_Type, Supply_Segment, Functional_variety, Size, Brand_essence, Pack_Type, Traded_unit_format, Consumer_pack_format 
                    var step3Records = step2Records.Where(r =>
                                    IsNotEmptyOrNotApplicable(r.Submark_Flag) ||
                                    IsNotEmptyOrNotApplicable(r.Ingredient_variety) ||
                                    IsNotEmptyOrNotApplicable(r.Product_Category) ||
                                    IsNotEmptyOrNotApplicable(r.Product_Type) ||
                                    IsNotEmptyOrNotApplicable(r.Supply_Segment) ||
                                    IsNotEmptyOrNotApplicable(r.Functional_variety) ||
                                    IsNotEmptyOrNotApplicable(r.Size) ||
                                    IsNotEmptyOrNotApplicable(r.Pack_Type) ||
                                    IsNotEmptyOrNotApplicable(r.Traded_unit_format) ||
                                    IsNotEmptyOrNotApplicable(r.Consumer_pack_format));

                    var step3Log = step2Records.Where(r =>
                                    !(IsNotEmptyOrNotApplicable(r.Submark_Flag) ||
                                    IsNotEmptyOrNotApplicable(r.Ingredient_variety) ||
                                    IsNotEmptyOrNotApplicable(r.Product_Category) ||
                                    IsNotEmptyOrNotApplicable(r.Product_Type) ||
                                    IsNotEmptyOrNotApplicable(r.Supply_Segment) ||
                                    IsNotEmptyOrNotApplicable(r.Functional_variety) ||
                                    IsNotEmptyOrNotApplicable(r.Size) ||
                                    IsNotEmptyOrNotApplicable(r.Pack_Type) ||
                                    IsNotEmptyOrNotApplicable(r.Traded_unit_format) ||
                                    IsNotEmptyOrNotApplicable(r.Consumer_pack_format))).GroupBy(x => x.ZREP);
                    foreach (var group in step3Log)
                    {
                        warningMessages.Add(String.Format("{0} GRD with ZREP {0} has all of Submark_Flag, Ingredient_variety, Product_Category, Product_Type, Supply_Segment, Functional_variety, Size, Brand_essence, Pack_Type, Traded_unit_format, Consumer_pack_format fields not applicable or empty.", group.Count(), group.Key.TrimStart('0')));
                        notifyErrors[group.Key.TrimStart('0')] = "ZREP has all of Submark_Flag, Ingredient_variety, Product_Category, Product_Type, Supply_Segment, Functional_variety, Size, Brand_essence, Pack_Type, Traded_unit_format, Consumer_pack_format fields not applicable or empty.";
                    }

                    // Убираем одинаковые записи для одного ZREP (по полям Segmen_code, VMSTD, Tech_code, Brand_Flag_abbr, Brand_Flag, Size, BrandsegTech_code, Brand_code, BrandSegTechSub_code, SubBrand_code)
                    //DistinctBy VMSTD, т.к. без этого берется случайная запись с любой датой последнего обновления
                    var groupedRecords = step3Records.DistinctBy(y => new { y.ZREP, y.VMSTD, y.Segmen_code, y.Tech_code, y.Brand_Flag_abbr, y.Brand_Flag, y.Size, y.BrandsegTech_code, y.Brand_code, y.BrandSegTechSub_code, y.SubBrand_code }).GroupBy(x => x.ZREP);

                    // Если остаётся более 2 подходящих записей по одному ZREP, то берем с последней датой
                    var materialsToCheck = new List<MARS_UNIVERSAL_PETCARE_MATERIALS>();
                    foreach (var group in groupedRecords)
                    {
                        if (group.Count() == 1)
                        {
                            materialsToCheck.Add(group.First());
                        }
                        else if (group.Count() > 1)
                        {
                            var orderedByDescDateRecords = group.OrderByDescending(x => x.VMSTD);
                            var firstRec = orderedByDescDateRecords.ElementAt(0);
                            var secondRec = orderedByDescDateRecords.ElementAt(1);

                            if (firstRec.VMSTD == secondRec.VMSTD)
                            {
                                errorMeassages.Add(String.Format("ZREP {0} has two or more GRD with different values in one or more of Segmen_code, Tech_code, Brand_Flag_abbr, Brand_Flag, Size, BrandsegTech_code, Brand_code fields.", group.Key.TrimStart('0')));
                                notifyErrors[group.Key.TrimStart('0')] = "ZREP has two or more GRD with different values in one or more of Segmen_code, Tech_code, Brand_Flag_abbr, Brand_Flag, Size, BrandsegTech_code, Brand_code fields.";
                            }
                            else
                            {
                                materialsToCheck.Add(orderedByDescDateRecords.First());
                            }
                        }
                    }

                    var materialsToWarning = materialsToCheck.DistinctBy(y => new { y.ZREP, y.Submark_Flag, y.Ingredient_variety, y.Product_Category, y.Product_Type, y.Supply_Segment, y.Functional_variety, y.Size, y.Brand_essence, y.Pack_Type, y.Traded_unit_format, y.Consumer_pack_format }).GroupBy(x => x.ZREP);
                    foreach (var group in materialsToWarning)
                    {
                        if (group.Count() > 1)
                        {
                            warningMessages.Add(String.Format("ZREP {0} has two or more GRD with different values in one or more of Submark_Flag, Ingredient_variety, Product_Category, Product_Type, Supply_Segment, Functional_variety, Size, Brand_essence, Pack_Type, Traded_unit_format, Consumer_pack_format fields.", group.Key.TrimStart('0')));
                            notifyErrors[group.Key.TrimStart('0')] = "ZREP has two or more GRD with different values in one or more of Submark_Flag, Ingredient_variety, Product_Category, Product_Type, Supply_Segment, Functional_variety, Size, Brand_essence, Pack_Type, Traded_unit_format, Consumer_pack_format fields.";
                        }
                    }

                    if (!materialsToCheck.Any() && sourceRecordCount > 0)
                    {
                        Warnings.Add(String.Format("No materials found suitable for updating or creating products.", sourceRecordCount));
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(exceptedZREPs))
                        {
                            var appMixMaterials = new List<string>();
                            foreach (var material in materialsToCheck)
                            {
                                if (exceptedZREPs.Split(',').Contains(material.ZREP.TrimStart('0')))
                                {
                                    material.Brandsegtech.Replace("Pouch", "Pouch App.Mix");
                                    if (material.Brandsegtech.Contains("App.Mix"))
                                    {
                                        appMixMaterials.Add(material.ZREP.TrimStart('0'));
                                    }
                                }
                            }
                            if (appMixMaterials.Any())
                            {
                                Results.Add(String.Format("Brandsegtech changed for ZREPs {0} from App.Mix ZREP list (setting 'APP_MIX_EXCEPTED_ZREPs')",
                                    String.Join(",", appMixMaterials)), null);
                            }
                        }
                    }

                    // Проверяем есть ли новые ZREP
                    var products = context.Set<Product>().Where(p => !p.Disabled).ToList();
                    var newProductsMaterial = materialsToCheck.Where(x => !products.Any(p => p.ZREP == x.ZREP.TrimStart('0')));
                    var createdZREPs = new List<string>();
                    bool isExecutionSuccess = false;

                    foreach (var material in newProductsMaterial)
                    {
                        //Если такой для tech_code существует пермиальность, тогда проставляется SubCode по-умолчанию при значениях null, или остается существующий
                        //if (context.Set<Technology>().Any(t => t.Tech_code.Equals(material.Tech_code) && t.SubBrand_code != null))
                        //{
                        //    material.SubBrand_code = material.SubBrand_code.IsNullOrEmpty() ? "01" : material.SubBrand_code;
                        //    material.SubBrand = material.SubBrand.IsNullOrEmpty() ? "Core" : material.SubBrand;
                        //}

                        var errors = UpdateTech(material.Tech_code, material.Technology, material.SubBrand_code, material.SubBrand);
                        errors.AddRange(CheckNewBrandTech(material.Brand_code, material.Brand, material.Segmen_code, material.Tech_code, material.Technology, material.SubBrand_code, material.SubBrand));

                        foreach (var error in errors)
                            errorMeassages.Add(error);

                        if (!errors.Any())
                        {
                            Product newProduct = CreateProduct(material);
                            context.Set<Product>().Add(newProduct);
                            context.Set<ProductChangeIncident>().Add(CreateIncident(newProduct, true, false));

                            try
                            {
                                context.SaveChanges();
                                createdZREPs.Add(newProduct.ZREP);
                                successMessages.Add(String.Format("Product with ZREP {0} was created.", newProduct.ZREP));
                                isExecutionSuccess = true;
                            }
                            catch (Exception e)
                            {
                                errorMeassages.Add(String.Format("Error while creating product with ZREP {0}. Message: {1}", newProduct.ZREP, e.Message));
                                notifyErrors[material.ZREP.TrimStart('0')] = String.Format("Error while creating ZREP. Message: {0}.", e.Message);
                            }
                        }
                        else
                        {
                            errorMeassages.Add(String.Format("Product with ZREP {0} was not added because an error occurred while checking brand technology.", material.ZREP.TrimStart('0')));
                            notifyErrors[material.ZREP.TrimStart('0')] = "ZREP was not added because an error occurred while checking brand technology.";
                        }
                    }
                    if (createdZREPs.Any())
                    {
                        Results.Add(String.Format("Added notification incidents for created products with ZREPs: {0}", String.Join(",", createdZREPs.Select(x => x.TrimStart('0')))), null);
                    }

                    // Проверяем, есть ли продукты, которые необходимо обновить
                    var updateProductsMaterial = materialsToCheck.Except(newProductsMaterial);
                    var updatedZREPs = new List<string>();

                    foreach (var material in updateProductsMaterial)
                    {
                        material.ZREP = material.ZREP.TrimStart('0');
                        Product productToUpdate = products.Where(p => p.ZREP == material.ZREP).FirstOrDefault();
                        List<string> errors = new List<string>();
                        if (productToUpdate != null)
                        {
                            if (IsChange(material, productToUpdate))
                            {

                                errors = UpdateTech(material.Tech_code, material.Technology, material.SubBrand_code, material.SubBrand);
                                errors.AddRange(CheckNewBrandTech(material.Brand_code, material.Brand, material.Segmen_code, material.Tech_code, material.Technology, material.SubBrand_code, material.SubBrand));

                                foreach (var error in errors)
                                    errorMeassages.Add(error);

                                if (!errors.Any())
                                {
                                    Product product = context.Set<Product>().Find(productToUpdate.Id);
                                    List<string> updatedFileds = ApplyChanges(material, product, context);

                                    context.Set<ProductChangeIncident>().Add(CreateIncident(product, false, false));

                                    try
                                    {
                                        context.SaveChanges();
                                        updatedZREPs.Add(product.ZREP);
                                        successMessages.Add(String.Format("Product with ZREP {0} was updated. Fields changed: {1}.", product.ZREP, String.Join(",", updatedFileds)));
                                        isExecutionSuccess = true;
                                    }
                                    catch (Exception e)
                                    {
                                        errorMeassages.Add(String.Format("Error while updating product with ZREP {0}. Message: {1}", product.ZREP, e.Message));
                                        notifyErrors[material.ZREP.TrimStart('0')] = String.Format("Error while updating ZREP. Message: {0}.", e.Message);
                                    }
                                }
                                else
                                {
                                    errorMeassages.Add(String.Format("Product with ZREP {0} was not updated because an error occurred while checking brand technology.", material.ZREP.TrimStart('0')));
                                    notifyErrors[material.ZREP.TrimStart('0')] = "ZREP was not updated because an error occurred while checking brand technology.";
                                }
                            }

                        }
                    }
                    if (updatedZREPs.Any())
                    {
                        Results.Add(String.Format("Added notification incident for updated products with ZREPs: {0}", String.Join(",", updatedZREPs)), null);
                    }

                    if (isExecutionSuccess)
                    {
                        Setting lastSuccessDateSetting = context.Settings.Where(x => x.Name == "MATERIALS_LAST_SUCCESSFUL_EXECUTION_DATE").FirstOrDefault();
                        if (lastSuccessDateSetting != null)
                        {
                            lastSuccessDateSetting.Value = today.ToString("yyyy'-'MM'-'dd HH':'mm':'ss.fff");
                            context.SaveChanges();
                        }
                    }

                    CreateNotificationHandler(notifyErrors, new List<string>(createdZREPs).Union(updatedZREPs).ToList(), context);

                    if (newProductsMaterial.Any() || updateProductsMaterial.Any())
                    {
                        var errors = CheckBrandDoubles(context);

                        foreach (var error in errors)
                            errorMeassages.Add(error);
                    }

                    DataLakeSyncResultFilesModel model = SaveResultToFile(handlerId, successMessages, errorMeassages, warningMessages);

                    // Сохранить выходные параметры
                    Results["DataLakeSyncSourceRecordCount"] = sourceRecordCount;
                    Results["DataLakeSyncResultRecordCount"] = successMessages.Count();
                    Results["ErrorCount"] = errorMeassages.Count();
                    Results["WarningCount"] = warningMessages.Count();
                    Results["DataLakeSyncResultFilesModel"] = model;
                }
            }
            catch (Exception e)
            {
                string msg = String.Format("An error occurred while cheking Mars products", e.ToString());
                Errors.Add(msg);
            }
        }

        private void CreateNotificationHandler(Dictionary<string, string> notifyErrors, List<string> successZREPs, DatabaseContext context)
        {
            notifyErrors = notifyErrors.Where(kvp => !successZREPs.Contains(kvp.Key) && !string.IsNullOrEmpty(kvp.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (!notifyErrors.Any()) return;

            var handlerData = new HandlerData();
            HandlerDataHelper.SaveIncomingArgument("ErrorsToNotify", notifyErrors, handlerData, throwIfNotExists: false);
            var handler = new LoopHandler()
            {
                Id = Guid.NewGuid(),
                ConfigurationName = "PROCESSING",
                Description = "Sending notifications with ZREPs that failed to sync with Products.",
                Name = "Module.Host.TPM.Handlers.Notifications.ProductSyncFailNotificationHandler",
                ExecutionPeriod = null,
                CreateDate = ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                LastExecutionDate = null,
                NextExecutionDate = null,
                ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                UserId = userId,
                RoleId = roleId
            };

            handler.SetParameterData(handlerData);
            context.LoopHandlers.Add(handler);

            context.SaveChanges();
        }

        private List<string> CheckBrandDoubles(DatabaseContext context)
        {
            var errors = new List<string>();
            var brandsByCode = context.Set<Brand>().Where(x => !x.Disabled).GroupBy(x => x.Brand_code).ToList();

            foreach (var brandGroup in brandsByCode)
            {
                if (brandGroup.DistinctBy(x => new { x.Brand_code, x.Segmen_code }).Count() == 2)
                {
                    string updateTemplate = "UPDATE Brand SET Name = '{0}' WHERE Id = '{1}'";

                    if (brandGroup.DistinctBy(x => x.Name).Count() != 1)
                    {
                        continue;
                    }

                    foreach (var brand in brandGroup)
                    {
                        try
                        {
                            if (brand.Segmen_code == "04")
                            {
                                string catName = String.Format("{0} Cat", brand.Name);
                                string catBrandUpdate = String.Format(updateTemplate, catName, brand.Id);
                                context.Database.ExecuteSqlCommand(catBrandUpdate);

                                Results.Add(String.Format("Added segment to Brand name for brand with code {0} (segment code: {1})", brand.Brand_code, brand.Segmen_code), null);
                            }
                            else if (brand.Segmen_code == "07")
                            {
                                string dogName = String.Format("{0} Dog", brand.Name);
                                string dogBrandUpdate = String.Format(updateTemplate, dogName, brand.Id);
                                context.Database.ExecuteSqlCommand(dogBrandUpdate);

                                Results.Add(String.Format("Added segment to Brand name for brand with code {0} (segment code: {1})", brand.Brand_code, brand.Segmen_code), null);
                            }
                        }
                        catch (Exception e)
                        {
                            errors.Add(String.Format("Error while checking brand doublecates by brand_code {0}. Message: {1}", brand.Brand_code, e.Message));
                        }

                    }
                }
            }

            return errors;
        }

        private List<string> CheckNewBrandTech(string brandCode, string brandName, string segmenCode, string techCode, string techName, string subCode, string subName)
        {
            var errors = new List<string>();
            using (DatabaseContext context = new DatabaseContext())
            {
                if (!String.IsNullOrEmpty(brandCode) && !String.IsNullOrEmpty(segmenCode) && !String.IsNullOrEmpty(techCode))
                {
                    Brand checkBrand = context.Set<Brand>().FirstOrDefault(n => n.Brand_code == brandCode && n.Segmen_code == segmenCode && !n.Disabled);
                    Technology checkTech = context.Set<Technology>().FirstOrDefault(n => n.Tech_code == techCode && n.SubBrand_code == subCode && !n.Disabled);

                    if (checkBrand == null || checkTech == null)
                    {
                        if (checkBrand == null)
                        {
                            checkBrand = new Brand { Disabled = false, Brand_code = brandCode, Name = brandName, Segmen_code = segmenCode };
                            checkBrand = context.Set<Brand>().Add(checkBrand);
                            try
                            {
                                context.SaveChanges();
                                Results.Add(String.Format("Added new Brand: {0}", checkBrand.Name), null);
                            }
                            catch (Exception e)
                            {
                                errors.Add(String.Format("Error while adding new Brand with brand code {0}. Message: {1}", brandCode, e.Message));
                            }
                        }

                        if (checkTech == null)
                        {
                            checkTech = new Technology { Disabled = false, Tech_code = techCode, Name = techName, SubBrand = subName, SubBrand_code = subCode };
                            checkTech = context.Set<Technology>().Add(checkTech);
                            try
                            {
                                context.SaveChanges();
                                Results.Add(String.Format("Added new Technology: {0} {1}", checkTech.Name, checkTech.SubBrand), null);
                            }
                            catch (Exception e)
                            {
                                errors.Add(String.Format("Error while adding new Technology with technology code {0} and sub code {1}. Message: {2}", techCode, subCode, e.Message));
                            }
                        }

                        var newBrandTech = new BrandTech { Disabled = false, BrandId = checkBrand.Id, TechnologyId = checkTech.Id };
                        newBrandTech = context.Set<BrandTech>().Add(newBrandTech);
                        try
                        {
                            context.SaveChanges();
                            List<string> brandTechList = new List<string>
                            {
                                newBrandTech.BrandsegTechsub_code
                            };
                            CreateCoefficientSI2SOHandler(brandTechList, null, 1);
                            CreateActualCloneWithZeroShareByBrandTech(context, newBrandTech);

                            Results.Add(String.Format("Added new BrandTech: {0}. BrandTech code: {1}. BrandSegTechSub code: {2}", newBrandTech.Name, newBrandTech.BrandTech_code, newBrandTech.BrandsegTechsub_code), null);
                        }
                        catch (Exception e)
                        {
                            errors.Add(String.Format("Error while adding new BrandTech with brand code {0}, technology code {1} and sub code {2}. Message: {3}", brandCode, techCode, subCode, e.Message));
                        }
                    }
                }
            }

            return errors;
        }

        private List<string> UpdateTech(string techCode, string techName, string subCode, string subName)
        {
            var errors = new List<string>();
            using (DatabaseContext context = new DatabaseContext())
            {
                if (!String.IsNullOrEmpty(techName))
                {
                    var checkTech = context.Set<Technology>().FirstOrDefault(t => t.Tech_code.Equals(techCode) &&
                                                                                    t.SubBrand_code.Equals(subCode) &&
                                                                                    !t.Disabled);             

                    if (checkTech != null)
                    {
                        bool isUpdated = false;

                        if (String.IsNullOrEmpty(checkTech.SubBrand_code))
                        {
                            checkTech.SubBrand_code = subCode;
                            isUpdated = true;
                        }
                        if (checkTech.SubBrand_code == subCode && checkTech.Name != techName)
                        {
                            var oldName = checkTech.Name;
                            checkTech.Name = techName;
                            var newName = String.Format($"{checkTech.Name} {checkTech.SubBrand}");
                            Task.Run(() => PromoHelper.UpdateProductHierarchy("Technology", newName, oldName, checkTech.Id));
                            UpdateProductTrees(checkTech.Id, newName, context);
                            isUpdated = true;
                        }

                        if (isUpdated)
                        {
                            try
                            {
                                context.SaveChanges();
                                Results.Add(String.Format("Updated Technology with technology code {0} and sub code {1}", checkTech.Tech_code, checkTech.SubBrand_code), null);
                            }
                            catch (Exception e)
                            {
                                errors.Add(String.Format("Error while updating Technology with technology code {0} and sub code {1}. Message: {2}", techCode, subCode, e.Message));
                            }
                        }
                    }
                }
                else
                {
                    Warnings.Add(String.Format("Warning while updating Technology with technology code {0} and sub code {1}. TechName is undefined.", techCode, subCode));
                }
            }

            return errors;
        }

        private void UpdateProductTrees(Guid id, string newName, DatabaseContext context)
        {
            ProductTree[] productTree = context.Set<ProductTree>().Where(n => n.TechnologyId == id).ToArray();
            for (int i = 0; i < productTree.Length; i++)
            {
                // меняем также FullPathName
                string oldFullPath = productTree[i].FullPathName;
                int ind = oldFullPath.LastIndexOf(">");
                ind = ind < 0 ? 0 : ind + 2;

                productTree[i].FullPathName = oldFullPath.Substring(0, ind) + newName;
                productTree[i].Name = newName;
                ProductTreesController.UpdateFullPathProductTree(productTree[i], context.Set<ProductTree>());
            }
        }

        public static void CreateActualCloneWithZeroShareByBrandTech(DatabaseContext databaseContext, BrandTech brandTech)
        {
            var newClientTreeBrandTeches = new List<ClientTreeBrandTech>();
            var actualClientTreeBrandTeches = GetActualQuery(databaseContext);
            var clientTrees = databaseContext.Set<ClientTree>().Where(x => x.EndDate == null);

            foreach (var clientTree in clientTrees)
            {
                var parentDemandCode = actualClientTreeBrandTeches.FirstOrDefault(x => x.ClientTreeId == clientTree.Id)?.ParentClientTreeDemandCode;
                if (!String.IsNullOrEmpty(parentDemandCode))
                {
                    var newClientTreeBrandTech = new ClientTreeBrandTech
                    {
                        Id = Guid.NewGuid(),
                        ClientTreeId = clientTree.Id,
                        BrandTechId = brandTech.Id,
                        Share = 0,
                        ParentClientTreeDemandCode = parentDemandCode,
                        CurrentBrandTechName = brandTech.BrandsegTechsub
                    };

                    newClientTreeBrandTeches.Add(newClientTreeBrandTech);
                }
            }

            databaseContext.Set<ClientTreeBrandTech>().AddRange(newClientTreeBrandTeches);
            databaseContext.SaveChanges();
        }

        public static List<ClientTreeBrandTech> GetActualQuery(DatabaseContext databaseContext)
        {
            var actualClientTreeBrandTeches = new List<ClientTreeBrandTech>();

            var clientTreeBrandTeches = databaseContext.Set<ClientTreeBrandTech>().Where(x => !x.Disabled).ToList();
            var actualBrandTeches = databaseContext.Set<BrandTech>().Where(x => !x.Disabled).ToList();
            var actualClientTrees = databaseContext.Set<ClientTree>().Where(x => x.EndDate == null && !x.IsBaseClient).ToList();

            foreach (var clientTreeBrandTech in clientTreeBrandTeches)
            {
                if (clientTreeBrandTech.ClientTree.EndDate == null && clientTreeBrandTech.ClientTree.IsBaseClient)
                {
                    if (actualBrandTeches.Any(x => x.Name == clientTreeBrandTech.BrandTech.BrandsegTechsub /*&& !x.Brand.Disabled && !x.Technology.Disabled*/))
                    {
                        var currentClientTree = clientTreeBrandTech.ClientTree;
                        while (currentClientTree != null && currentClientTree.Type != "root" && String.IsNullOrEmpty(currentClientTree.DemandCode))
                        {
                            currentClientTree = actualClientTrees.FirstOrDefault(x => x.ObjectId == currentClientTree.parentId);
                        }

                        // Если родитель узла существует и его DemandCode равен DemandCode из ClientTreeBrandTech.
                        if (currentClientTree != null &&
                            !String.IsNullOrEmpty(currentClientTree.DemandCode)
                            && currentClientTree.DemandCode == clientTreeBrandTech.ParentClientTreeDemandCode)
                        {
                            actualClientTreeBrandTeches.Add(clientTreeBrandTech);
                        }
                    }
                }
            }

            return actualClientTreeBrandTeches;
        }

        private Product CreateProduct(MARS_UNIVERSAL_PETCARE_MATERIALS material)
        {
            return new Product()
            {
                BrandEssence = material.Brand_essence,
                BrandFlag = material.Brand_Flag,
                BrandFlagAbbr = material.Brand_Flag_abbr,
                Brandsegtech = material.Brandsegtech,
                SubBrand_code = material.SubBrand_code,
                Brand_code = material.Brand_code,
                ConsumerPackFormat = material.Consumer_pack_format,
                Division = !String.IsNullOrEmpty(material.DIVISION) ? (int?)int.Parse(material.DIVISION) : null,
                EAN_Case = material.EAN_Case.HasValue ? DecimalToString(material.EAN_Case) : null,
                EAN_PC = material.EAN_PC.HasValue ? DecimalToString(material.EAN_PC) : null,
                FunctionalVariety = material.Functional_variety,
                IngredientVariety = material.Ingredient_variety,
                MarketSegment = material.Segmen,
                PackType = material.Pack_Type,
                ProductCategory = material.Product_Category,
                ProductEN = material.SKU,
                ProductType = material.Product_Type,
                Segmen_code = material.Segmen_code,
                Size = material.Size,
                SubmarkFlag = material.Submark_Flag,
                SupplySegment = material.Supply_Segment,
                Tech_code = material.Tech_code,
                TradedUnitFormat = material.Traded_unit_format,
                UOM_PC2Case = !String.IsNullOrEmpty(material.UOM_PC2Case) ? (int?)int.Parse(material.UOM_PC2Case) : null,
                ZREP = material.ZREP.TrimStart('0')
            };
        }

        private bool IsChange(MARS_UNIVERSAL_PETCARE_MATERIALS material, Product product)
        {
            return (product.BrandEssence != material.Brand_essence ||
                product.BrandFlag != material.Brand_Flag ||
                product.BrandFlagAbbr != material.Brand_Flag_abbr ||
                product.Brand_code != material.Brand_code ||
                product.ConsumerPackFormat != material.Consumer_pack_format ||
                product.FunctionalVariety != material.Functional_variety ||
                product.IngredientVariety != material.Ingredient_variety ||
                product.PackType != material.Pack_Type ||
                product.ProductCategory != material.Product_Category ||
                product.ProductType != material.Product_Type ||
                product.Segmen_code != material.Segmen_code ||
                product.Size != material.Size ||
                product.SubmarkFlag != material.Submark_Flag ||
                product.SupplySegment != material.Supply_Segment ||
                product.Tech_code != material.Tech_code ||
                product.SubBrand_code != material.SubBrand_code ||
                product.TradedUnitFormat != material.Traded_unit_format);
        }

        private List<string> ApplyChanges(MARS_UNIVERSAL_PETCARE_MATERIALS material, Product product, DatabaseContext context)
        {
            var updatedFields = new List<string>();
            int? division = !String.IsNullOrEmpty(material.DIVISION) ? (int?)int.Parse(material.DIVISION) : null;
            int? UOM_PC2Case = !String.IsNullOrEmpty(material.UOM_PC2Case) ? (int?)int.Parse(material.UOM_PC2Case) : null;
            string EAN_Case = material.EAN_Case.HasValue ? DecimalToString(material.EAN_Case) : null;
            string EAN_PC = material.EAN_PC.HasValue ? DecimalToString(material.EAN_PC) : null;

            product.BrandEssence = product.BrandEssence != material.Brand_essence ? material.Brand_essence : product.BrandEssence;
            product.BrandFlag = product.BrandFlag != material.Brand_Flag ? material.Brand_Flag : product.BrandFlag;
            product.BrandFlagAbbr = product.BrandFlagAbbr != material.Brand_Flag_abbr ? material.Brand_Flag_abbr : product.BrandFlagAbbr;
            product.Brandsegtech = product.Brandsegtech != material.Brandsegtech ? material.Brandsegtech : product.Brandsegtech;
            product.BrandsegTechsub_code = product.BrandsegTechsub_code != material.BrandSegTechSub_code ? material.BrandSegTechSub_code : product.BrandsegTechsub_code;
            product.SubBrand_code = product.SubBrand_code != material.SubBrand_code ? material.SubBrand_code : product.SubBrand_code;
            product.Brand_code = product.Brand_code != material.Brand_code ? material.Brand_code : product.Brand_code;
            product.ConsumerPackFormat = product.ConsumerPackFormat != material.Consumer_pack_format ? material.Consumer_pack_format : product.ConsumerPackFormat;
            product.Division = product.Division != division ? division : product.Division;
            product.EAN_Case = product.EAN_Case != EAN_Case ? EAN_Case : product.EAN_Case;
            product.EAN_PC = product.EAN_PC != EAN_PC ? EAN_PC : product.EAN_PC;
            product.FunctionalVariety = product.FunctionalVariety != material.Functional_variety ? material.Functional_variety : product.FunctionalVariety;
            product.IngredientVariety = product.IngredientVariety != material.Ingredient_variety ? material.Ingredient_variety : product.IngredientVariety;
            product.MarketSegment = product.MarketSegment != material.Segmen ? material.Segmen : product.MarketSegment;
            product.PackType = product.PackType != material.Pack_Type ? material.Pack_Type : product.PackType;
            product.ProductCategory = product.ProductCategory != material.Product_Category ? material.Product_Category : product.ProductCategory;
            product.ProductEN = product.ProductEN != material.SKU ? material.SKU : product.ProductEN;
            product.ProductType = product.ProductType != material.Product_Type ? material.Product_Type : product.ProductType;
            product.Segmen_code = product.Segmen_code != material.Segmen_code ? material.Segmen_code : product.Segmen_code;
            product.Size = product.Size != material.Size ? material.Size : product.Size;
            product.SubmarkFlag = product.SubmarkFlag != material.Submark_Flag ? material.Submark_Flag : product.SubmarkFlag;
            product.SupplySegment = product.SupplySegment != material.Supply_Segment ? material.Supply_Segment : product.SupplySegment;
            product.Tech_code = product.Tech_code != material.Tech_code ? material.Tech_code : product.Tech_code;
            product.TradedUnitFormat = product.TradedUnitFormat != material.Traded_unit_format ? material.Traded_unit_format : product.TradedUnitFormat;
            product.UOM_PC2Case = product.UOM_PC2Case != UOM_PC2Case ? UOM_PC2Case : product.UOM_PC2Case;
            product.ZREP = product.ZREP != material.ZREP.TrimStart('0') ? material.ZREP.TrimStart('0') : product.ZREP;

            var productState = context.Entry<Product>(product);
            foreach (var value in productState.OriginalValues.PropertyNames)
            {
                var originalValue = productState.OriginalValues.GetValue<object>(value);
                var currentValue = productState.CurrentValues.GetValue<object>(value);
                if (!object.Equals(originalValue, currentValue))
                {
                    updatedFields.Add(value);
                }
            }

            return updatedFields;
        }

        private ProductChangeIncident CreateIncident(Product product, bool isCreate, bool isDelete)
        {
            return new ProductChangeIncident
            {
                CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                ProductId = product.Id,
                IsCreate = isCreate,
                IsDelete = isDelete
            };
        }

        private bool IsNumeric(string s, int length = -1)
        {
            if (String.IsNullOrEmpty(s))
                return false;

            int numResult;
            bool result = int.TryParse(s, out numResult);

            if (length == -1)
                return result;
            else if (result)
                return numResult.ToString().Length == length;
            else
                return false;
        }

        private bool IsExactNumeric(string s, int num)
        {
            if (String.IsNullOrEmpty(s))
                return false;

            bool result = false;
            int numResult;

            if (int.TryParse(s, out numResult))
                result = num == numResult ? true : false;

            return result;
        }

        private bool IsLetterOrDigit(string s, bool allowWhiteSpaces = false)
        {
            if (String.IsNullOrEmpty(s))
                return false;

            Regex regexItem;

            if (allowWhiteSpaces)
                regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            else
                regexItem = new Regex("^[a-zA-Z0-9]*$");

            return regexItem.IsMatch(s);
        }

        private bool IsValidDate(string date)
        {
            if (String.IsNullOrEmpty(date))
                return false;

            bool result = false;
            DateTimeOffset dateTimeOffsetResult;

            if (date.Length == 8)
            {
                string format = "yyyyMMdd";
                result = DateTimeOffset.TryParseExact(date, format, null, DateTimeStyles.None, out dateTimeOffsetResult);
            }
            else if (date.Length == 10)
            {
                string format = "yyyy'-'MM'-'dd";
                result = DateTimeOffset.TryParseExact(date, format, null, DateTimeStyles.None, out dateTimeOffsetResult);
            }

            return result;
        }

        //private bool IsValidDatePeriod(string startDate, string endDate)
        //{
        //    bool result = false;

        //    if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
        //    {
        //        if (IsValidDate(startDate) && IsValidDate(endDate))
        //        {
        //            DateTimeOffset start = DateTimeOffset.Parse(startDate);
        //            DateTimeOffset end = DateTimeOffset.Parse(endDate);

        //            if (start < DateTimeOffset.Now && end > DateTimeOffset.Now)
        //            {
        //                result = true;
        //            }
        //        }
        //    }

        //    return result;
        //}

        private bool IsNotEmptyOrNotApplicable(string s)
        {
            bool result = true;

            if (String.IsNullOrWhiteSpace(s) || s.ToLower() == "not applicable")
            {
                result = false;
            }
            return result;
        }

        private DateTime ToDate(string date)
        {
            var dt = new DateTime();
            string format = "yyyyMMdd";

            DateTime.TryParseExact(date, format, null, DateTimeStyles.None, out dt);

            return dt;
        }

        private string DecimalToString(decimal? value)
        {
            return value.HasValue ? Decimal.Round(value.Value).ToString() : null;
        }

        public static DataLakeSyncResultFilesModel SaveResultToFile(string taskId, IEnumerable<string> successRecords, IEnumerable<string> errorRecords, IEnumerable<string> warningsRecords)
        {
            Guid guid = Guid.Parse(taskId);
            DataLakeSyncResultFilesModel result = new DataLakeSyncResultFilesModel()
            {
                TaskId = guid
            };
            string dataLakeSyncResultDir = AppSettingsManager.GetSetting<string>("DATALAKESYNC_RESULT_DIRECTORY", @"C:\Windows\Temp\TPM\DataLakeSyncResultFiles\");
            if (!Directory.Exists(dataLakeSyncResultDir))
            {
                Directory.CreateDirectory(dataLakeSyncResultDir);
            }
            string successPath = Path.Combine(dataLakeSyncResultDir, "SUCCESS");
            if (!Directory.Exists(successPath))
            {
                Directory.CreateDirectory(successPath);
            }
            string warningPath = Path.Combine(dataLakeSyncResultDir, "WARNING");
            if (!Directory.Exists(warningPath))
            {
                Directory.CreateDirectory(warningPath);
            }
            string errorPath = Path.Combine(dataLakeSyncResultDir, "ERROR");
            if (!Directory.Exists(errorPath))
            {
                Directory.CreateDirectory(errorPath);
            }

            // Сохранить все в файлы и сформировать модель файлов
            string successFileContent = SerializeRecords("Выполнена синхронизация записей:", successRecords);
            File.WriteAllText(Path.Combine(successPath, taskId + ".txt"), successFileContent, defaultEncoding);
            string errorFileContent = SerializeRecords("Ошибка разбора записей:", errorRecords);
            File.WriteAllText(Path.Combine(errorPath, taskId + ".txt"), errorFileContent, defaultEncoding);
            string warningFileContent = SerializeRecords("Предупреждение синхронизации записей:", warningsRecords);
            File.WriteAllText(Path.Combine(warningPath, taskId + ".txt"), warningFileContent, defaultEncoding);
            return result;
        }

        private static string SerializeRecords(string intro, IEnumerable<string> logRecords)
        {
            StringBuilder result = new StringBuilder();
            if (logRecords != null && logRecords.Any())
            {
                result.AppendLine(intro);
                foreach (var item in logRecords)
                {
                    result.AppendLine(item);
                }
            }
            return result.ToString();
        }

        private void CreateCoefficientSI2SOHandler(List<string> brandTechCode, string demandCode, double cValue)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                HandlerData data = new HandlerData();
                HandlerDataHelper.SaveIncomingArgument("brandTechCode", brandTechCode, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("demandCode", demandCode, data, visible: false, throwIfNotExists: false);
                HandlerDataHelper.SaveIncomingArgument("cValue", cValue, data, visible: false, throwIfNotExists: false);

                LoopHandler handler = new LoopHandler()
                {
                    Id = Guid.NewGuid(),
                    ConfigurationName = "PROCESSING",
                    Description = "Adding new records for coefficients SI/SO",
                    Name = "Module.Host.TPM.Handlers.CreateCoefficientSI2SOHandler",
                    ExecutionPeriod = null,
                    CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
                    LastExecutionDate = null,
                    NextExecutionDate = null,
                    ExecutionMode = Looper.Consts.ExecutionModes.SINGLE,
                    UserId = userId,
                    RoleId = roleId
                };
                handler.SetParameterData(data);
                context.LoopHandlers.Add(handler);
                context.SaveChanges();
            }
        }
    }

    static class Extension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
