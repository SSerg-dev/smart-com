using Core.Dependency;
using Core.Settings;
using Interfaces.Implementation.Action;
using Module.Persist.TPM.Model.SimpleModel;
using Module.Persist.TPM.Model.TPM;
using Module.Persist.TPM.Utils;
using Persist;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Interfaces.Implementation.Utils.Serialization;
using Looper.Parameters;
using System.IO;
using Core.Data;
using System.Collections.Concurrent;
using Utility.Import;
using System.Reflection;
using Persist.Model.Import;
using Interfaces.Implementation.Import.FullImport;
using Persist.Model;

namespace Module.Host.TPM.Actions.DataLakeIntegrationActions
{
	/// <summary>
	/// Класс для проверки наличия новых или обновленных продуктов из DataLake и создания инцидента для отправки нотификации
	/// </summary>
	public class MarsProductsCheckAction : BaseAction
	{
		private static Encoding defaultEncoding = Encoding.GetEncoding("UTF-8");
		private string handlerId;

		public MarsProductsCheckAction (string id)
		{
			this.handlerId = id;
		}

		public override void Execute ()
		{
			try
			{
				using (DatabaseContext context = new DatabaseContext())
				{
					var settingsManager = (ISettingsManager)IoC.Kernel.GetService(typeof(ISettingsManager));
					var timeRangeToCheck = settingsManager.GetSetting<int>("DATA_LAKE_MATERIALS_SYNC_TIME_RANGE", 24);
					var exceptedZREPs = settingsManager.GetSetting<string>("APP_MIX_EXCEPTED_ZREPS", null);
					var successList = new ConcurrentBag<string>();
					var errorList = new ConcurrentBag<string>();
					var warningList = new ConcurrentBag<string>();
					int sourceRecordCount;

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
						        materials.[Brandsegtech]
		
						FROM MARS_UNIVERSAL_PETCARE_MATERIALS materials
	
						WHERE materials.ZREP IS NOT NULL AND (DATEDIFF(hour, materials.VMSTD, '{today.ToString("yyyy'-'MM'-'dd HH':'mm':'ss.fff")}') < {timeRangeToCheck})
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

					if (!String.IsNullOrEmpty(exceptedZREPs))
					{
						var exceptedZREPsList = newRecords.Where(r => exceptedZREPs.Split(',').Contains(r.ZREP.TrimStart('0'))).Select(x => x.ZREP.TrimStart('0'))
							.Distinct();
						warningList.Add(String.Format("ZREPs {0} is in except list (setting 'APP_MIX_EXCEPTED_ZREPs')", String.Join(", ", exceptedZREPsList)));
						newRecords = newRecords.Where(r => !exceptedZREPs.Split(',').Contains(r.ZREP.TrimStart('0')));
					}

					// Проверка масок VKORG, 0DIVISION, 0DIVISION___T, 0MATL_TYPE___T, MATNR, VMSTD, 0CREATEDON, ZREP, EAN_Case, EAN_PC, UOM_PC2Case
					// и проверка на наличие исключенных ZREPов
					var step1Records = newRecords.Where(r =>
									IsExactNumeric(r.VKORG, 261) &&
									IsExactNumeric(r.DIVISION, 5) &&
									r.DIVISION___T == "Petcare" &&
									r.MATL_TYPE___T == "Finished products" &&
									IsLetterOrDigit(r.MATNR) &&
									IsValidDate(r.VMSTD.ToString("yyyy'-'MM'-'dd")) &&
									IsValidDate(r.CREATEDON) &&
									IsNumeric(r.ZREP.TrimStart('0'), 6) &&
									r.EAN_Case.HasValue && DecimalToString(r.EAN_Case).Length == 13 &&
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
									IsNumeric(r.ZREP.TrimStart('0'), 6) &&
									r.EAN_Case.HasValue && DecimalToString(r.EAN_Case).Length == 13 &&
									r.EAN_PC.HasValue && DecimalToString(r.EAN_PC).Length == 13 &&
									IsNumeric(r.UOM_PC2Case)));
					foreach (var item in step1Log)
					{
						warningList.Add(String.Format("GRD with ZREP {0} has inappropriate value for one or more of VKORG, 0DIVISION, 0DIVISION___T, 0MATL_TYPE___T, MATNR, VMSTD, 0CREATEDON, ZREP, EAN_Case, EAN_PC, UOM_PC2Case fields.", item.ZREP.TrimStart('0')));
					}

					// Проверка заполненности полей MATERIAL, SKU, UOM_PC2Case, Segmen, Technology, Brand_Flag_abbr, Brand_Flag, Size, Brandsegtech 
					var step2Records = step1Records.Where(r =>
									IsNotEmptyOrNotApplicable(r.MATERIAL) &&
									IsNotEmptyOrNotApplicable(r.SKU) &&
									IsNotEmptyOrNotApplicable(r.UOM_PC2Case) &&
									IsNotEmptyOrNotApplicable(r.Segmen) &&
									IsNotEmptyOrNotApplicable(r.Technology) &&
									IsNotEmptyOrNotApplicable(r.Brand_Flag_abbr) &&
									IsNotEmptyOrNotApplicable(r.Brand_Flag) &&
									IsNotEmptyOrNotApplicable(r.Size) &&
									IsNotEmptyOrNotApplicable(r.Brandsegtech));

					var step2Log = step1Records.Where(r =>
									!(IsNotEmptyOrNotApplicable(r.MATERIAL) &&
									IsNotEmptyOrNotApplicable(r.SKU) &&
									IsNotEmptyOrNotApplicable(r.UOM_PC2Case) &&
									IsNotEmptyOrNotApplicable(r.Segmen) &&
									IsNotEmptyOrNotApplicable(r.Technology) &&
									IsNotEmptyOrNotApplicable(r.Brand_Flag_abbr) &&
									IsNotEmptyOrNotApplicable(r.Brand_Flag) &&
									IsNotEmptyOrNotApplicable(r.Size) &&
									IsNotEmptyOrNotApplicable(r.Brandsegtech)));
					foreach (var item in step2Log)
					{
						warningList.Add(String.Format("GRD with ZREP {0} has one of MATERIAL, SKU, UOM_PC2Case, Segmen, Technology, Brand_Flag_abbr, Brand_Flag, Size, Brandsegtech fields not applicable or empty.", item.ZREP.TrimStart('0')));
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
									IsNotEmptyOrNotApplicable(r.Consumer_pack_format)));
					foreach (var item in step3Log)
					{
						warningList.Add(String.Format("GRD with ZREP {0} has all of Submark_Flag, Ingredient_variety, Product_Category, Product_Type, Supply_Segment, Functional_variety, Size, Brand_essence, Pack_Type, Traded_unit_format, Consumer_pack_format fields not applicable or empty.", item.ZREP.TrimStart('0')));
					}

					// Убираем различающиеся записи для одного ZREP (по полям Segmen, Technology, Brand_Flag_abbr, Brand_Flag, Size, Brandsegtech)
					var groupedRecords = step3Records.DistinctBy(y => new { y.ZREP, y.Segmen, y.Technology, y.Brand_Flag_abbr, y.Brand_Flag, y.Size, y.Brandsegtech }).GroupBy(x => x.ZREP);

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
							errorList.Add(String.Format("ZREP {0} has two or more GRD with different values in one or more of Segmen, Technology, Brand_Flag_abbr, Brand_Flag, Size, Brandsegtech fields", group.Key.TrimStart('0')));
						}
					}

