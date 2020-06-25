Ext.define('App.controller.tpm.brand.Brand', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'brand[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'brand directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'brand #datatable': {
                    activate: this.onActivateCard
                },
                'brand #detailform': {
                    activate: this.onActivateCard
                },
                'brand #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'brand #detailform #next': {
                    click: this.onNextButtonClick
                },
                'brand #detail': {
                    click: this.onDetailButtonClick
                },
                'brand #table': {
                    click: this.onTableButtonClick
                },
                'brand #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'brand #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'brand #createbutton': {
                    click: this.onCreateButtonClick
                },
                'brand #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'brand #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'brand #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'brand #refresh': {
                    click: this.onRefreshButtonClick
                },
                'brand #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'brand #exportbutton': {
                    click: this.onExportButtonClick
                },
                'brand #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'brand #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'brand #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },
    onUpdateButtonClick: function (button) {
        var grid = button.up('brand').down('directorygrid');
        var selModel = grid.getSelectionModel();
        if (selModel.hasSelection()) {
            var selected = selModel.getSelection()[0];
            this.startEditRecord(selected, grid);
        }
    },

    onDetailButtonClick: function (button) {
        var grid = button.up('brand').down('directorygrid');
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

        this.editor.down('textfield[name=Brand_code]').setReadOnly(true);
        this.editor.down('textfield[name=Brand_code]').addCls('readOnlyField');
        this.editor.down('textfield[name=Segmen_code]').setReadOnly(true);
        this.editor.down('textfield[name=Segmen_code]').addCls('readOnlyField');

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

        this.editor.down('textfield[name=Brand_code]').setReadOnly(true);
        this.editor.down('textfield[name=Brand_code]').addCls('readOnlyField');
        this.editor.down('textfield[name=Segmen_code]').setReadOnly(true);
        this.editor.down('textfield[name=Segmen_code]').addCls('readOnlyField');
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
                field.removeCls('readOnlyField');;
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
