Ext.define('App.controller.tpm.incrementalpromo.IncrementalPromo', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    startEndModel: null,
    canEditInRSmode: boolean = false,

    init: function () {
        this.listen({
            component: {
                'incrementalpromo[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'incrementalpromo directorygrid': {
                    selectionchange: this.onGridSelectionChangeCustom,
                    afterrender: this.onGridIncrementalPromoAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'incrementalpromo #datatable': {
                    activate: this.onActivateCard
                },
                'incrementalpromo #detailform': {
                    activate: this.onActivateCard
                },
                'incrementalpromo #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'incrementalpromo #detailform #next': {
                    click: this.onNextButtonClick
                },
                'incrementalpromo #detail': {
                    click: this.onDetailButtonClick
                },
                'incrementalpromo #table': {
                    click: this.onTableButtonClick
                },
                'incrementalpromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'incrementalpromo #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'incrementalpromo #createbutton': {
                    click: this.onCreateButtonClick
                },
                'incrementalpromo #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'incrementalpromo #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'incrementalpromo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'incrementalpromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'incrementalpromo #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'incrementalpromo #exportbutton': {
                    click: this.onExportIncrementalPromoButtonClick
                },
                'incrementalpromo #loadimportipbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'incrementalpromo #customloadimporttemplatebutton': {
                    click: this.onCustomLoadImportTemplateButtonClick
                },
                'incrementalpromo #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                '#intpromouploadfilewindow #userOk': {
                    click: this.onUploadFileOkButtonClick
                },
            }
        });
    },

    onGridIncrementalPromoAfterrender: function (grid) {
        var RSmodeController = App.app.getController('tpm.rsmode.RSmode');
        if (!TpmModes.isRsRaMode()) {
            var indexh = this.getColumnIndex(grid, 'TPMmode');
            grid.columnManager.getColumns()[indexh].hide();
        } else {
            RSmodeController.getRSPeriod(function (returnValue) {
                startEndModel = returnValue;
            });
            var incrementalPromoGridStore = grid.getStore();
            var incrementalPromoGridStoreProxy = incrementalPromoGridStore.getProxy();

            incrementalPromoGridStoreProxy.extraParams.TPMmode = TpmModes.getSelectedMode().alias;
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

    onGridSelectionChangeCustom: function (selMode, selected) {
        if (selected[0]) {
            const tpmMode = TpmModes.getSelectedModeId();
            if (TpmModes.isRsMode(tpmMode)) {
                if (
                    (
                        new Date(selected[0].data.PromoDispatchStartDate) > new Date(startEndModel.StartDate) &&
                        new Date(selected[0].data.PromoDispatchStartDate) <= new Date(startEndModel.EndDate) &&
                        startEndModel.BudgetYear == selected[0].data.PromoBudgetYear
                    ) &&
                    (
                        selected[0].data.PromoStatusName != "Draft" &&
                        selected[0].data.PromoStatusName != "Planned" &&
                        selected[0].data.PromoStatusName != "Started" &&
                        selected[0].data.PromoStatusName != "Finished" &&
                        selected[0].data.PromoStatusName != "Closed" &&
                        selected[0].data.PromoStatusName != "Cancelled"
                    ) &&
                    (
                        !selected[0].data.IsGrowthAcceleration ||
                        !selected[0].data.IsInExchange
                    )
                ) {
                    Ext.ComponentQuery.query('incrementalpromo')[0].down('#updatebutton').enable();
                    this.canEditInRSmode = true;
                } else {
                    Ext.ComponentQuery.query('incrementalpromo')[0].down('#updatebutton').disable();
                    this.canEditInRSmode = false;
                }
            }
            else if (TpmModes.isRaMode(tpmMode)) {
                if (
                    (
                        new Date(selected[0].data.PromoDispatchStartDate) > new Date(startEndModel.StartDate) &&
                        new Date(selected[0].data.PromoDispatchStartDate) <= new Date(startEndModel.EndDate) &&
                        startEndModel.BudgetYear == selected[0].data.PromoBudgetYear
                    ) &&
                    (
                        selected[0].data.PromoStatusName != "Draft" &&
                        selected[0].data.PromoStatusName != "Planned" &&
                        selected[0].data.PromoStatusName != "Started" &&
                        selected[0].data.PromoStatusName != "Finished" &&
                        selected[0].data.PromoStatusName != "Closed" &&
                        selected[0].data.PromoStatusName != "Cancelled"
                    ) &&
                    (
                        !selected[0].data.IsGrowthAcceleration ||
                        !selected[0].data.IsInExchange
                    )
                ) {
                    Ext.ComponentQuery.query('incrementalpromo')[0].down('#updatebutton').enable();
                    this.canEditInRSmode = true;
                } else {
                    Ext.ComponentQuery.query('incrementalpromo')[0].down('#updatebutton').disable();
                    this.canEditInRSmode = false;
                }
            }
            else if (selected[0].data.PromoStatusName != 'Closed') {
                Ext.ComponentQuery.query('incrementalpromo')[0].down('#updatebutton').enable();
            } else {
                Ext.ComponentQuery.query('incrementalpromo')[0].down('#updatebutton').disable();
            }
        }
    },

    onUpdateButtonClick: function (button) {
        var grid = button.up('incrementalpromo').down('directorygrid');
        var selModel = grid.getSelectionModel();
        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0];
            this.startEditRecord(selected, grid);
        }
    },

    onDetailButtonClick: function (button) {
        var grid = button.up('incrementalpromo').down('directorygrid');
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

        if (!isHistorical && !isDeleted && toEditAccess && this.canEditInRSmode) {
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

    onOkButtonClick: function (button) {
        var form = this.editor.down('editorform').getForm(),
            record = form.getRecord();

        if (!form.isValid()) {
            return;
        }

        form.updateRecord();

        var errors = record.validate();

        if (!errors.isValid()) {
            form.markInvalid(errors);
            return;
        }

        console.log('record data: ', record.getData());
        this.saveModel(record);
    },

    saveModel: function (model, callback) {
        var isCreate = model.phantom,
            grid = this.editor.grid;

        this.editor.setLoading(l10n.ns('core').value('savingText'));

        model.data.TPMmode = TpmModes.getSelectedModeId();
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

                    var casePrice = this.editor.down('singlelinedisplayfield[name=CasePrice]');
                    var planPromoIncrementalLSV = this.editor.down('singlelinedisplayfield[name=PlanPromoIncrementalLSV]');

                    if (rec && rec.data) {
                        App.model.tpm.incrementalpromo.IncrementalPromo.load(rec.data.Id, {
                            callback: function (newModel, operation) {
                                if (newModel && newModel.data) {
                                    if (newModel.data.CasePrice != null) {
                                        casePrice.setValue(newModel.data.CasePrice);
                                        planPromoIncrementalLSV.setValue(newModel.data.PlanPromoIncrementalLSV);
                                    } else {
                                        casePrice.setValue(0);
                                    }
                                }
                            }
                        })
                    }
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
            }
        });
    },

    onCancelButtonClick: function (button) {
        //если редактирование вызвано из режима просмотра, то при отмене редактирования происходит возврат на форму просмотра
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
        // Window will close automatically.
    },

    onEditorClose: function (window) {
        var form = this.editor.down('editorform'),
            record = form.getRecord();

        form.getForm().reset(true);
        this.editor = null;
        this.detailMode = null;
    },
    onExportIncrementalPromoButtonClick: function (button) {
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
                tPMmode: TpmModes.getSelectedModeId()
            });

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
                $method: 'POST',
                tPMmode: TpmModes.getSelectedModeId()
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
    onShowImportFormButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            panel = grid.up('combineddirectorypanel'),
            viewClassName = App.Util.buildViewClassName(panel, panel.getBaseModel(), 'Import', 'ParamForm'),
            defaultResource = this.getDefaultResource(button),
            resource = Ext.String.format(button.resource || defaultResource, defaultResource),
            action = Ext.String.format(button.action, resource);

        var editor = Ext.create('App.view.core.common.UploadFileWindow', {
            title: l10n.ns('core').value('uploadFileWindowTitle'),
            itemId: 'intpromouploadfilewindow',
            parentGrid: grid,
            resource: resource,
            action: action,
            tpmmode: TpmModes.getSelectedModeId(),
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