					var materialsToWarning = materialsToCheck.DistinctBy(y => new { y.ZREP, y.Submark_Flag, y.Ingredient_variety, y.Product_Category, y.Product_Type, y.Supply_Segment, y.Functional_variety, y.Size, y.Brand_essence, y.Pack_Type, y.Traded_unit_format, y.Consumer_pack_format }).GroupBy(x => x.ZREP);
					foreach (var group in materialsToWarning)
					{
						if (group.Count() > 1)
						{
							warningList.Add(String.Format("ZREP {0} has two or more GRD with different values in one or more of Submark_Flag, Ingredient_variety, Product_Category, Product_Type, Supply_Segment, Functional_variety, Size, Brand_essence, Pack_Type, Traded_unit_format, Consumer_pack_format fields", group.Key.TrimStart('0')));
						}
					}

					if (!materialsToCheck.Any() && sourceRecordCount > 0)
					{
						Warnings.Add(String.Format("No materials found suitable for updating or creating products.", sourceRecordCount));
					}

					// Проверяем есть ли новые ZREP
					var products = context.Set<Product>().Where(p => !p.Disabled).ToList();
					var newProductsMaterial = materialsToCheck.Where(x => !products.Any(p => p.ZREP == x.ZREP.TrimStart('0')));

					foreach (var material in newProductsMaterial)
					{
						var errors = CheckNewBrandTech(material.Brand_code, material.Brand, material.Segmen_code, material.Tech_code, material.Technology);

						foreach (var error in errors)
							errorList.Add(error);

						if (!errors.Any())
						{
							Product newProduct = CreateProduct(material);
							context.Set<Product>().Add(newProduct);
							context.Set<ProductChangeIncident>().Add(CreateIncident(newProduct, true, false));

							try
							{
								context.SaveChanges();
								successList.Add(String.Format("Product with ZREP {0} was created.", newProduct.ZREP));
							}
							catch (Exception e)
							{
								errorList.Add(String.Format("Error while creating product with ZREP {0}. Message: {1}", newProduct.ZREP, e.Message));
							}
						}
						else
						{
							errorList.Add(String.Format("Product with ZREP {0} was not added because an error occurred while checking brand technology", material.ZREP.TrimStart('0')));
						}
					}
					if (newProductsMaterial.Any())
					{
						Results.Add(String.Format("Added notification incidents for created products with ZREPs: {0}", String.Join(",", newProductsMaterial.Select(p => p.ZREP.TrimStart('0')))), null);
					}

