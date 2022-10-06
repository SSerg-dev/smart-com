﻿Ext.define('App.controller.tpm.promoproductcorrectionpriceincrease.PromoProductCorrectionPriceIncrease', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],
    
    init: function () {
        this.listen({
            component: {
                'promoproductcorrectionpriceincrease[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'promoproductcorrectionpriceincrease directorygrid': {
                    selectionchange: this.onPromoProductCorrectionPIGridSelectionChange,
                    afterrender: this.onGridPromoProductCorrectionPIAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promoproductcorrectionpriceincrease #datatable': {
                    activate: this.onActivateCard
                },
                'promoproductcorrectionpriceincrease #detailform': {
                    activate: this.onActivateCard
                },
                'promoproductcorrectionpriceincrease #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promoproductcorrectionpriceincrease #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promoproductcorrectionpriceincrease #detail': {
                    click: this.onDetailButtonClick
                },
                'promoproductcorrectionpriceincrease #table': {
                    click: this.onTableButtonClick
                },
                'promoproductcorrectionpriceincrease #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promoproductcorrectionpriceincrease #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promoproductcorrectionpriceincrease #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promoproductcorrectionpriceincrease #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promoproductcorrectionpriceincrease #deletebutton': {
                    click: this.onDeletePromoProductCorrectionPIButtonClick
                },
                'promoproductcorrectionpriceincrease #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promoproductcorrectionpriceincrease #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promoproductcorrectionpriceincrease #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promoproductcorrectionpriceincrease #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promoproductcorrectionpriceincrease #loadimportppcbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promoproductcorrectionpriceincrease #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promoproductcorrectionpriceincrease #customloadimporttemplatebutton': {
                    click: this.onCustomLoadImportTemplateButtonClick
                },
                'promoproductcorrectionpriceincrease #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                'promoproductcorrectionpriceincrease #exportcorrectionxlsxbutton': {
                    click: this.onExportCorrectionPIButtonClick
                },                
                '#ppcuploadfilewindow #userOk': {
                    click: this.onUploadFileOkButtonClick
                },
            }
        });
    },


    onGridPromoProductCorrectionPIAfterrender: function (grid) {

        this.onGridAfterrender(grid);
    },

    onCreateButtonClick: function (button) {
        var grid = this.getGridByButton(button);
        store = grid.getStore(),
        model = Ext.create(Ext.ModelManager.getModel(store.model)),
        this.startCreateRecord(model, grid);

        var promoproductcorrectionpriceincreaseeditor = Ext.ComponentQuery.query('promoproductcorrectionpriceincreaseeditor')[0];
        var createDate = promoproductcorrectionpriceincreaseeditor.down('[name=CreateDate]');
        var changeDate = promoproductcorrectionpriceincreaseeditor.down('[name=ChangeDate]');
        var userName = promoproductcorrectionpriceincreaseeditor.down('[name=UserName]');
        var number = promoproductcorrectionpriceincreaseeditor.down('[name=Number]');
        userName.setValue(App.UserInfo.getUserName());
        number.isCreate = true;


        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);   // приведение к московской timezone
        createDate.setValue(date);                                              // вывести дату в поле 
        changeDate.setValue(date);
    },

    onUpdateButtonClick: function (button) {
        var grid = button.up('promoproductcorrectionpriceincrease').down('directorygrid');
        var selModel = grid.getSelectionModel();
        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0];
            this.startEditRecord(selected, grid);
        }
    },

    onDetailButtonClick: function (button) {
        var grid = button.up('promoproductcorrectionpriceincrease').down('directorygrid');
        selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            this.startDetailRecord(selModel.getSelection()[0], grid);
        } else {
            console.log('No selection');
        }
    },

    startDetailRecord: function (model, grid) {
        this.editor = grid.editorModel.createEditor({ title: l10n.ns('core').value('detailWindowTitle') });
        this.editor.model = model;
        this.editor.grid = grid;
        var isHistorical = this.editor.down('#historicaldetailform'),
            isDeleted = this.editor.down('#deleteddetailform');

        // если запись из гридов История или Удаленные, то скрывается кнопка Редактировать

        //Проверка на доступ к изменению данной модели для тоже скрытия
        var toEditAccess = false;
        if (model.proxy) {
            if (model.proxy.type == 'breeze') {
                toEditAccess = App.UserInfo.hasAccessPoint(model.proxy.resourceName, 'Patch');
            }
        }

        this.editor.down('editorform').getForm().getFields().each(function (field, index, len) {
            field.setReadOnly(true);
        }, this);

        if (!isHistorical && !isDeleted && toEditAccess) {
            this.editor.down('#ok').setVisible(false);
            this.editor.down('#canceledit').setVisible(false);

            this.editor.down('#ok').on('click', this.onOkButtonClick, this);
            this.editor.down('#edit').on('click', this.onEditButtonClick, this);
            this.editor.down('#canceledit').on('click', this.onCancelButtonClick, this);
            this.editor.down('#close').on('click', this.onCloseButtonClick, this);
            this.editor.on('close', this.onEditorClose, this);

        } else {
            this.editor.down('#ok').setVisible(false);
            this.editor.down('#edit').setVisible(false);
            this.editor.down('#canceledit').setVisible(false);

            this.editor.down('#close').on('click', this.onCloseButtonClick, this);
            this.editor.on('close', this.onEditorClose, this);
        }

        this.editor.down('editorform').loadRecord(model);
        this.editor.show();
    },

    startCreateRecord: function(model, grid) {
        this.editor = grid.editorModel.createEditor({
            title: l10n.ns('core').value('createWindowTitle'),
            buttons: [{
                text: l10n.ns('core', 'createWindowButtons').value('cancel'),
                itemId: 'cancel'
            }, {
                text: l10n.ns('core', 'createWindowButtons').value('ok'),
                ui: 'green-button-footer-toolbar',
                itemId: 'ok'
            }]
        });

        this.editor.down('#ok').on('click', this.onOkButtonClick, this);
        this.editor.down('#cancel').on('click', this.onCancelButtonClick, this);
        this.editor.on('close', this.onEditorClose, this);

        this.editor.down('editorform').loadRecord(model);
        this.editor.show();

        this.editor.afterWindowShow(this.editor, true);
        this.editor.down('editorform').getForm().getFields().each(function (field, index, len) {
            if (field.xtype === 'singlelinedisplayfield')
                field.setReadOnly(false);
        }, this);
    },

    startEditRecord: function (model, grid) {
        this.editor = grid.editorModel.createEditor({ title: l10n.ns('core').value('updateWindowTitle') });
        this.editor.grid = grid;
        if (this.editor.down('#edit') && this.editor.down('#close')) {
            this.editor.down('#edit').setVisible(false);
            this.editor.down('#close').setVisible(false);
        }

        this.editor.down('#ok').on('click', this.onOkButtonClick, this);
        if (this.editor.down('#canceledit'))
            this.editor.down('#canceledit').on('click', this.onCancelButtonClick, this);
        else
            this.editor.down('#close').on('click', this.onCloseButtonClick, this);
        this.editor.on('close', this.onEditorClose, this);

        this.editor.down('editorform').loadRecord(model);
        this.editor.show();

        this.editor.afterWindowShow(this.editor, false);
    },

    onEditButtonClick: function (button) {
        this.editor.down('#ok').setVisible(true);
        this.editor.down('#canceledit').setVisible(true);
        this.editor.down('#edit').setVisible(false);
        this.editor.down('#close').setVisible(false);
        this.editor.setTitle(l10n.ns('core').value('updateWindowTitle'));
        this.detailMode = true;

        this.editor.afterWindowShow(this.editor, false);
        this.editor.down('editorform').getForm().getFields().each(function (field, index, len) {
            field.setReadOnly(false);
        }, this);
    },

    onCancelButtonClick: function (button) {
        var form = this.editor.down('editorform').getForm(),
            record = form.getRecord();
        form.loadRecord(record);

        if (this.detailMode) {
            this.editor.down('#ok').setVisible(false);
            this.editor.down('#canceledit').setVisible(false);
            this.editor.down('#edit').setVisible(true);
            this.editor.down('#close').setVisible(true);
            this.editor.setTitle(l10n.ns('core').value('detailWindowTitle'));
            this.detailMode = false;

            this.editor.down('editorform').getForm().getFields().each(function (field, index, len) {
                field.setReadOnly(true);
                field.focus(false);
            }, this);
        } else {
            this.editor.close();
        }
    },

    onCloseButtonClick: function (button) {
        this.editor.grid.getStore().load();
    },

    getAndSaveFormData: function() {
        var form = this.editor.down('editorform').getForm();
        record = form.getRecord();        
    
        if (!form.isValid()) {
            return;
        }

        form.updateRecord(record);
        var errors = record.validate();

        if (!errors.isValid()) {
            form.markInvalid(errors);
        }

        values = form.getValues();
        record.set(values);

        this.saveModelPatch(record);
    },

    onOkButtonClick: function (button) {
        this.getAndSaveFormData();
    },

    saveModelPatch: function (model, callback) {
        var isCreate = model.phantom;
        grid = this.editor.grid;
        this.editor.setLoading(l10n.ns('core').value('savingText'));
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');


        var tPMmode = mode.data.value === 1? 'RS' : 'Current';

        model.set('TPMmode', tPMmode);

        model.save({
            scope: this,
            success: function (rec, resp, opts) {
                if (callback) {
                    callback(true);
                }
                if (grid) {
                    grid.getStore().on({
                        single: true,
                        scope: this,
                        load: function (records, operation, success) {
                            model.set('Key');
                            if (typeof grid.afterSaveCallback === 'function') {
                                grid.afterSaveCallback(grid);
                            }
                        },
                    });

                    grid.getStore().load();

                    this.editor.setLoading(false);

                    //если редактирование вызвано из режима просмотра, то при сохранении происходит возврат на форму просмотра
                    if (this.detailMode) {
                        this.editor.down('#ok').setVisible(false);
                        this.editor.down('#canceledit').setVisible(false);
                        this.editor.down('#edit').setVisible(true);
                        this.editor.down('#close').setVisible(true);
                        this.editor.setTitle(l10n.ns('core').value('detailWindowTitle'));
                        this.detailMode = false;

                        this.editor.down('editorform').getForm().getFields().each(function (field, index, len) {
                            field.setReadOnly(true);
                            field.focus(false);
                        }, this);
                    } else {
                        this.editor.close();
                    }               
                }
                if (resp.action == 'create') {
                    this.editor.setLoading(false);
                    this.editor.close();
                }                
            },
            failure: function (fff) {
                if (callback) {
                    callback(false);
                }
                this.editor.setLoading(false);
                this.editor.close();
                grid.getStore().load();
            }
        });
    },

    //Получение промо продукта по промо и продукту
    saveModel: function (promoId, productId) {
        if (promoId && productId) {

            var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
            var mode = settingStore.findRecord('name', 'mode');

            var parameters = {
                promoId: breeze.DataType.Guid.fmtOData(promoId),
                productId: breeze.DataType.Guid.fmtOData(productId),
                mode: mode.data.value

            };

            App.Util.makeRequestWithCallback('PromoProducts', 'GetPromoProductByPromoAndProduct', parameters, function (data) {
                var result = Ext.JSON.decode(data.httpResponse.data.value);
                if (result.success) {

                    if (result.models.length !== 0) {
                        var promoproductcorrectionpriceincreaseeditor = Ext.ComponentQuery.query('promoproductcorrectionpriceincreaseeditor')[0];
                        var promoProductId = promoproductcorrectionpriceincreaseeditor.down('[name=PromoProductId]');

                        var clientHierarchy = promoproductcorrectionpriceincreaseeditor.down('[name=ClientHierarchy]');
                        var brandTech = promoproductcorrectionpriceincreaseeditor.down('[name=BrandTechName]');
                        var productSubrangesList = promoproductcorrectionpriceincreaseeditor.down('[name=ProductSubrangesList]');
                        var event = promoproductcorrectionpriceincreaseeditor.down('[name=EventName]');
                        var status = promoproductcorrectionpriceincreaseeditor.down('[name=PromoStatusSystemName]');
                        var marsStartDate = promoproductcorrectionpriceincreaseeditor.down('[name=MarsStartDate]');
                        var marsEndDate = promoproductcorrectionpriceincreaseeditor.down('[name=MarsEndDate]');
                        var planProductBaselineLSV = promoproductcorrectionpriceincreaseeditor.down('[name=PlanProductBaselineLSV]');
                        var planProductIncrementalLSV = promoproductcorrectionpriceincreaseeditor.down('[name=PlanProductIncrementalLSV]');
                        var planProductLSV = promoproductcorrectionpriceincreaseeditor.down('[name=PlanProductLSV]');
                        var mechanic = promoproductcorrectionpriceincreaseeditor.down('[name=MarsMechanicName]');

                        promoProductId.setValue(result.models.Id);
                        clientHierarchy.setValue(result.models.Promo.ClientHierarchy);
                        brandTech.setValue(result.models.Promo.BrandTech.Name);
                        productSubrangesList.setValue(result.models.Promo.ProductSubrangesList);
                        mechanic.setValue(result.models.Promo.Mechanic);
                        event.setValue(result.models.Promo.Event.Name);
                        status.setValue(result.models.Promo.PromoStatus.Name);
                        marsStartDate.setValue(result.models.Promo.MarsStartDate);
                        marsEndDate.setValue(result.models.Promo.MarsEndDate);
                        planProductBaselineLSV.setValue(result.models.PlanProductBaselineLSV);
                        planProductIncrementalLSV.setValue(result.models.PlanProductIncrementalLSV);
                        planProductLSV.setValue(result.models.PlanProductLSV);
                    } else {
                        App.Notify.pushError('Group is empty');

                    }

                }
                else {
                    App.Notify.pushError(result.message);
                }
            });
        }

    },

    onExportCorrectionPIButtonClick: function (button) {
        var actionName = button.action || 'ExportCorrectionXLSX';
        this.ExportPromoProductCorrection(actionName, button);
    },

    onExportButtonClick: function (button) {
        var actionName = button.action || 'ExportXLSX';
        this.ExportPromoProductCorrection(actionName, button)
    },

    ExportPromoProductCorrection: function(actionName, button) {
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var resource = button.resource || proxy.resourceName;
        panel.setLoading(true);

        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        
        var query = breeze.EntityQuery
        .from(resource)
        .withParameters({
            $actionName: actionName,
            $method: 'POST',
            TPMmode: mode?.data?.value
        });
        
        // тут store фильтр не работает на бэке другой запрос
        query = me.buildQuery(query, store)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                panel.setLoading(false);
                App.Notify.pushInfo('Export task created successfully');
                App.System.openUserTasksPanel()
            })
            .fail(function (data) {
                panel.setLoading(false);
                App.Notify.pushError(me.getErrorMessage(data));
            });
    },    

    onEditorClose: function (window) {
        var form = this.editor.down('editorform'),
            record = form.getRecord();

        form.getForm().reset(true);
        this.editor = null;
        this.detailMode = null;
    },
    onDeletePromoProductCorrectionPIButtonClick: function(button) {
        var grid = this.getGridByButton(button),
            panel = grid.up('combineddirectorypanel'),
            selModel = grid.getSelectionModel();

        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');


        if (mode) {
            if (mode.data.value == 1) {
                if (selModel.hasSelection()) {
                    Ext.Msg.show({
                        title: l10n.ns('core').value('deleteWindowTitle'),
                        msg: l10n.ns('core').value('deleteConfirmMessage'),
                        fn: onMsgBoxClose,
                        scope: this,
                        icon: Ext.Msg.QUESTION,
                        buttons: Ext.Msg.YESNO,
                        buttonText: {
                            yes: l10n.ns('core', 'buttons').value('delete'),
                            no: l10n.ns('core', 'buttons').value('cancel')
                        }
                    });
                } else {
                    console.log('No selection');
                }

                function onMsgBoxClose(buttonId) {
                    if (buttonId === 'yes') {
                        var record = selModel.getSelection()[0],
                            store = grid.getStore(),
                            view = grid.getView(),
                            currentIndex = store.indexOf(record),
                            pageIndex = store.getPageFromRecordIndex(currentIndex),
                            endIndex = store.getTotalCount() - 2; // 2, т.к. после удаления станет на одну запись меньше

                        currentIndex = Math.min(Math.max(currentIndex, 0), endIndex);
                        panel.setLoading(l10n.ns('core').value('deletingText'));


                        $.ajax({
                            type: "POST",
                            cache: false,
                            url: "/odata/PromoProductCorrectionViews/PromoProductCorrectionDelete?key=" + record.data.Id + '&TPMmode=' + mode.data.value,
                            dataType: "json",
                            contentType: false,
                            processData: false,
                            success: function (response) {
                                var result = Ext.JSON.decode(response.value);
                                if (result.success) {
                                    store.on('load', function () {
                                        panel.setLoading(false);
                                    });

                                    store.load();
                                } else {
                                    App.Notify.pushError(result.message);
                                    panel.setLoading(false);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                App.Notify.pushError();
                                panel.setLoading(false);
                            }
                        });
                    }
                }
            }
            else {
                this.onDeleteButtonClick(button);
            }
        }
        else {
            this.onDeleteButtonClick(button);
        }
    },

    onPromoProductCorrectionPIGridSelectionChange: function (selMode, selected) {
        if (selected[0]) {
            var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
            const tpmMode = settingStore.findRecord('name', 'mode').data.value;
            if (selected[0].data.PromoStatusName != 'Closed') {
                Ext.ComponentQuery.query('promoproductcorrectionpriceincrease')[0].down('#updatebutton').enable();
                Ext.ComponentQuery.query('promoproductcorrectionpriceincrease')[0].down('#deletebutton').enable();
            } else {
                Ext.ComponentQuery.query('promoproductcorrectionpriceincrease')[0].down('#updatebutton').disable();
                Ext.ComponentQuery.query('promoproductcorrectionpriceincrease')[0].down('#deletebutton').disable();
            }
        }
    },

    onCustomLoadImportTemplateButtonClick: function (button) {
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'DownloadTemplateXLSX';
        var resource = button.resource || proxy.resourceName;
        panel.setLoading(true);

        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST',
                tPMmode: mode?.data?.value
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

    onShowImportFormButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            panel = grid.up('combineddirectorypanel'),
            viewClassName = App.Util.buildViewClassName(panel, panel.getBaseModel(), 'Import', 'ParamForm'),
            defaultResource = this.getDefaultResource(button),
            resource = Ext.String.format(button.resource || defaultResource, defaultResource),
            action = Ext.String.format(button.action, resource);

        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');

        var editor = Ext.create('App.view.core.common.UploadFileWindow', {
            title: l10n.ns('core').value('uploadFileWindowTitle'),
            itemId: 'ppcuploadfilewindow',
            parentGrid: grid,
            resource: resource,
            action: action,
            tpmmode: mode?.data?.value,
            buttons: [{
                text: l10n.ns('core', 'buttons').value('cancel'),
                itemId: 'cancel'
            }, {
                text: l10n.ns('core', 'buttons').value('upload'),
                ui: 'green-button-footer-toolbar',
                itemId: 'userOk'
            }]
        });

        if (button.additionParameters) {
            var fields = [];
            for (var param in button.additionParameters) {
                if (button.hasOwnProperty(param)) {
                    fields.push({
                        xtype: 'hiddenfield',
                        name: param,
                        value: button.additionParameters[param]
                    });
                }
            }
            editor.down('editorform').add(fields);
        }
        var btnBrowse = editor.down('filefield');
        if (btnBrowse) {
            var allowFormat = button.allowFormat || ['csv', 'zip'];
            btnBrowse.allowFormat = allowFormat;
            btnBrowse.vtypeText = 'Формат файла не поддерживается. Необходим файл формата: ' + allowFormat.join(',');
        }

        if (Ext.ClassManager.get(viewClassName)) {
            var paramForm = Ext.create(viewClassName);
            var fieldValues = button.fieldValues ? Ext.clone(button.fieldValues) : null;
            paramForm.initFields(fieldValues);
            editor.down('#importform').insert(0, paramForm);
        }
        editor.show();
    },
    onUploadFileOkButtonClick: function (button) {
        var me = this;
        var win = button.up('uploadfilewindow');
        var url = Ext.String.format("/odata/{0}/{1}?tPMmode={2}", win.resource, win.action, win.tpmmode);
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
                        var infoText = win.successMessage || 'Задача обработки импортируемого файла успешно создана';
                        App.Notify.pushInfo(infoText);
                        // Открыть панель задач
                        if (!win.isNotTask) {
                            App.System.openUserTasksPanel();
                        }
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
});