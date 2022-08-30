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
                    click: this.onExportButtonClick
                },
                'incrementalpromo #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'incrementalpromo #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'incrementalpromo #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },

    onGridIncrementalPromoAfterrender: function (grid) {
        var RSmodeController = App.app.getController('tpm.rsmode.RSmode');
        var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
        var mode = settingStore.findRecord('name', 'mode');
        if (mode) {
            if (mode.data.value != 1) {
                var indexh = this.getColumnIndex(grid, 'TPMmode');
                grid.columnManager.getColumns()[indexh].hide();
            }
            else {
                RSmodeController.getRSPeriod(function (returnValue) {
                    startEndModel = returnValue;
                });
                var incrementalPromoGridStore = grid.getStore();
                var incrementalPromoGridStoreProxy = incrementalPromoGridStore.getProxy();

                incrementalPromoGridStoreProxy.extraParams.TPMmode = 'RS';
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

    onGridSelectionChangeCustom: function (selMode, selected) {
        if (selected[0]) {
            debugger;
            var settingStore = Ext.data.StoreManager.lookup('settingLocalStore');
            const tpmMode = settingStore.findRecord('name', 'mode').data.value;
            if (tpmMode == 1) {
                if (
                    (
                        new Date(selected[0].data.PromoDispatchStartDate) > new Date(startEndModel.StartDate) &&
                        new Date(selected[0].data.PromoDispatchStartDate) <= new Date(startEndModel.EndDate)
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
                Ext.ComponentQuery.query('incrementalpromo')[0].down('#updatebutton').disable();
            } else {
                Ext.ComponentQuery.query('incrementalpromo')[0].down('#updatebutton').enable();
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
    }
});