					// Проверяем, есть ли продукты, которые необходимо обновить
					var updateProductsMaterial = materialsToCheck.Except(newProductsMaterial);

					foreach (var material in updateProductsMaterial)
					{
						material.ZREP = material.ZREP.TrimStart('0');
						Product productToUpdate = products.Where(p => p.ZREP == material.ZREP).FirstOrDefault();

						if (productToUpdate != null)
						{
							var updatedZREPs = new List<string>();
							if (IsChange(material, productToUpdate))
							{
								var errors = CheckNewBrandTech(material.Brand_code, material.Brand, material.Segmen_code, material.Tech_code, material.Technology);

								foreach (var error in errors)
									errorList.Add(error);

								if (!errors.Any())
								{
									Product product = context.Set<Product>().Find(productToUpdate.Id);
									List<string> updatedFileds = ApplyChanges(material, product, context);

									context.Set<ProductChangeIncident>().Add(CreateIncident(product, false, false));
									updatedZREPs.Add(product.ZREP);

									try
									{
										context.SaveChanges();
										successList.Add(String.Format("Product with ZREP {0} was updated. Fields changed: {1}.", product.ZREP, String.Join(",", updatedFileds)));
									}
									catch (Exception e)
									{
										errorList.Add(String.Format("Error while updating product with ZREP {0}. Message: {1}", product.ZREP, e.Message));
									}
								}
								else
								{
									errorList.Add(String.Format("Product with ZREP {0} was not updated because an error occurred while checking brand technology", material.ZREP.TrimStart('0')));
								}
							}
							if (updatedZREPs.Any())
							{
								Results.Add(String.Format("Added notification incidents for updated products with ZREPs: {0}", String.Join(",", updatedZREPs)), null);
							}
						}
					}

					DataLakeSyncResultFilesModel model = SaveResultToFile(handlerId, successList, errorList, warningList);

