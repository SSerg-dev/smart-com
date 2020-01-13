Ext.define('Core.form.EditorWindowModel', {
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

        this.editor.down('#ok').on('click', this.onOkButtonClick, this);
        this.editor.down('#close').on('click', this.onCancelButtonClick, this);
        this.editor.on('close', this.onEditorClose, this);

        this.getForm().loadRecord(model);
        this.editor.show();

        this.editor.afterWindowShow(this.editor, false);
        this.getForm().getForm().getFields().each(function (field, index, len) {
            if (field.xtype === 'singlelinedisplayfield')
                field.setReadOnly(false);
        }, this);
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

    onCancelButtonClick: function (button) {
        // Window will close automatically.
    },

    onEditorClose: function (window) {
        var form = this.getForm();
        if (form) {
            record = form.getRecord();
            if (record) {
                record.reject();
            }

            form.getForm().reset(true);
        }

        this.editor = null;
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
                this.editor.close();
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
            viewClassName = App.Util.buildViewClassName(panel, model, null, 'Editor');

        console.log(viewClassName);

        return Ext.create(viewClassName, cfg);
    }
});