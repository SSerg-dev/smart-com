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

                //Brand tech
                'customnonpromosupporteditor nonpromosupportbrandtech directorygrid': {
                    selectionchange: this.onBrandTechGridSelectionChange,
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
        var clientTreeField = window.down('treesearchfield[name=ClientTreeId]');

        clientTreeField.validate();

        if (clientTreeField && clientTreeField.isValid()) {
            var customNonPromoSupportEditor = Ext.widget('customnonpromosupporteditor');
            var choosenClient = {
                fullPath: clientTreeField.rawValue,
                id: clientTreeField.value
            };

            customNonPromoSupportEditor.choosenClient = choosenClient;
            customNonPromoSupportEditor.clientId = choosenClient.id;

            customNonPromoSupportEditor.down('promosupporttoptoolbar').down('label[name=client]').setText('Client: ' + choosenClient.fullPath);

            customNonPromoSupportEditor.show(null, function () {
                window.setLoading(false);
                window.close();
            });
        } else {
            App.Notify.pushError('You should choose client before creating non-promo support.');
            window.setLoading(false);
        }
    },

    onCreateButtonClick: function (button) {
        var supportClient = Ext.widget('nonpromosupportclient');
        supportClient.show();
    },

    onAddNonPromoSupportToolBar: function (toolbar, panel) {
        Ext.ComponentQuery.query('#promoSupportCounter')[0].setText(toolbar.items.items.length);
    },

    onUpdateButtonClick: function (button) {
        var customNonPromoSupportEditor = Ext.widget('customnonpromosupporteditor');

        var nonPromoSupportGrid = button.up('panel').down('grid'),
            selModel = nonPromoSupportGrid.getSelectionModel(),
            associatedContainer = button.up('#associatednonpromosupportcontainer'),
            nonPromoSupportBrandTechDetailGrid = associatedContainer.down('nonpromosupportbrandtechdetail').down('grid'),
            nonPromoSupportBrandTechDetailStore = nonPromoSupportBrandTechDetailGrid.getStore(),
            count = nonPromoSupportBrandTechDetailStore.getCount(),
            nonPromoSupportBrandTechDetailRecords = count > 0 ? nonPromoSupportBrandTechDetailStore.getRange(0, count) : [];

        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0],
                nonPromoSupportBrandTechGrid = customNonPromoSupportEditor.down('nonpromosupportbrandtech grid'),
                nonPromoSupportBrandTechStore = nonPromoSupportBrandTechGrid.getStore(),
                nonPromoSupportBrandTechProxy = nonPromoSupportBrandTechStore.getProxy();

            var choosenClient = {
                fullPath: selected.data.ClientTreeFullPathName,
                id: selected.data.ClientTreeId
            };

            customNonPromoSupportEditor.nonPromoSupportBrandTeches = nonPromoSupportBrandTechProxy
                .getWriter()
                .writeRecords(nonPromoSupportBrandTechDetailRecords, nonPromoSupportBrandTechDetailRecords.length)
                .splice(0, nonPromoSupportBrandTechDetailRecords.length);

            customNonPromoSupportEditor.promoSupportModel = selected;
            customNonPromoSupportEditor.choosenClient = choosenClient;
            customNonPromoSupportEditor.clientId = choosenClient.id;

            customNonPromoSupportEditor.show();
            this.fillSingleNonPromoSupportForm(customNonPromoSupportEditor);
        } else {
            App.Notify.pushInfo('No selection');
        }
    },

    fillSingleNonPromoSupportForm: function (editor) {
        var nonPromoSupportForm = editor.down('nonpromosupportform'),
            model = editor.promoSupportModel;

        if (!model) {
            return;
        }

        // Заполнение поля имени клиента в хедере
        editor.down('promosupporttoptoolbar').down('label[name=client]').setText('Client: ' + model.data.ClientTreeFullPathName);
        editor.clientId = model.data.ClientTreeId;

        var nonPromoEquipmentField = nonPromoSupportForm.down('searchcombobox[name=NonPromoEquipmentId]');
        nonPromoEquipmentField.setRawValue(model.data.NonPromoEquipmentEquipmentType);
        editor.nonPromoEquipmentId = model.data.NonPromoEquipmentId;

        //InvoiceNumber
        nonPromoSupportForm.down('textfield[name=InvoiceNumber]').setValue(model.data.InvoiceNumber);

        //Parameters
        nonPromoSupportForm.down('numberfield[name=PlanQuantity]').setValue(model.data.PlanQuantity);
        nonPromoSupportForm.down('numberfield[name=ActualQuantity]').setValue(model.data.ActualQuantity);
        nonPromoSupportForm.down('numberfield[name=PlanCostTE]').setValue(model.data.PlanCostTE);
        nonPromoSupportForm.down('numberfield[name=ActualCostTE]').setValue(model.data.ActualCostTE);

        //Period
        nonPromoSupportForm.down('datefield[name=StartDate]').setValue(model.data.StartDate);
        nonPromoSupportForm.down('datefield[name=EndDate]').setValue(model.data.EndDate);

        //Attach
        var pattern = '/odata/NonPromoSupports/DownloadFile?fileName={0}';
        var downloadFileUrl = document.location.href + Ext.String.format(pattern, model.data.AttachFileName || '');
        nonPromoSupportForm.down('#attachFileName').attachFileName = model.data.AttachFileName;
        nonPromoSupportForm.down('#attachFileName').setValue(model.data.AttachFileName ? '<a href=' + downloadFileUrl + '>' + model.data.AttachFileName + '</a>' : "");

        //BrandTech
        var nonPromoSupportBrandTech = editor.down('nonpromosupportbrandtech');
        var nonPromoSupportBrandTechStore = nonPromoSupportBrandTech.down('grid').getStore();
        var nonPromoSupportBrandTechProxy = nonPromoSupportBrandTechStore.getProxy();
        var nonPromoSupportBrandTeches = nonPromoSupportBrandTechProxy.getReader().readRecords(editor.nonPromoSupportBrandTeches).records;

        nonPromoSupportBrandTechProxy.data = nonPromoSupportBrandTeches;
        nonPromoSupportBrandTechStore.load();
    },

    onSaveNonPromoSupportFormClick: function (button) {
        this.saveNonPromoSupport(button, null);
    },

    saveNonPromoSupport: function (button, callback) {
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
            nonPromoSupportForm = editor.down('nonpromosupportform'),
            nonPromoSupportBrandtech = editor.down('nonpromosupportbrandtech'),
            nonPromoSupportBrandtechGrid = nonPromoSupportBrandtech.down('grid'),
            nonPromoSupportBrandtechStore = nonPromoSupportBrandtechGrid.getStore();

        //InvoiceNumber
        var invoiceNumber = nonPromoSupportForm.down('textfield[name=InvoiceNumber]').getValue();

        if (!invoiceNumber) {
            invoiceNumber = '';
            nonPromoSupportForm.down('textfield[name=InvoiceNumber]').setValue('');
        }

        //поля на форме NonPromoSupportForm
        //Parameters
        var planQuantityValue = nonPromoSupportForm.down('numberfield[name=PlanQuantity]').getValue(),
            actualQuantityValue = nonPromoSupportForm.down('numberfield[name=ActualQuantity]').getValue(),
            planCostTEValue = nonPromoSupportForm.down('numberfield[name=PlanCostTE]').getValue(),
            actualCostTEValue = nonPromoSupportForm.down('numberfield[name=ActualCostTE]').getValue();

        // какие-то проблемы с 0 и Null, в БД Null не бывает поэтому: (можно и дефолт поставить, но не будем)
        if (!planQuantityValue) {
            planQuantityValue = 0;
            nonPromoSupportForm.down('numberfield[name=PlanQuantity]').setValue(0);
        }

        if (!actualQuantityValue) {
            actualQuantityValue = 0;
            nonPromoSupportForm.down('numberfield[name=ActualQuantity]').setValue(0);
        }

        if (!planCostTEValue) {
            planCostTEValue = 0;
            nonPromoSupportForm.down('numberfield[name=PlanCostTE]').setValue(0);
        }

        if (!actualCostTEValue) {
            actualCostTEValue = 0;
            nonPromoSupportForm.down('numberfield[name=ActualCostTE]').setValue(0);
        }

        //Period
        var startDateValueField = nonPromoSupportForm.down('datefield[name=StartDate]');
        var endDateValueField = nonPromoSupportForm.down('datefield[name=EndDate]');

        var startDateValue = startDateValueField.getValue(),
            endDateValue = endDateValueField.getValue();

        //Attach
        var attachFileField = nonPromoSupportForm.down('#attachFileName');
        var attachFileName = attachFileField.attachFileName !== undefined && attachFileField.attachFileName !== null
            ? attachFileField.attachFileName : "";

        //Заполнение модели и сохранение записи
        var model = editor.promoSupportModel ? editor.promoSupportModel : Ext.create('App.model.tpm.nonpromosupport.NonPromoSupport');

        model.editing = true;
        //nonPromoEquipmentField.validate();
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
        model.set('AttachFileName', attachFileName);

        editor.setLoading(l10n.ns('core').value('savingText'));

        var count = nonPromoSupportBrandtechStore.getCount(),
            nonPromoSupportBrandtechRecords = count > 0 ? nonPromoSupportBrandtechStore.getRange(0, count) : [];

        model.save({
            scope: me,
            success: function () {
                var brandTeches = []

                if (nonPromoSupportBrandtechRecords.length > 0) {
                    nonPromoSupportBrandtechRecords.forEach(function (record) {
                        brandTeches.push(record.data.BrandTechId);
                    });
                }

                $.ajax({
                    type: "POST",
                    cache: false,
                    url: "/odata/NonPromoSupportBrandTeches/ModifyNonPromoSupportBrandTechList?nonPromoSupportId=" + model.data.Id,
                    data: JSON.stringify(brandTeches),
                    dataType: "json",
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        var result = Ext.JSON.decode(response.value);
                        if (result.success) {
                            model.set('ClientTreeFullPathName', editor.clientFullPathName);
                            editor.promoSupportModel = model;
                            editor.setLoading(false);
                            editor.close();
                        }
                        else {
                            App.Notify.pushError(result.message);
                            editor.setLoading(false);
                        }
                    }
                });

                if (callback) {
                    callback();
                }

            },
            failure: function () {
                editor.setLoading(false);
            }
        })
    },

    onCancelNonPromoSupportFormClick: function (button) {
        var me = this;
        var editor = button.up('customnonpromosupporteditor');

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

            //Brandtech
            editor.down('nonpromosupportbrandtech').down('#addbutton').setDisabled(true);
            editor.down('nonpromosupportbrandtech').down('#deletebutton').setDisabled(true);
            editor.down('nonpromosupportbrandtech directorygrid').setDisabled(true);

            editor.down('#editNonPromoSupportEditorButton').setVisible(true);
            editor.down('#saveNonPromoSupportForm').setVisible(false);
            editor.down('#cancelNonPromoSupportForm').setVisible(false);

            var nonpromosupportbrandtech = Ext.ComponentQuery.query('nonpromosupportbrandtech')[0]; 
            nonpromosupportbrandtech.down('grid').setDisabled(false);
            nonpromosupportbrandtech.down('#deletebutton').setDisabled(true, true);

            editor.setLoading(false);
        }, 0);
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

    onDetailNonPromoSupportClick: function (item) {
        var associatedContainer = item.up('#associatednonpromosupportcontainer'),
            me = this;
        associatedContainer.setLoading(true);

        setTimeout(function () {
            var customNonPromoSupportEditor = Ext.widget('customnonpromosupporteditor');
            // дисейблим поля
            Ext.ComponentQuery.query('customnonpromosupporteditor field').forEach(function (field) {
                field.setReadOnly(true);
                field.addCls('readOnlyField');
            });

            // можно ли редактировать -> скрываем/показываем кнопку "Редактировать"
            var pointsAccess = App.UserInfo.getCurrentRole().AccessPoints;
            var access = pointsAccess.find(function (element) {
                return element.Resource == 'NonPromoSupports' && element.Action == 'Patch';
            });

            customNonPromoSupportEditor.down('#editNonPromoSupportEditorButton').setVisible(access);
            customNonPromoSupportEditor.down('#saveNonPromoSupportForm').setVisible(false);
            customNonPromoSupportEditor.down('#cancelNonPromoSupportForm').setVisible(false);
            customNonPromoSupportEditor.singleUpdateMode = true;

            var nonPromoSupportGrid = item.up('grid'),
                selModel = nonPromoSupportGrid.getSelectionModel(),
                nonPromoSupportBrandTechDetailGrid = associatedContainer.down('nonpromosupportbrandtechdetail').down('grid'),
                nonPromoSupportBrandTechDetailStore = nonPromoSupportBrandTechDetailGrid.getStore(),
                count = nonPromoSupportBrandTechDetailStore.getCount(),
                nonPromoSupportBrandTechDetailRecords = count > 0 ? nonPromoSupportBrandTechDetailStore.getRange(0, count) : [];

            if (selModel.hasSelection()) {
                var selected = selModel.getSelection()[0],
                    nonPromoSupportBrandTechGrid = customNonPromoSupportEditor.down('nonpromosupportbrandtech grid'),
                    nonPromoSupportBrandTechStore = nonPromoSupportBrandTechGrid.getStore(),
                    nonPromoSupportBrandTechProxy = nonPromoSupportBrandTechStore.getProxy();

                var choosenClient = {
                    fullPath: selected.data.ClientTreeFullPathName,
                    id: selected.data.ClientTreeId
                };

                customNonPromoSupportEditor.nonPromoSupportBrandTeches = nonPromoSupportBrandTechProxy
                    .getWriter()
                    .writeRecords(nonPromoSupportBrandTechDetailRecords, nonPromoSupportBrandTechDetailRecords.length)
                    .splice(0, nonPromoSupportBrandTechDetailRecords.length);

                customNonPromoSupportEditor.promoSupportModel = selected;
                customNonPromoSupportEditor.choosenClient = choosenClient;
                customNonPromoSupportEditor.clientId = choosenClient.id;

                me.fillSingleNonPromoSupportForm(customNonPromoSupportEditor);
            } else {
                App.Notify.pushInfo('No selection');
            }
            // кнопки прикрепления файла
            customNonPromoSupportEditor.down('#attachFile').setDisabled(true);
            customNonPromoSupportEditor.down('#deleteAttachFile').setDisabled(true);

            // BrandTech
            customNonPromoSupportEditor.down('nonpromosupportbrandtech').down('#addbutton').setDisabled(true);
            //customNonPromoSupportEditor.down('nonpromosupportbrandtech').down('#deletebutton').setDisabled(true);
            //customNonPromoSupportEditor.down('nonpromosupportbrandtech directorygrid').setDisabled(true);

            customNonPromoSupportEditor.show();
            associatedContainer.setLoading(false);
        }, 0);
    },

    onEditNonPromoSupportEditorButton: function (button) {
        var customNonPromoSupportEditor = button.up('customnonpromosupporteditor');

        Ext.ComponentQuery.query('nonpromosupportbrandtech')[0].down('#deletebutton').setDisabled(false, false);

        Ext.ComponentQuery.query('customnonpromosupporteditor field').forEach(function (field) {
            field.setReadOnly(false);
            field.removeCls('readOnlyField');
        });

        // кнопки прикрепления файла
        customNonPromoSupportEditor.down('#attachFile').setDisabled(false);
        customNonPromoSupportEditor.down('#deleteAttachFile').setDisabled(false);

        // BrandTech
        customNonPromoSupportEditor.down('nonpromosupportbrandtech').down('#addbutton').setDisabled(false);
        customNonPromoSupportEditor.down('nonpromosupportbrandtech').down('#deletebutton').setDisabled(false);
        customNonPromoSupportEditor.down('nonpromosupportbrandtech directorygrid').setDisabled(false);

        customNonPromoSupportEditor.down('#editNonPromoSupportEditorButton').setVisible(false);
        // кнопки сохранить и отменить
        customNonPromoSupportEditor.down('#saveNonPromoSupportForm').setVisible(true);
        customNonPromoSupportEditor.down('#cancelNonPromoSupportForm').setVisible(true);
    },

    onBeforeCloseNonPromoSupportEditor: function (window) {
        var masterStore = Ext.ComponentQuery.query('nonpromosupport')[0].down('grid').getStore(),
            detailStore = Ext.ComponentQuery.query('nonpromosupportbrandtechdetail')[0].down('grid').getStore();

        if (masterStore) {
            masterStore.load();
        };

        if (detailStore) {
            detailStore.load();
        }
    },

    // провести валидацию полей
    validateFields: function (editor) {
        var startDateValueField = editor.down('datefield[name=StartDate]');
        var endDateValueField = editor.down('datefield[name=EndDate]');
        var nonPromoEquipmentField = editor.down('searchcombobox[name=NonPromoEquipmentId]');

        var fieldsToValidate = [startDateValueField, endDateValueField, nonPromoEquipmentField];

        var planQuantityValue = editor.down('numberfield[name=PlanQuantity]');
        var actualQuantityValue = editor.down('numberfield[name=ActualQuantity]');
        var planCostTEValue = editor.down('numberfield[name=PlanCostTE]');
        var actualCostTEValue = editor.down('numberfield[name=ActualCostTE]');
        var invoiceNumberValue = editor.down('textfield[name=InvoiceNumber]');

        fieldsToValidate.push(planQuantityValue);
        fieldsToValidate.push(actualQuantityValue);
        fieldsToValidate.push(planCostTEValue);
        fieldsToValidate.push(actualCostTEValue);
        fieldsToValidate.push(invoiceNumberValue);

        var isValid = true;
        fieldsToValidate.forEach(function (n) {
            if (n) {
                var fieldIsValid = n.isValid();
                if (!fieldIsValid) {
                    n.validate();
                    isValid = false;
                }
            }
        })

        if (isValid == true) {
            var nonPromoSupportBrandTechGrid = editor.down('nonpromosupportbrandtech').down('grid'),
                nonPromoSupportBrandTechStore = nonPromoSupportBrandTechGrid.getStore(),
                count = nonPromoSupportBrandTechStore.getTotalCount();
            if (count === 0) {
                Ext.Msg.show({
                    title: l10n.ns('core').value('errorTitle'),
                    msg: l10n.ns('tpm', 'NonPromoSupportBrandTech').value('InvalidMsg'),
                    buttons: Ext.MessageBox.OK,
                    icon: Ext.Msg.ERROR
                });
                isValid = false;
            }
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
        }
    },

    onBrandTechGridSelectionChange: function (selModel, selected, eOpts) {
        var grid = selModel.view.up('grid'),
            container = grid.up('nonpromosupportbrandtech');

        this.brandTechUpdateButtonsState(container, selModel.hasSelection());
    },

    brandTechUpdateButtonsState: function (container, isEnabled) {
        container.query('#updatebutton, #historybutton, #deletebutton, #detail')
            .forEach(function (button) {
                button.setDisabled(!isEnabled);
            });

        var deleteButton = container.query('#deletebutton')[0];
        var editButton = Ext.ComponentQuery.query('#editNonPromoSupportEditorButton')[0]; 
        
        if (deleteButton && editButton && editButton.hidden) {
            deleteButton.setDisabled(false, false);
        } else {
            deleteButton.setDisabled(true, true);
        }
    },
});