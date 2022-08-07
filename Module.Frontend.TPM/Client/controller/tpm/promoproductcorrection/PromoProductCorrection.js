Ext.define('App.controller.tpm.promoproductcorrection.PromoProductCorrection', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promoproductcorrection[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'promoproductcorrection directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridPromoProductCorrectionAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promoproductcorrection #datatable': {
                    activate: this.onActivateCard
                },
                'promoproductcorrection #detailform': {
                    activate: this.onActivateCard
                },
                'promoproductcorrection #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promoproductcorrection #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promoproductcorrection #detail': {
                    click: this.onDetailButtonClick
                },
                'promoproductcorrection #table': {
                    click: this.onTableButtonClick
                },
                'promoproductcorrection #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promoproductcorrection #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promoproductcorrection #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promoproductcorrection #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promoproductcorrection #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promoproductcorrection #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promoproductcorrection #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promoproductcorrection #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promoproductcorrection #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promoproductcorrection #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promoproductcorrection #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promoproductcorrection #loadimporttemplatexlsxbuttonTLC': {
                    click: this.onLoadImportTemplateXLSXTLCButtonClick
                },
                'promoproductcorrection #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                'promoproductcorrection #exportcorrectionxlsxbutton': {
                    click: this.onExportCorrectionButtonClick
                }
            }
        });
    },


    onGridPromoProductCorrectionAfterrender: function (grid) {
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        if (mode) {
            if (mode.data.value != 1) {
                var indexh = this.getColumnIndex(grid, 'TPMmode');
                grid.columnManager.getColumns()[indexh].hide();
            }
            else {
                var promoProductCorrectionGridStore = grid.getStore();
                var promoProductCorrectionGridStoreProxy = promoProductCorrectionGridStore.getProxy();
                promoProductCorrectionGridStoreProxy.extraParams.TPMmode = 'RS';
            }
        }
        this.onGridAfterrender(grid);
    },

    getColumnIndex: function (grid, dataIndex) {
        gridColumns = grid.headerCt.getGridColumns();
        for (var i = 0; i < gridColumns.length; i++) {
            if (gridColumns[i].dataIndex == dataIndex) {
                return i;
            }
        }
    },
    
    onUpdateButtonClick: function (button) {
        var grid = button.up('promoproductcorrection').down('directorygrid');
        var selModel = grid.getSelectionModel();
        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0];
            this.startEditRecord(selected, grid);
        }
    },

    onDetailButtonClick: function (button) {
        var grid = button.up('promoproductcorrection').down('directorygrid');
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
        
        console.log('FUCK THE SYSTEM!!!');

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

    onCreateButtonClick: function () {

        this.callParent(arguments);

        var promoproductcorrectioneditor = Ext.ComponentQuery.query('promoproductcorrectioneditor')[0];
        var createDate = promoproductcorrectioneditor.down('[name=CreateDate]');
        var changeDate = promoproductcorrectioneditor.down('[name=ChangeDate]');
        var userName = promoproductcorrectioneditor.down('[name=UserName]');
        var number = promoproductcorrectioneditor.down('[name=Number]');
        userName.setValue(App.UserInfo.getUserName());
        number.isCreate = true;


        var date = new Date();
        date.setHours(date.getHours() + (date.getTimezoneOffset() / 60) + 3);   // приведение к московской timezone
        createDate.setValue(date);                                              // вывести дату в поле 
        changeDate.setValue(date);
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

    onOkButtonClick: function (button) {
        var form = this.editor.down('editorform').getForm();
            record = form.getRecord();
        
        if (!form.isValid()) {
            return;
        }

        form.updateRecord();
        var errors = record.validate();

        if (!errors.isValid()) {
            form.markInvalid(errors);
        }

        
        this.saveModelPatch(record);

    },

    saveModelPatch: function (model) {
        var isCreate = model.phantom;
        grid = this.editor.grid;

        this.editor.setLoading(l10n.ns('core').value('savingText'));

        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        model.data.TPMmode = mode.data.value;

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
            },
            failure: function () {
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
                        var promoproductcorrectioneditor = Ext.ComponentQuery.query('promoproductcorrectioneditor')[0];
                        var promoProductId = promoproductcorrectioneditor.down('[name=PromoProductId]');

                        var clientHierarchy = promoproductcorrectioneditor.down('[name=ClientHierarchy]');
                        var brandTech = promoproductcorrectioneditor.down('[name=BrandTech]');
                        var productSubrangesList = promoproductcorrectioneditor.down('[name=ProductSubrangesList]');
                        var event = promoproductcorrectioneditor.down('[name=Event]');
                        var status = promoproductcorrectioneditor.down('[name=Status]');
                        var marsStartDate = promoproductcorrectioneditor.down('[name=MarsStartDate]');
                        var marsEndDate = promoproductcorrectioneditor.down('[name=MarsEndDate]');
                        var planProductBaselineLSV = promoproductcorrectioneditor.down('[name=PlanProductBaselineLSV]');
                        var planProductIncrementalLSV = promoproductcorrectioneditor.down('[name=PlanProductIncrementalLSV]');
                        var planProductLSV = promoproductcorrectioneditor.down('[name=PlanProductLSV]');
                        var mechanic = promoproductcorrectioneditor.down('[name=Mechanic]');

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
    onExportCorrectionButtonClick: function (button) {
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'ExportCorrectionXLSX';
        var resource = button.resource || proxy.resourceName;
        panel.setLoading(true);

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST',
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
    }

});