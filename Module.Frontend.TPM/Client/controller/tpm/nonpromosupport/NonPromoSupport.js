Ext.define('App.controller.tpm.nonpromosupport.NonPromoSupport', {
	extend: 'App.controller.core.AssociatedDirectory',
	mixins: ['App.controller.core.ImportExportLogic'],

	init: function () {
		this.listen({
			component: {
				'nonpromosupport[isSearch!=true] directorygrid': {
					load: this.onGridStoreLoad,
					itemdblclick: this.onDetailNonPromoSupportClick
				},
				'nonpromosupport directorygrid': {
					selectionchange: this.onGridSelectionChange,
					afterrender: this.onGridAfterrender,
					extfilterchange: this.onExtFilterChange
				},
				'nonpromosupport #datatable': {
					activate: this.onActivateCard
				},
				'nonpromosupport #detailform': {
					activate: this.onActivateCard
				},
				'nonpromosupport #detailform #prev': {
					click: this.onPrevButtonClick
				},
				'nonpromosupport #detailform #next': {
					click: this.onNextButtonClick
				},
				'nonpromosupport #detail': {
					click: this.onDetailButtonClick
				},
				'nonpromosupport #table': {
					click: this.onTableButtonClick
				},
				'nonpromosupport #extfilterbutton': {
					click: this.onFilterButtonClick
				},
				'nonpromosupport #deletedbutton': {
					click: this.onDeletedButtonClick
				},
				'nonpromosupport #createbutton': {
					click: this.onCreateButtonClick
				},
				'nonpromosupport #updatebutton': {
					click: this.onUpdateButtonClick
				},
				'nonpromosupport #deletebutton': {
					click: this.onDeleteButtonClick
				},
				'nonpromosupport #historybutton': {
					click: this.onHistoryButtonClick
				},
				'nonpromosupport #refresh': {
					click: this.onRefreshButtonClick
				},
				'nonpromosupport #close': {
					click: this.onCloseButtonClick
				},
				// import/export
				'nonpromosupport #customexportxlsxbutton': {
					click: this.onExportBtnClick
				},
				'nonpromosupport #exportbutton': {
					click: this.onExportButtonClick
				},
				'nonpromosupport #loadimportbutton': {
					click: this.onShowImportFormButtonClick
				},
				'nonpromosupport #loadimporttemplatebutton': {
					click: this.onLoadImportTemplateButtonClick
				},
				'nonpromosupport #applyimportbutton': {
					click: this.onApplyImportButtonClick
				},

				// Choose brand tech
				'nonpromosupportbrandtech #chooseBrandTechBtn': {
					click: this.onChooseBrandTechBtnClick
				},

				// NonPromoSupportClient
				'nonpromosupportclient': {
					afterrender: this.onNonPromoSupportClientAfterRender
				},
				'nonpromosupportclient #ok': {
					click: this.onNonPromoSupportClientOkButtonClick
				},
				'nonpromosupportclient button[itemId!=ok]': {
					click: this.onSelectionButtonClick
				},

				//NonPromoSupportForm
				'nonpromosupportbottomtoolbar #saveNonPromoSupportForm': {
					click: this.onSaveNonPromoSupportFormClick
				},
				'nonpromosupportbottomtoolbar #cancelNonPromoSupportForm': {
					click: this.onCancelNonPromoSupportFormClick
				},

				//закрытие окна
				'customnonpromosupporteditor': {
					beforeclose: this.onBeforeCloseNonPromoSupportEditor
				},
				'customnonpromosupporteditor #closeNonPromoSupportEditorButton': {
					click: this.onCloseNonPromoSupportEditorButtonClick
				},
				'customnonpromosupporteditor #attachFile': {
					click: this.onAttachFileButtonClick
				},
				'customnonpromosupporteditor #deleteAttachFile': {
					click: this.onDeleteAttachFileButtonClick
				},
				'#customNonPromoSupportEditorUploadFileWindow #userOk': {
					click: this.onUploadFileOkButtonClick
				},
				'customnonpromosupporteditor #editNonPromoSupportEditorButton': {
					click: this.onEditNonPromoSupportEditorButton
				},
			}
		});
	},

	onNonPromoSupportClientAfterRender: function (window) {
		var closeButton = window.down('#close');
		var okButton = window.down('#ok');

		closeButton.setText(l10n.ns('tpm', 'NonPromoSupportClient').value('ModalWindowCloseButton'));
		okButton.setText(l10n.ns('tpm', 'NonPromoSupportClient').value('ModalWindowOkButton'));
	},

	onNonPromoSupportClientOkButtonClick: function (button) {
		var window = button.up('window');
		var clientTreeField = window.down('treesearchfield[name=ClientTreeId]'),
			me = this;

		clientTreeField.validate();

		if (clientTreeField && clientTreeField.isValid()) {
			var promoSupportData = {
				clientField: clientTreeField
			};

			//в зависимости от того, какая кнопка инициировала создание NonPromoSupport, нажатие на кнопку ОК в окне NonPromoSupportClient 
			//приводит или к открытию формы создания NonPromoSupport или к появлению еще одной панельки в левом контейнере
			window.setLoading(true);
			if (window.createNonPromoSupportButton && window.createNonPromoSupportButton.itemId == 'createNonPromoSupport') {
				var editor = window.createNonPromoSupportButton.up('customnonpromosupporteditor');
				// Костыль для появления loading...
				setTimeout(this.addNonPromoSupportPanel, 0, promoSupportData, editor);
			} else {
				window.setLoading(true);
				$.ajax({
					dataType: 'json',
					url: '/odata/NonPromoSupports/GetUserTimestamp',
					type: 'POST',
					success: function (data) {
						var customNonPromoSupportEditor = Ext.widget('customnonpromosupporteditor');
						var choosenClient = {
							fullPath: clientTreeField.rawValue,
							id: clientTreeField.value
						};

						customNonPromoSupportEditor.choosenClient = choosenClient;
						customNonPromoSupportEditor.clientId = choosenClient.id;
						customNonPromoSupportEditor.userTimestamp = data.userTimestamp;

						customNonPromoSupportEditor.down('promosupporttoptoolbar').down('label[name=client]').setText('Client: ' + choosenClient.fullPath);
						customNonPromoSupportEditor.down('nonpromosupportbrandtech').down('label[name=client]').setText('Brand tech');

						customNonPromoSupportEditor.show(null, function () {
							window.setLoading(false);
							window.close();
						});

						window.setLoading(false);
					},
					error: function (data) {
						window.setLoading(false);
						App.Notify.pushError(data.statusText);
					}
				});
			}
		} else {
			App.Notify.pushError('You should choose client before creating non-promo support.');
			window.setLoading(false);
		}
	},

	onCreateButtonClick: function (button) {
		var supportClient = Ext.widget('nonpromosupportclient');

		supportClient.show();
		supportClient.createNonPromoSupportButton = button;

		var customNonPromoSupportEditor = Ext.ComponentQuery.query('customnonpromosupporteditor')[0];
		if (customNonPromoSupportEditor) {
			//var model = customNonPromoSupportEditor.promoSupportModel;
			var clientTreeIdTreeSearchField = supportClient.down('[name=ClientTreeId]');

			clientTreeIdTreeSearchField.setDisabled(true);
			clientTreeIdTreeSearchField.setRawValue(customNonPromoSupportEditor.choosenClient.fullPath);
			clientTreeIdTreeSearchField.value = customNonPromoSupportEditor.choosenClient.id;
		}
	},

	onAddNonPromoSupportToolBar: function (toolbar, panel) {
		Ext.ComponentQuery.query('#promoSupportCounter')[0].setText(toolbar.items.items.length);
	},

	onUpdateButtonClick: function (button) {
		var customNonPromoSupportEditor = Ext.widget('customnonpromosupporteditor');

		var promoSupportGrid = button.up('panel').down('grid'),
			selModel = promoSupportGrid.getSelectionModel()

		if (selModel.hasSelection()) {
			var selected = selModel.getSelection()[0];
			var choosenClient = {
				fullPath: selected.data.ClientTreeFullPathName,
				id: selected.data.ClientTreeId
			};
			var choosenBrandTech = {
				name: selected.data.BrandTechName,
				id: selected.data.BrandTechId
			};

			customNonPromoSupportEditor.promoSupportModel = selected;
			customNonPromoSupportEditor.choosenClient = choosenClient;
			customNonPromoSupportEditor.choosenBrandTech = choosenBrandTech;
			customNonPromoSupportEditor.clientId = choosenClient.id;

			setTimeout(function () {
				customNonPromoSupportEditor.setLoading(true);
				customNonPromoSupportEditor.show();
			}, 0);

			this.fillSingleNonPromoSupportForm(customNonPromoSupportEditor);
		} else {
			App.Notify.pushInfo('No selection');
		}
	},

	fillSingleNonPromoSupportForm: function (editor) {
		var promoSupportForm = editor.down('nonpromosupportform'),
			promoSupprotBrandTech = editor.down('nonpromosupportbrandtech'),
			model = editor.promoSupportModel;
		// Заполнение поля имени клиента в хедере
		editor.down('promosupporttoptoolbar').down('label[name=client]').setText('Client: ' + model.data.ClientTreeFullPathName);
		editor.down('nonpromosupportbrandtech').down('label[name=client]').setText('Brand tech');
		editor.clientId = model.data.ClientTreeId;

		var NonPromoEquipmentField = promoSupportForm.down('searchcombobox[name=NonPromoEquipmentId]');
		NonPromoEquipmentField.setRawValue(model.data.NonPromoEquipmentEquipmentType);
		editor.nonPromoEquipmentId = model.data.NonPromoEquipmentId;

		//InvoiceNumber
		promoSupportForm.down('textfield[name=InvoiceNumber]').setValue(model.data.InvoiceNumber);

		//Parameters
		promoSupportForm.down('numberfield[name=PlanQuantity]').setValue(model.data.PlanQuantity);
		promoSupportForm.down('numberfield[name=ActualQuantity]').setValue(model.data.ActualQuantity);
		promoSupportForm.down('numberfield[name=PlanCostTE]').setValue(model.data.PlanCostTE);
		promoSupportForm.down('numberfield[name=ActualCostTE]').setValue(model.data.ActualCostTE);

		//Period
		promoSupportForm.down('datefield[name=StartDate]').setValue(model.data.StartDate);
		promoSupportForm.down('datefield[name=EndDate]').setValue(model.data.EndDate);

		//Attach
		var pattern = '/odata/NonPromoSupports/DownloadFile?fileName={0}';
		var downloadFileUrl = document.location.href + Ext.String.format(pattern, model.data.AttachFileName || '');
		promoSupportForm.down('#attachFileName').attachFileName = model.data.AttachFileName;
		promoSupportForm.down('#attachFileName').setValue(model.data.AttachFileName ? '<a href=' + downloadFileUrl + '>' + model.data.AttachFileName + '</a>' : "");

		//BrandTech
		var brandNameFiled = promoSupprotBrandTech.down('singlelinedisplayfield[name=Brand]');
		var techNameField = promoSupprotBrandTech.down('singlelinedisplayfield[name=Technology]');
		editor.brandTechId = model.data.BrandTechId;

		var param = [model.data.BrandTechId];
		breeze.EntityQuery
			.from('BrandTeches')
			.withParameters({
				$actionName: 'GetBrandTechById',
				$method: 'POST',
				$data: {
					id: param
				}
			})
			.using(Ext.ux.data.BreezeEntityManager.getEntityManager())
			.execute()
			.then(function (data) {
				var result = Ext.JSON.decode(data.httpResponse.data.value);
				if (result.success) {
					result = Ext.JSON.decode(result.data);

					brandNameFiled.setValue(result.Brand.Name);
					techNameField.setValue(result.Technology.Name);
				} else {
					App.Notify.pushError(result.data);
				}
				editor.setLoading(false);
			})
			.fail(function (data) {
				App.Notify.pushError(data.message);
				editor.setLoading(false);
			})
	},

	onSelectionButtonClick: function (button) {
		var window = button.up('window');
		var fieldsetWithButtons = window.down('fieldset');

		fieldsetWithButtons.items.items.forEach(function (item) {
			item.down('button').up('container').removeCls('promo-support-type-select-list-container-button-clicked');
		});

		button.up('container').addCls('promo-support-type-select-list-container-button-clicked');
		window.selectedButton = button;
	},

	addNonPromoSupportPanel: function (data, editor, onTheBasis) {
		var promoSupport = App.app.getController('tpm.nonpromosupport.NonPromoSupport');
		var PromoSupportClientWindow = Ext.ComponentQuery.query('nonPromoSupportClient')[0];

		promoSupportPanel = Ext.widget('nonpromosupportpanel'),
			promoSupportForm = editor.down('nonpromosupportform'),
			PromoSupportClientField = promoSupportForm.down('#PromoSupportClientField'),
			PromoSupportClientText = promoSupportPanel.down('#PromoSupportClientText'),
			NonPromoEquipmentEquipmentType = data.selectedButtonText ? data.selectedButtonText.Name : data.NonPromoEquipmentEquipmentType,
			borderColor = data.selectedButtonText ? data.selectedButtonText.ButtonColor : data.borderColor;

		promoSupportPanel.style = 'border-left: 5px solid ' + borderColor;

		// заполняем поле клиента в заголовке
		var clientFullPathName = data.clientField ? data.clientField.rawValue : data.clientFullPathName;
		editor.down('promosupporttoptoolbar').down('label[name=client]').setText('Client: ' + clientFullPathName);
		editor.PromoSupportClientData = data;
		editor.clientId = data.clientField ? data.clientField.getValue() : data.clientId;
		editor.clientFullPathName = clientFullPathName;
		editor.borderColor = borderColor;

		// фильтр по BudgetItemId для SearchComboBox Equipment Type
		var nonPromoEquipmentId = data.selectedButtonText ? data.selectedButtonText.Id : data.nonPromoEquipmentId,
			NonPromoEquipmentField = promoSupportForm.down('searchcombobox[name=NonPromoEquipmentId]'),
			NonPromoEquipmentFieldStore = NonPromoEquipmentField.getStore();
		promoSupportPanel.nonPromoEquipmentId = nonPromoEquipmentId;
		NonPromoEquipmentFieldStore.proxy.extraParams.ClientTreeId = editor.clientId;
		NonPromoEquipmentFieldStore.proxy.extraParams.NonPromoEquipmentId = nonPromoEquipmentId;

		//var parameters = {
		//    ClientTreeId: editor.clientId
		//};
		//App.Util.makeRequestWithCallback('NonPromoEquipmentClientTrees', 'GetByClient', parameters, function (data) {
		//    var result = Ext.JSON.decode(data.httpResponse.data.value);
		//    NonPromoEquipmentFieldStore.add(result.models);
		//});

		NonPromoEquipmentFieldStore.setFixedFilter('NonPromoEquipmentFilter', {
			property: 'Id',
			operation: 'Equals',
			value: nonPromoEquipmentId
		})
		NonPromoEquipmentFieldStore.load();

		PromoSupportClientField.setValue(NonPromoEquipmentEquipmentType);
		PromoSupportClientText.setText(NonPromoEquipmentEquipmentType);

		promoSupportPanel.saved = false;
		editor.promoSupportModel = null;

		if (onTheBasis) {
			promoSupportForm.down('datefield[name=StartDate]').setValue(editor.startDateValue);
			promoSupportForm.down('datefield[name=EndDate]').setValue(editor.endDateValue);

			var periodText = promoSupportPanel.down('#periodText');
			periodText.setText(' c ' + Ext.Date.format(editor.startDateValue, "d.m.Y") + ' по ' + Ext.Date.format(editor.endDateValue, "d.m.Y"));
		}

		if (PromoSupportClientWindow) {
			PromoSupportClientWindow.setLoading(false);
			PromoSupportClientWindow.close();
		}
	},

	onNonPromoSupportPanelRender: function (panel) {
		panel.body.on({
			click: { fn: this.onNonPromoSupportPanelClick, scope: panel }
		})
	},

	updateNonPromoSupportForm: function (panel) {
		var editor = panel.up('customnonpromosupporteditor'),
			promoLinkedViewer = editor.down('promolinkedviewer'),
			promoSupportForm = editor.down('nonpromosupportform');

		editor.promoSupportModel = panel.model;

		// заполняем поле клиента в заголовке
		var clientFullPathName = panel.clientFullPathName;
		editor.down('promosupporttoptoolbar').down('label[name=client]').setText('Client: ' + clientFullPathName);
		editor.PromoSupportClientData = panel.PromoSupportClientData;
		editor.clientId = panel.clientId;
		editor.clientFullPathName = panel.clientFullPathName;

		// фильтр по BudgetItemId для SearchComboBox Equipment Type
		var NonPromoEquipmentId = panel.nonPromoEquipmentId,
			NonPromoEquipmentField = promoSupportForm.down('searchcombobox[name=NonPromoEquipmentId]'),
			NonPromoEquipmentFieldStore = NonPromoEquipmentField.getStore();
		NonPromoEquipmentFieldStore.proxy.extraParams.ClientTreeId = editor.clientId;
		NonPromoEquipmentFieldStore.proxy.extraParams.NonPromoEquipmentId = nonPromoEquipmentId;
		//var parameters = {
		//    ClientTreeId: editor.clientId
		//};
		//App.Util.makeRequestWithCallback('NonPromoEquipmentClientTrees', 'GetByClient', parameters, function (data) {
		//    var result = Ext.JSON.decode(data.httpResponse.data.value);
		//    NonPromoEquipmentFieldStore.add(result.models);
		//    NonPromoEquipmentField.setValue(model.data.NonPromoEquipmentId);
		//});


		NonPromoEquipmentFieldStore.setFixedFilter('NonPromoEquipmentFilter', {
			property: 'BudgetItemId',
			operation: 'Equals',
			value: budgetItemId
		})
		NonPromoEquipmentField.setValue(panel.NonPromoEquipmentId);

		//Promo Support Type (budgetItem)
		var PromoSupportClientText = panel.down('#PromoSupportClientText').text;
		PromoSupportClientField = promoSupportForm.down('#PromoSupportClientField').setValue(PromoSupportClientText);

		//PONumber
		promoSupportForm.down('textfield[name=PONumber]').setValue(panel.poNumber);

		//InvoiceNumber
		promoSupportForm.down('textfield[name=InvoiceNumber]').setValue(panel.invoiceNumber);


		//Parameters
		var planQuantityText = panel.down('#planQuantityText').text,
			actualQuantityText = panel.down('#actualQuantityText').text,
			planCostTEText = panel.down('#planCostTEText').text,
			actualCostTEText = panel.down('#actualCostTEText').text,
			attachFileText = panel.down('#attachFileText').text;

		promoSupportForm.down('numberfield[name=PlanQuantity]').setValue(planQuantityText);
		promoSupportForm.down('numberfield[name=ActualQuantity]').setValue(actualQuantityText);
		promoSupportForm.down('numberfield[name=PlanCostTE]').setValue(planCostTEText);
		promoSupportForm.down('numberfield[name=ActualCostTE]').setValue(actualCostTEText);
		promoSupportForm.down('#attachFileName').setValue(attachFileText);

		//Period
		promoSupportForm.down('datefield[name=StartDate]').setValue(panel.startDate);
		promoSupportForm.down('datefield[name=EndDate]').setValue(panel.endDate);

		//Cost 
		promoSupportForm.down('numberfield[name=PlanProdCostPer1Item]').setValue(panel.planProdCostPer1Item);
		promoSupportForm.down('numberfield[name=ActualProdCostPer1Item]').setValue(panel.actualProdCostPer1Item);
		promoSupportForm.down('numberfield[name=PlanProductionCost]').setValue(panel.planProdCost);
		promoSupportForm.down('numberfield[name=ActualProductionCost]').setValue(panel.actualProdCost);

		//Attach
		var pattern = '/odata/NonPromoSupports/DownloadFile?fileName={0}';
		var downloadFileUrl = document.location.href + Ext.String.format(pattern, panel.attachFileName || '');
		promoSupportForm.down('#attachFileName').attachFileName = panel.attachFileName;
		promoSupportForm.down('#attachFileName').setValue('<a href=' + downloadFileUrl + '>' + panel.attachFileName || '' + '</a>');
	},

	clearNonPromoSupportForm: function (editor) {
		var promoLinkedViewer = editor.down('promolinkedviewer'),
			promoSupportForm = editor.down('nonpromosupportform');

		//очистка полей на форме NonPromoSupportForm
		var elementsToClear = promoSupportForm.query('[needClear=true]');
		elementsToClear.forEach(function (el) {
			el.setValue(null);
		});

		var attachFileField = editor.down('#attachFileName');
		attachFileField.setValue('');
		attachFileField.attachFileName = '';
	},

	onSaveNonPromoSupportFormClick: function (button) {
		this.SaveNonPromoSupport(button, null);
	},

	SaveNonPromoSupport: function (button, callback) {
		var me = this,
			editor = button.up('customnonpromosupporteditor');

		if (me.validateFields(editor)) {
			editor.setLoading(l10n.ns('core').value('savingText'));
			setTimeout(function () {
				me.generateAndSendModel(editor, callback, me);
			}, 0);
		}
	},

	generateAndSendModel: function (editor, callback, scope) {
		var me = scope,
			promoSupportForm = editor.down('nonpromosupportform')

		//InvoiceNumber
		var invoiceNumber = promoSupportForm.down('textfield[name=InvoiceNumber]').getValue();

		if (!invoiceNumber) {
			invoiceNumber = '';
			promoSupportForm.down('textfield[name=InvoiceNumber]').setValue('');
		}

		//поля на форме NonPromoSupportForm
		//Parameters
		var planQuantityValue = promoSupportForm.down('numberfield[name=PlanQuantity]').getValue(),
			actualQuantityValue = promoSupportForm.down('numberfield[name=ActualQuantity]').getValue(),
			planCostTEValue = promoSupportForm.down('numberfield[name=PlanCostTE]').getValue(),
			actualCostTEValue = promoSupportForm.down('numberfield[name=ActualCostTE]').getValue();

		// какие-то проблемы с 0 и Null, в БД Null не бывает поэтому: (можно и дефолт поставить, но не будем)
		if (!planQuantityValue) {
			planQuantityValue = 0;
			promoSupportForm.down('numberfield[name=PlanQuantity]').setValue(0);
		}

		if (!actualQuantityValue) {
			actualQuantityValue = 0;
			promoSupportForm.down('numberfield[name=ActualQuantity]').setValue(0);
		}

		if (!planCostTEValue) {
			planCostTEValue = 0;
			promoSupportForm.down('numberfield[name=PlanCostTE]').setValue(0);
		}

		if (!actualCostTEValue) {
			actualCostTEValue = 0;
			promoSupportForm.down('numberfield[name=ActualCostTE]').setValue(0);
		}

		//Period
		var startDateValueField = promoSupportForm.down('datefield[name=StartDate]');
		var endDateValueField = promoSupportForm.down('datefield[name=EndDate]');

		var startDateValue = startDateValueField.getValue(),
			endDateValue = endDateValueField.getValue();

		//Attach
		var attachFileField = promoSupportForm.down('#attachFileName');
		var attachFileName = attachFileField.attachFileName !== undefined && attachFileField.attachFileName !== null
			? attachFileField.attachFileName : "";

		//Заполнение модели и сохранение записи
		var model = editor.promoSupportModel ? editor.promoSupportModel : Ext.create('App.model.tpm.nonpromosupport.NonPromoSupport');

		model.editing = true;
		//NonPromoEquipmentField.validate();
		model.set('InvoiceNumber', invoiceNumber);
		model.set('ClientTreeId', editor.clientId);
		model.set('BrandTechId', editor.brandTechId);
		model.set('NonPromoEquipmentId', editor.nonPromoEquipmentId);
		model.set('PlanQuantity', planQuantityValue);
		model.set('ActualQuantity', actualQuantityValue);
		model.set('PlanCostTE', planCostTEValue);
		model.set('ActualCostTE', actualCostTEValue);
		model.set('StartDate', startDateValue);
		model.set('EndDate', endDateValue);
		model.set('UserTimestamp', editor.userTimestamp ? editor.userTimestamp : null);
		model.set('AttachFileName', attachFileName);

		editor.setLoading(l10n.ns('core').value('savingText'));

		model.save({
			scope: me,
			success: function () {
				model.set('ClientTreeFullPathName', editor.clientFullPathName);

				editor.promoSupportModel = model;
				editor.setLoading(false);
				editor.close()
			},
			failure: function () {
				editor.setLoading(false);
			}
		})
	},

	onCancelNonPromoSupportFormClick: function (button) {
		var me = this;
		var editor = button.up('customnonpromosupporteditor');
		var selectedItem;

		editor.setLoading(true);
		setTimeout(function () {
			me.fillSingleNonPromoSupportForm(editor);
			// дисейблим поля
			Ext.ComponentQuery.query('customnonpromosupporteditor field').forEach(function (field) {
				field.setReadOnly(true);
				field.addCls('readOnlyField');
			});
			// кнопки прикрепления файла
			editor.down('#attachFile').setDisabled(true);
			editor.down('#deleteAttachFile').setDisabled(true);

			// BrandTech btn
			editor.down('#chooseBrandTechBtn').setDisabled(true);

			editor.down('#editNonPromoSupportEditorButton').setVisible(true);
			editor.down('#saveNonPromoSupportForm').setVisible(false);
			editor.down('#cancelNonPromoSupportForm').setVisible(false);

			editor.setLoading(false);
		}, 0);
	},

	cancelUnsaveNonPromoSupport: function (editor, selectedItem) {
		var mainContainer = editor.down('#mainNonPromoSupportLeftToolbarContainer');

		if (!selectedItem.saved) {
			mainContainer.remove(selectedItem);
			//выбор предыдущей записи в контейнере
			var length = mainContainer.items.items.length,
				prevPanel = mainContainer.items.items[length - 1];

			this.clearNonPromoSupportForm(editor);

			if (prevPanel) {
				this.selectNonPromoSupportPanel(prevPanel);
				this.updateNonPromoSupportForm(prevPanel);
			}
		}
	},

	onCloseNonPromoSupportEditorButtonClick: function (button) {
		var editor = button.up('customnonpromosupporteditor');

		editor.close();
	},

	onAttachFileButtonClick: function (button) {
		var resource = 'NonPromoSupports';
		var action = 'UploadFile';

		var uploadFileWindow = Ext.widget('uploadfilewindow', {
			itemId: 'customNonPromoSupportEditorUploadFileWindow',
			resource: resource,
			action: action,
			buttons: [{
				text: l10n.ns('core', 'buttons').value('cancel'),
				itemId: 'cancel'
			}, {
				text: l10n.ns('core', 'buttons').value('upload'),
				ui: 'green-button-footer-toolbar',
				itemId: 'userOk'
			}]
		});

		uploadFileWindow.show();
	},

	onDeleteAttachFileButtonClick: function (button) {
		var attachFileName = button.up('customnonpromosupporteditor').down('#attachFileName');
		var editor = button.up('customnonpromosupporteditor');

		attachFileName.setValue('');
		attachFileName.attachFileName = '';
	},

	onUploadFileOkButtonClick: function (button) {
		var me = this;
		var win = button.up('uploadfilewindow');
		var url = Ext.String.format("/odata/{0}/{1}", win.resource, win.action);
		var needCloseParentAfterUpload = win.needCloseParentAfterUpload;
		var parentWin = win.parentGrid ? win.parentGrid.up('window') : null;
		var form = win.down('#importform');
		var paramform = form.down('importparamform');
		var isEmpty;
		if (paramform) {
			var constrains = paramform.query('field[isConstrain=true]');
			isEmpty = constrains && constrains.length > 0 && constrains.every(function (item) {
				return Ext.isEmpty(item.getValue());
			});

			if (isEmpty) {
				paramform.addCls('error-import-form');
				paramform.down('#errormsg').getEl().setVisible();
			}
		}
		if (form.isValid() && !isEmpty) {
			form.getForm().submit({
				url: url,
				waitMsg: l10n.ns('core').value('uploadingFileWaitMessageText'),
				success: function (fp, o) {
					// Проверить ответ от сервера на наличие ошибки и отобразить ее, в случае необходимости
					if (o.result) {
						win.close();
						if (parentWin && needCloseParentAfterUpload) {
							parentWin.close();
						}
						var pattern = '/odata/NonPromoSupports/DownloadFile?fileName={0}';
						var downloadFileUrl = document.location.href + Ext.String.format(pattern, o.result.fileName);
						var customnonpromosupporteditor = Ext.ComponentQuery.query('customnonpromosupporteditor')[0];

						Ext.ComponentQuery.query('#attachFileName')[0].setValue('<a href=' + downloadFileUrl + '>' + o.result.fileName + '</a>');
						Ext.ComponentQuery.query('#attachFileName')[0].attachFileName = o.result.fileName;

						App.Notify.pushInfo(win.successMessage || 'Файл был загружен на сервер');
					} else {
						App.Notify.pushError(o.result.message);
					}
				},
				failure: function (fp, o) {
					App.Notify.pushError(o.result.message || 'Ошибка при обработке запроса');
				}
			});
		}
	},

	onAttachFileNameClick: function () {
	},

	// переопределение нажатия кнопки экспорта, для определения раздела (TI Cost/Cost Production)
	onExportBtnClick: function (button) {
		var me = this;
		var grid = me.getGridByButton(button);
		var panel = grid.up('combineddirectorypanel');
		var store = grid.getStore();
		var proxy = store.getProxy();
		var actionName = button.action || 'ExportXLSX';
		var resource = button.resource || proxy.resourceName;
		panel.setLoading(true);

		var query = breeze.EntityQuery
			.from(resource)
			.withParameters({
				$actionName: actionName,
				$method: 'POST',
				section: 'ticosts'
			});

		query = me.buildQuery(query, store)
			.using(Ext.ux.data.BreezeEntityManager.getEntityManager())
			.execute()
			.then(function (data) {
				panel.setLoading(false);
				var filename = data.httpResponse.data.value;
				me.downloadFile('ExportDownload', 'filename', filename);
			})
			.fail(function (data) {
				panel.setLoading(false);
				App.Notify.pushError(me.getErrorMessage(data));
			});
	},

	// функция выбора редактируемой записи (касается визуальной части)
	selectNonPromoSupportPanel: function (panel) {
		var mainContainer = panel.up('#mainNonPromoSupportLeftToolbarContainer');
		var btnPanel = mainContainer.up('nonpromosupportlefttoolbar'); // Панель с кнопками создания/удаления

		panel.addCls('selected');
		btnPanel.down('#deleteNonPromoSupport').setDisabled(!panel.saved);
		btnPanel.down('#createNonPromoSupport').setDisabled(!panel.saved);
		btnPanel.down('#createNonPromoSupportOnTheBasis').setDisabled(!panel.saved);

		var promoLinkedStore = panel.up('customnonpromosupporteditor').down('promolinkedviewer grid').getStore();
		var promoLinkedProxy = promoLinkedStore.getProxy();

		if (panel.NonPromoSupportPromoes) {
			var promoSupportPromoes = promoLinkedProxy.getReader().readRecords(panel.NonPromoSupportPromoes).records;

			promoLinkedProxy.data = promoSupportPromoes;
			promoLinkedStore.load();
		}
		else {
			promoLinkedProxy.data = [];
			promoLinkedStore.load();
		}
	},

	// возвращает true, если появились изменения
	changedNonPromoSupport: function (promoSupportPanel) {
		var result = true;
		var editor = promoSupportPanel.up('customnonpromosupporteditor');
		var promoSupportForm = editor.down('nonpromosupportform');
		var promoLinkedViewer = editor.down('promolinkedviewer');
		var promoLinkedStore = promoLinkedViewer.down('grid').getStore();

		//поля на форме NonPromoSupportForm
		//Parameters
		var planQuantityValue = promoSupportForm.down('numberfield[name=PlanQuantity]').getValue();
		var actualQuantityValue = promoSupportForm.down('numberfield[name=ActualQuantity]').getValue();
		var planCostTEValue = promoSupportForm.down('numberfield[name=PlanCostTE]').getValue();
		var actualCostTEValue = promoSupportForm.down('numberfield[name=ActualCostTE]').getValue();

		//Period
		var startDateValue = promoSupportForm.down('datefield[name=StartDate]').getValue();
		var endDateValue = promoSupportForm.down('datefield[name=EndDate]').getValue();

		//Cost
		var planProdCostPer1Item = promoSupportForm.down('numberfield[name=PlanProdCostPer1Item]').getValue();
		var actualProdCostPer1Item = promoSupportForm.down('numberfield[name=ActualProdCostPer1Item]').getValue();
		var planProdCostValue = promoSupportForm.down('numberfield[name=PlanProductionCost]').getValue();
		var actualProdCostValue = promoSupportForm.down('numberfield[name=ActualProductionCost]').getValue();

		var attachFileName = promoSupportForm.down('#attachFileName').attachFileName;
		var NonPromoEquipmentField = promoSupportForm.down('searchcombobox[name=NonPromoEquipmentId]');
		var NonPromoEquipmentId = NonPromoEquipmentField.getValue(),

			result = result && promoSupportPanel.model.data.ActualCostTE == actualCostTEValue;
		result = result && promoSupportPanel.model.data.PlanCostTE == planCostTEValue;
		result = result && promoSupportPanel.model.data.ActualQuantity == actualQuantityValue;
		result = result && promoSupportPanel.model.data.PlanQuantity == planQuantityValue;
		result = result && promoSupportPanel.model.data.AttachFileName == attachFileName;
		result = result && promoSupportPanel.model.data.NonPromoEquipmentId == NonPromoEquipmentId;
		result = result && promoSupportPanel.model.data.StartDate.toString() == startDateValue.toString();
		result = result && promoSupportPanel.model.data.EndDate.toString() == endDateValue.toString();
		result = result && promoSupportPanel.model.data.ActualProdCostPer1Item == actualProdCostPer1Item;
		result = result && promoSupportPanel.model.data.PlanProdCostPer1Item == planProdCostPer1Item;
		result = result && promoSupportPanel.model.data.ActualProdCost == actualProdCostValue;
		result = result && promoSupportPanel.model.data.PlanProdCost == planProdCostValue;

		// проверка изменений прикрепленных промо
		var newLinkedPromoCount = promoLinkedStore.getCount();
		var promoLinkedRecords = newLinkedPromoCount > 0 ? promoLinkedStore.getRange(0, newLinkedPromoCount) : [];
		var checkedRowsIds = [];

		if (promoLinkedRecords.length > 0) {
			promoLinkedRecords.forEach(function (record) {
				checkedRowsIds.push(record.data.Id);
			});
		}

		var intersect = promoSupportPanel.NonPromoSupportPromoes.filter(function (n) {
			return checkedRowsIds.indexOf(n.Id) !== -1;
		});

		result = result && promoSupportPanel.NonPromoSupportPromoes.length == intersect.length
			&& checkedRowsIds.length == intersect.length;

		// если все верно осталось проверить только изменения распределенных значений для промо
		if (result) {
			var promoLinkedProxy = promoLinkedStore.getProxy();
			promoSupportPanel.NonPromoSupportPromoes.forEach(function (currentPsp) {
				var pspInStore = promoLinkedProxy.data.find(function (n) { return n.data.Id == currentPsp.Id })

				if (pspInStore.get('PlanCostProd') != currentPsp.PlanCostProd ||
					pspInStore.get('FactCostProd') != currentPsp.FactCostProd ||
					pspInStore.get('PlanCalculation') != currentPsp.PlanCalculation ||
					pspInStore.get('FactCalculation') != currentPsp.FactCalculation) {
					result = false;
				}
			});
		}

		return !result;
	},

	onDetailNonPromoSupportClick: function (item) {
		var customNonPromoSupportEditor = Ext.widget('customnonpromosupporteditor');

		// дисейблим поля
		Ext.ComponentQuery.query('customnonpromosupporteditor field').forEach(function (field) {
			field.setReadOnly(true);
			field.addCls('readOnlyField');
		});

		var associatedContainer = item.up('#associatednonpromosupportcontainer');

		// можно ли редактировать -> скрываем/показываем кнопку "Редактировать"
		var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;
		var access = pointsAccess.find(function (element) {
			return element.Resource == 'NonPromoSupports' && element.Action == 'Patch';
		});

		customNonPromoSupportEditor.down('#editNonPromoSupportEditorButton').setVisible(access);
		customNonPromoSupportEditor.down('#saveNonPromoSupportForm').setVisible(false);
		customNonPromoSupportEditor.down('#cancelNonPromoSupportForm').setVisible(false);
		customNonPromoSupportEditor.singleUpdateMode = true;

		var promoSupportGrid = item.up('grid'),
			selModel = promoSupportGrid.getSelectionModel()

		if (selModel.hasSelection()) {
			var selected = selModel.getSelection()[0];
			
			customNonPromoSupportEditor.promoSupportModel = selected;
			this.fillSingleNonPromoSupportForm(customNonPromoSupportEditor);
		} else {
			App.Notify.pushInfo('No selection');
		}
		// кнопки прикрепления файла
		customNonPromoSupportEditor.down('#attachFile').setDisabled(true);
		customNonPromoSupportEditor.down('#deleteAttachFile').setDisabled(true);

		// BrandTech btn
		customNonPromoSupportEditor.down('#chooseBrandTechBtn').setDisabled(true);

		customNonPromoSupportEditor.show();
	},

	onEditNonPromoSupportEditorButton: function (button) {
		var customNonPromoSupportEditor = button.up('customnonpromosupporteditor');

		Ext.ComponentQuery.query('customnonpromosupporteditor field').forEach(function (field) {
			field.setReadOnly(false);
			field.removeCls('readOnlyField');
		});

		// кнопки прикрепления файла
		customNonPromoSupportEditor.down('#attachFile').setDisabled(false);
		customNonPromoSupportEditor.down('#deleteAttachFile').setDisabled(false);

		// BrandTech btn
		customNonPromoSupportEditor.down('#chooseBrandTechBtn').setDisabled(false);

		customNonPromoSupportEditor.down('#editNonPromoSupportEditorButton').setVisible(false);
		// кнопки сохранить и отменить
		customNonPromoSupportEditor.down('#saveNonPromoSupportForm').setVisible(true);
		customNonPromoSupportEditor.down('#cancelNonPromoSupportForm').setVisible(true);
	},

	onBeforeCloseNonPromoSupportEditor: function (window) {
		var masterStore = Ext.ComponentQuery.query('nonpromosupport')[0].down('grid').getStore();

		if (masterStore) {
			masterStore.load();
		}
	},

	// провести валидацию полей
	validateFields: function (editor) {
		var startDateValueField = editor.down('datefield[name=StartDate]');
		var endDateValueField = editor.down('datefield[name=EndDate]');
		var NonPromoEquipmentField = editor.down('searchcombobox[name=NonPromoEquipmentId]');

		var fieldsToValidate = [startDateValueField, endDateValueField, NonPromoEquipmentField];

		var planQuantityValue = editor.down('numberfield[name=PlanQuantity]');
		var actualQuantityValue = editor.down('numberfield[name=ActualQuantity]');
		var planCostTEValue = editor.down('numberfield[name=PlanCostTE]');
		var actualCostTEValue = editor.down('numberfield[name=ActualCostTE]');
		var invoiceNumberValue = editor.down('textfield[name=InvoiceNumber]');
		var brandNameValue = editor.down('singlelinedisplayfield[name=Brand]');
		var techNameValue = editor.down('singlelinedisplayfield[name=Technology]');
		
		fieldsToValidate.push(planQuantityValue);
		fieldsToValidate.push(actualQuantityValue);
		fieldsToValidate.push(planCostTEValue);
		fieldsToValidate.push(actualCostTEValue);
		fieldsToValidate.push(invoiceNumberValue);
		fieldsToValidate.push(brandNameValue);
		fieldsToValidate.push(techNameValue);

		var isValid = true;
		fieldsToValidate.forEach(function (n) {
			var fieldIsValid = n.isValid();
			if (!fieldIsValid) {
				n.validate();
				isValid = false;
			}
		})

		if (isValid == true && (brandNameValue.getValue() == null || brandNameValue.getValue() == '' || techNameValue.getValue() == null || techNameValue.getValue() == '')) {
			Ext.Msg.show({
				title: l10n.ns('core').value('errorTitle'),
				msg: l10n.ns('tpm', 'NonPromoSupportBrandTech').value('InvalidMsg'),
				buttons: Ext.MessageBox.OK,
				icon: Ext.Msg.ERROR
			});
			isValid = false;
		}

		return isValid;
	},

	onChooseBrandTechBtnClick: function (button) {
        var widget = Ext.widget('nonpromosupportbrandtechchoose');
        widget.show();
    },

    onHistoryButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            var panel = grid.up('combineddirectorypanel'),
                model = panel.getBaseModel(),
                viewClassName = App.Util.buildViewClassName(panel, model, 'Historical');

            var baseReviewWindow = Ext.widget('basereviewwindow', { items: Ext.create(viewClassName, { baseModel: model }) });
            baseReviewWindow.show();

            var store = baseReviewWindow.down('grid').getStore();
            var proxy = store.getProxy();
            proxy.extraParams.Id = this.getRecordId(selModel.getSelection()[0]);

            store.setFixedFilter('HistoricalObjectId', {
                property: '_ObjectId',
                operation: 'Equals',
                value: this.getRecordId(selModel.getSelection()[0])
            });
        }
    }
});