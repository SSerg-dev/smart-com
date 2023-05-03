Ext.define('App.controller.tpm.actualLSV.ActualLSV', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'actuallsv[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'actuallsv directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'actuallsv #datatable': {
                    activate: this.onActivateCard
                },
                'actuallsv #detailform': {
                    activate: this.onActivateCard
                },
                'actuallsv #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'actuallsv #detailform #next': {
                    click: this.onNextButtonClick
                },
                'actuallsv #detail': {
                    click: this.onDetailButtonClick
                },
                'actuallsv #table': {
                    click: this.onTableButtonClick
                },
                'actuallsv #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'actuallsv #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'actuallsv #createbutton': {
                    click: this.onCreateButtonClick
                },
                'actuallsv #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'actuallsv #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'actuallsv #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'actuallsv #refresh': {
                    click: this.onRefreshButtonClick
                },
                'actuallsv #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'actuallsv #exportbutton': {
                    click: this.onExportButtonClick
                },
                'actuallsv #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'actuallsv #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'actuallsv #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },

    onUpdateButtonClick: function (button) {
        var grid = button.up('actuallsv').down('directorygrid');
        var selModel = grid.getSelectionModel();
        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0];
            this.startEditRecord(selected, grid);
        }
    },

    onDetailButtonClick: function (button) {
        var grid = button.up('actuallsv').down('directorygrid');
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

    startEditRecord: function (model, grid) {
        debugger;
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

        if (this.editor.down('[name=ActualPromoLSVByCompensation]').value != '' && !this.editor.down('#ok').hidden) {
            this.editor.down('numberfield[name=ActualPromoLSVSO]').setReadOnly(false);
            this.editor.down('numberfield[name=ActualPromoLSVSO]').removeCls('readOnlyFieldActualLSV');
        } else {
            this.editor.down('numberfield[name=ActualPromoLSVSO]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoLSVSO]').addCls('readOnlyFieldActualLSV');
        }

        if (model.data.InOut === 'Yes') {
            this.editor.down('numberfield[name=ActualPromoBaselineLSV]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoBaselineLSV]').addCls('readOnlyFieldActualLSV');

            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]').addCls('readOnlyFieldActualLSV');

            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]').addCls('readOnlyFieldActualLSV');
        }

        if (model.data.IsOnInvoice === 'Yes') {
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]').addCls('readOnlyFieldActualLSV');

            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]').addCls('readOnlyFieldActualLSV');
        }
        else {
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]').addCls('readOnlyFieldActualLSV');

            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]').addCls('readOnlyFieldActualLSV');
        }

        this.editor.afterWindowShow(this.editor, false);
        this.editor.down('editorform').getForm().getFields().each(function (field, index, len) {
            if (field.xtype === 'singlelinedisplayfield')
                field.setReadOnly(false);
        }, this);
    },

    onEditButtonClick: function (button) {
        debugger;
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

        if (this.editor.down('[name=ActualPromoLSVByCompensation]').value != '' && !this.editor.down('#ok').hidden) {
            this.editor.down('numberfield[name=ActualPromoLSVSO]').setReadOnly(false);
            this.editor.down('numberfield[name=ActualPromoLSVSO]').removeCls('readOnlyFieldActualLSV');
        } else {
            this.editor.down('numberfield[name=ActualPromoLSVSO]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoLSVSO]').addCls('readOnlyFieldActualLSV');
        }

        if (this.editor.model.data.InOut === 'Yes') {
            this.editor.down('numberfield[name=ActualPromoBaselineLSV]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoBaselineLSV]').addCls('readOnlyFieldActualLSV');

            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]').addCls('readOnlyFieldActualLSV');

            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]').addCls('readOnlyFieldActualLSV');
        }

        if (this.editor.model.data.IsOnInvoice === 'Yes') {
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]').addCls('readOnlyFieldActualLSV');

            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]').addCls('readOnlyFieldActualLSV');
        }
        else {
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]').addCls('readOnlyFieldActualLSV');

            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]').setReadOnly(true);
            this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]').addCls('readOnlyFieldActualLSV');
        }
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

        if (model.data.InOut === 'Yes') {
            model.data.InOut = true;
        } else {
            model.data.InOut = false;
        }

        if (model.data.IsOnInvoice === 'Yes') {
            model.data.IsOnInvoice = true;
        } else {
            model.data.IsOnInvoice = false;
        }

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
                        }
                    });
                    grid.getStore().load();
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

                    this.editor.down('numberfield[name=ActualPromoBaselineLSV]').removeCls('readOnlyFieldActualLSV');
                    this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW1]').removeCls('readOnlyFieldActualLSV');
                    this.editor.down('numberfield[name=ActualPromoPostPromoEffectLSVW2]').removeCls('readOnlyFieldActualLSV');
                    this.editor.down('numberfield[name=ActualPromoLSVSO]').removeCls('readOnlyFieldActualLSV');

                    this.editor.down('editorform').getForm().getFields().each(function (field, index, len) {
                        field.setReadOnly(true);
                        field.focus(false);
                    }, this);
                } else {
                    this.editor.close();
                }
            },
            failure: function () {
                if (callback) {
                    callback(false);
                }
                //model.reject();
                this.editor.setLoading(false);
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

        if (record) {
            record.reject();
        }

        form.getForm().reset(true);
        this.editor = null;
        this.detailMode = null;
    }
});
