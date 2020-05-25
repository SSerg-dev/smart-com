Ext.define('App.controller.tpm.clientkpidata.ClientKPIData', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic', 'App.controller.core.gridsetting.GridSetting'],

    init: function () {
        this.listen({
            component: {
                'clientkpidata[isSearch!=true] customlockedgrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'clientkpidata customlockedgrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'clientkpidata #datatable': {
                    activate: this.onActivateCard
                },
                'clientkpidata #detailform': {
                    activate: this.onActivateCard
                },
                'clientkpidata #detail': {
                    click: this.onDetailButtonClick
                },
                'clientkpidata #table': {
                    click: this.onTableButtonClick
                },
                'clientkpidata #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'clientkpidata #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'clientkpidata #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'clientkpidata #refresh': {
                    click: this.onRefreshButtonClick
                },
                'clientkpidata #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'clientkpidata #customexportbutton': {
                    click: this.onCustomExportButtonClick
                },
                'clientkpidata #customloadimportbutton': {
                    click: this.onCustomLoadImportButtonClick
                },
                'clientkpidata #customloadimporttemplatebutton': {
                    click: this.onCustomLoadImportTemplateButtonClick
                },
                'clientkpidata #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },

                'clientkpidata #gridsettings': {
                    click: this.onGridSettingsBtnClick
                },

                'clientkpidataeditor #edit': {
                    click: this.onEditButtonClick
                },
            }
        });
    },

    onGridStoreLoad: function (store, records, successful, eOpts) {
        var grid = eOpts.grid,
            selModel = grid.getSelectionModel();
        var selIndex = 0;
        if (selModel.hasSelection()) {
            selIndex = selModel.lastSelected.index;
        }
        selModel.select(selIndex);
    },
    onGridAfterrender: function (grid) {
        grid.getStore().on({
            scope: this,
            load: this.onGridStoreLoad,
            grid: grid
        });
        this.callParent(arguments);
    },
    onUpdateButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            this.startEditRecord(selModel.getSelection()[0], grid);
            this.setReadOnlyFields();
        } else {
            console.log('No selection');
        }
    },

    onDetailButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            this.startDetailRecord(selModel.getSelection()[0], grid);
        } else {
            console.log('No selection');
        }
    },

    onEditButtonClick: function (button) {
        this.editor.down('#ok').setVisible(true);
        this.editor.down('#canceledit').setVisible(true);
        this.editor.down('#edit').setVisible(false);
        this.editor.down('#close').setVisible(false);
        this.editor.setTitle(l10n.ns('core').value('updateWindowTitle'));
        this.detailMode = true;

        this.editor.afterWindowShow(this.editor, false);
        this.setReadOnlyFields();
    },

    setReadOnlyFields: function () {
        var window = Ext.ComponentQuery.query('clientkpidataeditor')[0];
        var ShopperTiPlanPercent = window.down('numberfield[name=ShopperTiPlanPercent]'),
            MarketingTiPlanPercent = window.down('numberfield[name=MarketingTiPlanPercent]'),
            ProductionPlan = window.down('numberfield[name=ProductionPlan]'),
            BrandingPlan = window.down('numberfield[name=BrandingPlan]'),
            BTLPlan = window.down('numberfield[name=BTLPlan]'),
            ROIPlanPercent = window.down('numberfield[name=ROIPlanPercent]'),
            IncrementalNSVPlan = window.down('numberfield[name=IncrementalNSVPlan]'),
            PromoNSVPlan = window.down('numberfield[name=PromoNSVPlan]'),
            LSVPlan = window.down('numberfield[name=LSVPlan]'),
            PromoTiCostPlanPercent = window.down('numberfield[name=PromoTiCostPlanPercent]'),
            NonPromoTiCostPlanPercent = window.down('numberfield[name=NonPromoTiCostPlanPercent]'),
            KAMEdit, DFEdit, canEdit = false;
        if (App.UserInfo.getCurrentRole()['SystemName'] === 'KeyAccountManager') {
            KAMEdit = true;
            DFEdit = false;
            DPEdit = false;
            canEdit = true;
        } else if (App.UserInfo.getCurrentRole()['SystemName'] === 'DemandFinance') {
            KAMEdit = false;
            DFEdit = true;
            DPEdit = false;
            canEdit = true;
        }
        else if (App.UserInfo.getCurrentRole()['SystemName'] === 'DemandPlanning') {
            KAMEdit = false;
            DFEdit = false;
            DPEdit = true;
            canEdit = true;
        } else if (App.UserInfo.getCurrentRole()['SystemName'] === 'Administrator'
            || App.UserInfo.getCurrentRole()['SystemName'] === 'SupportAdministrator') {
            KAMEdit = true;
            DFEdit = true;
            DPEdit = true;
            canEdit = true;
        }
        if (canEdit) {
            ShopperTiPlanPercent.setReadOnly(!DFEdit);
            MarketingTiPlanPercent.setReadOnly(!DFEdit);
            ProductionPlan.setReadOnly(!KAMEdit);
            BrandingPlan.setReadOnly(!KAMEdit);
            BTLPlan.setReadOnly(!KAMEdit);
            ROIPlanPercent.setReadOnly(!DFEdit);
            IncrementalNSVPlan.setReadOnly(!DFEdit);
            PromoNSVPlan.setReadOnly(!DFEdit);
            LSVPlan.setReadOnly(!DPEdit);
            PromoTiCostPlanPercent.setReadOnly(!DFEdit);
            NonPromoTiCostPlanPercent.setReadOnly(!DFEdit);
        }
    },

    onOkButtonClick: function () {
        var form = this.editor.down('editorform');
        this.editor.setLoading(true);
        if (form) {
            form = form.getForm();
            var record = form.getRecord();

            if (!form.isValid()) {
                return;
            }

            form.updateRecord(record);

            var errors = record.validate();

            if (!errors.isValid()) {
                form.markInvalid(errors);
                return;
            }

            var parameters = {
                ObjectId: record.data.ObjectId,
                ClientHierarchy: record.data.ClientHierarchy,
                BrandTechName: record.data.BrandTechName,
                BrandTechId: record.data.BrandTechId,
                Year: record.data.Year,
                ShopperTiPlanPercent: record.data.ShopperTiPlanPercent,
                MarketingTiPlanPercent: record.data.MarketingTiPlanPercent,
                ProductionPlan: record.data.ProductionPlan,
                BrandingPlan: record.data.BrandingPlan,
                BTLPlan: record.data.BTLPlan,
                ROIPlanPercent: record.data.ROIPlanPercent,
                IncrementalNSVPlan: record.data.IncrementalNSVPlan,
                PromoNSVPlan: record.data.PromoNSVPlan,
                PlanLSV: record.data.LSVPlan,
                PromoTiCostPlanPercent: record.data.PromoTiCostPlanPercent,
                NonPromoTiCostPlanPercent: record.data.NonPromoTiCostPlanPercent
            }
            var grid = this.grid;
            var me = this;
            App.Util.makeRequestWithCallback('ClientDashboardViews', 'Update', parameters, function (data) {
                me.editor.setLoading(false);
                me.editor.grid.getStore().load();
                me.editor.close();
            }, function (data) {
                me.editor.setLoading(false);
                if (data) {
                    App.Notify.pushError(data.message);
                }
            })
        }
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

        this.editor.down('editorform').getForm().loadRecord(model);
        this.editor.show();

        this.editor.afterWindowShow(this.editor, false);
        this.editor.down('editorform').getForm().getFields().each(function (field, index, len) {
            if (field.xtype === 'singlelinedisplayfield')
                field.setReadOnly(false);
        }, this);
    },

    startDetailRecord: function (model, grid) {
        this.editor = grid.editorModel.createEditor({ title: l10n.ns('core').value('detailWindowTitle') });
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

        this.editor.down('editorform').getForm().loadRecord(model);
        this.editor.show();
    },

    onCloseButtonClick: function (button) {
        this.editor.grid.getStore().load();
        // Window will close automatically.
    },

    onEditorClose: function (window) {
        var form = this.editor.down('editorform'),
            record = form.getRecord();

        if (record) {
            record.reject();
        }

        form.getForm().reset(true);
        this.editor = null;
        this.detailMode = null;
    },

    onCancelButtonClick: function (button) {
        //если редактирование вызвано из режима просмотра, то при отмене редактирования происходит возврат на форму просмотра
        var form = this.editor.down('editorform').getForm();
        if (form) {
            var record = form.getRecord();
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
        }
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
            var record = selModel.getSelection()[0];
            var Id = record.data.HistoryId || null;
                if (proxy.extraParams) {
                    proxy.extraParams.Id = Id;
                } else {
                    proxy.extraParams = {
                        Id: Id
                    }
            }
        }
    },

    onCustomLoadImportButtonClick: function (button) {
        var grid = button.up('menu').ownerButton
            .up('combineddirectorypanel')
            .down('customlockedgrid');

        var panel = grid.up('combineddirectorypanel'),
            viewClassName = App.Util.buildViewClassName(panel, panel.getBaseModel(), 'Import', 'ParamForm'),
            defaultResource = this.getDefaultResource(button),
            resource = Ext.String.format(button.resource || defaultResource, defaultResource),
            action = Ext.String.format(button.action, resource);

        var editor = Ext.create('App.view.core.common.UploadFileWindow', {
            title: l10n.ns('core').value('uploadFileWindowTitle'),
            parentGrid: grid,
            resource: resource,
            action: action
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

    onCustomLoadImportTemplateButtonClick: function (button) {
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'DownloadTemplateXLSX';
        var resource = button.resource || proxy.resourceName;
        panel.setLoading(true);

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST'
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

    onCustomUploadFileOkButtonClick: function (button) {
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

    onCustomExportButtonClick: function (button) {
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
                $method: 'POST'
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

    onGridSettingsBtnClick: function (button) {
        var grid = button.up('menu').ownerButton
            .up('combineddirectorypanel')
            .down('customlockedgrid');

        if (grid && grid.isConfigurable) {
            var state = grid.getCurrentState(),
                window = this.getGridSettingsWindow();

            this.loadGridsData(state, window);
            window.ownerGrid = grid;
            window.show();
        }
    },

    onRefreshButtonClick: function (button) {
        var grid = button.up('combineddirectorypanel').down('customlockedgrid').normalGrid;

        if (grid) {
            grid.getStore().load();
        }
    },

    getGridByButton: function (button) {
        return button.up('combineddirectorypanel').down('customlockedgrid');
    },

    updateDetailForm: function (container, selModel) {
    },

    //Замена directoryGrid на customlockedgrid
    onGridSelectionChange: function (selModel, selected) {
        var grid = selModel.view.up('grid'),
            container = grid.up('combineddirectorypanel');

        this.updateButtonsState(container, selModel.hasSelection());
        this.updateDetailForm(container, selModel);

        var me = this;
        var grid = selModel.view.up('customlockedgrid'),
            cdPanel = grid.up('combineddirectorypanel'),
            childrenPanels = cdPanel.getChildren();

        if (Ext.isEmpty(childrenPanels)) {
            return;
        }

        this.updateChildrenGridButtonsState(cdPanel, selModel.hasSelection());

        // Загрузка данных для дочерних таблиц, если они есть
        childrenPanels.forEach(function (item) {
            var childGrid = item.down('customlockedgrid'),
                store = childGrid.getStore(),
                filterbar = childGrid.getPlugin('filterbar'),
                cfg = cdPanel.linkConfig[item.getXType()];

            if (filterbar) {
                filterbar.clearFilters(true);
                //Дублирование метода onExtFilterChange в App.controller.core.CombinedDirectory 
                var clearButton = item.down('#extfilterclearbutton');
                if (clearButton) {
                    clearButton.setDisabled(true);
                    var text = true
                        ? l10n.ns('core', 'filter').value('filterEmptyStatus')
                        : l10n.ns('core', 'filter').value('filterNotEmptyStatus');
                    clearButton.setText(text);
                    clearButton.setTooltip(text);
                }
            }

            childGrid.getSelectionModel().deselectAll();
            store.clearAllFilters(true);

            if (selModel.hasSelection()) {
                store.setFixedFilter(cfg.detailField, {
                    property: cfg.detailField,
                    operation: 'Equals',
                    value: selected[0].get(cfg.masterField)
                });
            } else {
                store.loadData([]);
            }

            //Рекурсивно вызываем для текущих детей item
            me.onGridSelectionChange(childGrid.getSelectionModel(), childGrid.getSelectionModel().getSelection());

        });
    },
});