					// Сохранить выходные параметры
					Results["DataLakeSyncSourceRecordCount"] = sourceRecordCount;
					Results["DataLakeSyncResultRecordCount"] = successList.Count();
					Results["ErrorCount"] = errorList.Count();
					Results["WarningCount"] = warningList.Count();
					Results["DataLakeSyncResultFilesModel"] = model;
				}
			}
			catch (Exception e)
			{
				string msg = String.Format("An error occurred while cheking Mars products", e.ToString());
				Errors.Add(msg);
			}
		}

		private List<string> CheckNewBrandTech (string brandCode, string brandName, string segmenCode, string techCode, string techName)
		{
			var errors = new List<string>();
			using (DatabaseContext context = new DatabaseContext())
			{
				if (!String.IsNullOrEmpty(brandCode) && !String.IsNullOrEmpty(segmenCode) && !String.IsNullOrEmpty(techCode))
				{
					Brand checkBrand = context.Set<Brand>().FirstOrDefault(n => n.Brand_code == brandCode && n.Segmen_code == segmenCode && !n.Disabled);
					Technology checkTech = context.Set<Technology>().FirstOrDefault(n => n.Tech_code == techCode && !n.Disabled);

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
							checkTech = new Technology { Disabled = false, Tech_code = techCode, Name = techName };
							checkTech = context.Set<Technology>().Add(checkTech);
							try
							{
								context.SaveChanges();
								Results.Add(String.Format("Added new Technology: {0}", checkTech.Name), null);
							}
							catch (Exception e)
							{
								errors.Add(String.Format("Error while adding new Technology with technology code {0}. Message: {1}", techCode, e.Message));
							}
						}

						var newBrandTech = new BrandTech { Disabled = false, BrandId = checkBrand.Id, TechnologyId = checkTech.Id };
						newBrandTech = context.Set<BrandTech>().Add(newBrandTech);
						try
						{
							context.SaveChanges();
							Results.Add(String.Format("Added new BrandTech: {0}. BrandTech code: {1}", newBrandTech.Name, newBrandTech.BrandTech_code), null);
						}
						catch (Exception e)
						{
							errors.Add(String.Format("Error while adding new BrandTech with brand code {0} and technology code {1}. Message: {2}", brandCode, techCode, e.Message));
						}
					}
				}
			}

			return errors;
		}

		private Product CreateProduct (MARS_UNIVERSAL_PETCARE_MATERIALS material)
		{
			return new Product() {
				BrandEssence = material.Brand_essence,
				BrandFlag = material.Brand_Flag,
				BrandFlagAbbr = material.Brand_Flag_abbr,
				Brandsegtech = material.Brandsegtech,
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

		private bool IsChange (MARS_UNIVERSAL_PETCARE_MATERIALS material, Product product)
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
				product.TradedUnitFormat != material.Traded_unit_format);
		}

		private List<string> ApplyChanges (MARS_UNIVERSAL_PETCARE_MATERIALS material, Product product, DatabaseContext context)
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

		private ProductChangeIncident CreateIncident (Product product, bool isCreate, bool isDelete)
		{
			return new ProductChangeIncident {
				CreateDate = (DateTimeOffset)ChangeTimeZoneUtil.ChangeTimeZone(DateTimeOffset.UtcNow),
				ProductId = product.Id,
				IsCreate = isCreate,
				IsDelete = isDelete
			};
		}

		private bool IsNumeric (string s, int length = -1)
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

		private bool IsExactNumeric (string s, int num)
		{
			if (String.IsNullOrEmpty(s))
				return false;

			bool result = false;
			int numResult;

			if (int.TryParse(s, out numResult))
				result = num == numResult ? true : false;

			return result;
		}

		private bool IsLetterOrDigit (string s, bool allowWhiteSpaces = false)
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

		private bool IsValidDate (string date)
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

		private bool IsNotEmptyOrNotApplicable (string s)
		{
			bool result = true;

			if (String.IsNullOrWhiteSpace(s) || s.ToLower() == "not applicable")
			{
				result = false;
			}
			return result;
		}

		private DateTime ToDate (string date)
		{
			var dt = new DateTime();
			string format = "yyyyMMdd";

			DateTime.TryParseExact(date, format, null, DateTimeStyles.None, out dt);

			return dt;
		}

		private string DecimalToString (decimal? value)
		{
			return value.HasValue ? Decimal.Round(value.Value).ToString() : null;
		}

		public static DataLakeSyncResultFilesModel SaveResultToFile (string taskId, IEnumerable<string> successRecords, IEnumerable<string> errorRecords, IEnumerable<string> warningsRecords)
		{
			Guid guid = Guid.Parse(taskId);
			DataLakeSyncResultFilesModel result = new DataLakeSyncResultFilesModel() {
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

		private static string SerializeRecords (string intro, IEnumerable<string> logRecords)
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
	}

	static class Extension
	{
		public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
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
