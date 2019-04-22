Ext.define('Core.form.EditorDetailWindowModel', {
    extend: 'Core.form.BaseEditorModel',

    startCreateRecord: function (model) {
        this.editor = this.createEditor({
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

        this.getForm().loadRecord(model);
        this.editor.show();

        this.editor.afterWindowShow(this.editor, true);
        this.getForm().getForm().getFields().each(function (field, index, len) {
            if (field.xtype === 'singlelinedisplayfield')
                field.setReadOnly(false);
        }, this);
    },

    startEditRecord: function (model) {
        this.editor = this.createEditor({ title: l10n.ns('core').value('updateWindowTitle') });
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

        this.getForm().loadRecord(model);
        this.editor.show();

        this.editor.afterWindowShow(this.editor, false);
        this.getForm().getForm().getFields().each(function (field, index, len) {
            if (field.xtype === 'singlelinedisplayfield')
                field.setReadOnly(false);
        }, this);
    },

    startDetailRecord: function (model) {
        this.editor = this.createEditor({ title: l10n.ns('core').value('detailWindowTitle') });
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

        this.getForm().getForm().getFields().each(function (field, index, len) {
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

        this.getForm().loadRecord(model);
        this.editor.show();
    },

    onOkButtonClick: function (button) {
        var form = this.getForm().getForm(),
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

    onEditButtonClick: function (button) {
        this.editor.down('#ok').setVisible(true);
        this.editor.down('#canceledit').setVisible(true);
        this.editor.down('#edit').setVisible(false);
        this.editor.down('#close').setVisible(false);
        this.editor.setTitle(l10n.ns('core').value('updateWindowTitle'));
        this.detailMode = true;

        this.editor.afterWindowShow(this.editor, false);
        this.getForm().getForm().getFields().each(function (field, index, len) {
            field.setReadOnly(false);
        }, this);
    },

    onCancelButtonClick: function (button) {
        //если редактирование вызвано из режима просмотра, то при отмене редактирования происходит возврат на форму просмотра
        var form = this.getForm(),
            record = form.getRecord();
        form.loadRecord(record);

        if (this.detailMode) {
            this.editor.down('#ok').setVisible(false);
            this.editor.down('#canceledit').setVisible(false);
            this.editor.down('#edit').setVisible(true);
            this.editor.down('#close').setVisible(true);
            this.editor.setTitle(l10n.ns('core').value('detailWindowTitle'));
            this.detailMode = false;

            this.getForm().getForm().getFields().each(function (field, index, len) {
                field.setReadOnly(true);
                field.focus(false);
            }, this);
        } else {
            this.editor.close();
        }
    },

    onCloseButtonClick: function (button) {
        // Window will close automatically.
    },

    onEditorClose: function (window) {
        var form = this.getForm(),
            record = form.getRecord();

        if (record) {
            record.reject();
        }

        form.getForm().reset(true);
        this.editor = null;
        this.detailMode = null;
    },

    getForm: function () {
        if (this.editor) {
            return this.editor.down('editorform');
        }
    },

    saveModel: function (model, callback) {
        var isCreate = model.phantom,
            grid = this.getGrid(),
            tree = this.getTree();

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
                        }
                    });
                    grid.getStore().load();
                } else {
                    tree.getStore().on({
                        single: true,
                        scope: this,
                        load: function (records, operation, success) {
                            model.set('Key');
                            if (typeof tree.afterSaveCallback === 'function') {
                                tree.afterSaveCallback(tree, model);
                            }
                        }
                    });
                    tree.getStore().load();
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

                    this.getForm().getForm().getFields().each(function (field, index, len) {
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

    // Privates. 

    createEditor: function (cfg) {
        var grid = this.getGrid(),
            tree = this.getTree(), // Возможен вызов из дерева
            tmpModel = grid ? grid.getStore().model : tree.getStore().model,
            model = Ext.ModelManager.getModel(tmpModel),
            panel = grid ? grid.up('combineddirectorypanel') : tree.up('combineddirectorytreepanel'),
            historicalItem = panel.id.indexOf('historical'),
            deletedItem = panel.id.indexOf('deleted'),
            viewClassName;

        if (historicalItem !== -1 || deletedItem !== -1)
            viewClassName = App.Util.buildViewClassName(panel, model, null, 'Detail');
        else
            viewClassName = App.Util.buildViewClassName(panel, model, null, 'Editor');

        console.log(viewClassName);
        return Ext.create(viewClassName, cfg);
    }
});