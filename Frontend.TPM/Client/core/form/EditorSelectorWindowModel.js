Ext.define('Core.form.EditorSelectorWindowModel', {
    extend: 'Core.form.BaseEditorModel',

    // Предполагается, что виджет, указанный в selectorWidget, 
    // является наслендиком Ext.grid.Panel.
    selectorWidget: undefined,

    startCreateRecord: function (model) {
        this.editor = this.createEditor();

        this.editor.down('#select').on('click', this.onOkButtonClick, this);
        this.editor.down('#cancel').on('click', this.onCancelButtonClick, this);
        this.editor.on('close', this.onEditorClose, this);
        this.editor.down('grid').on('selectionchange', this.onSelectorGridSelectionChange, this);

        this.editor.show();
    },

    startEditRecord: function (model) {
        this.editor = this.createEditor();

        this.editor.down('#select').on('click', this.onOkButtonClick, this);
        this.editor.down('#close').on('click', this.onCancelButtonClick, this);
        this.editor.on('close', this.onEditorClose, this);
        this.editor.down('grid').on('selectionchange', this.onSelectorGridSelectionChange, this);

        this.editor.show();
    },

    onOkButtonClick: function (button) {
        var ownerGrid = this.getGrid(),
            grid = this.getForm(),
            selModel = grid.getSelectionModel(),
            record = selModel.hasSelection() ? selModel.getSelection()[0] : null;

        if (record && ownerGrid) {
            //TODO: Replace getSaveModelConfig на field mapping
            var modelClass = Ext.ModelManager.getModel(ownerGrid.getStore().model),
                model = Ext.create(modelClass, this.getSaveModelConfig(record, grid));

            this.saveModel(model);
        }
    },

    onCancelButtonClick: function (button) {
        // Window will close automatically.
    },

    onEditorClose: function (window) {

    },

    getForm: function () {
        if (this.editor) {
            return this.editor.down('grid');
        }
    },

    saveModel: function (model, callback) {
        var window = this.editor,
            ownerGrid = this.getGrid();

        window.setLoading(l10n.ns('core').value('savingText'));

        model.save({
            scope: this,
            success: function () {
                window.close();

                ownerGrid.getStore().on({
                    single: true,
                    scope: this,
                    load: function (records, operation, success) {
                        model.set('Key');
                        window.setLoading(false);
                        if (typeof ownerGrid.afterSaveCallback === 'function') {
                            ownerGrid.afterSaveCallback(ownerGrid);
                        }
                    }
                });

                ownerGrid.getStore().load();
                window.close();
            },
            failure: function () {
                //model.reject();
                window.setLoading(false);
            }
        });
    },

    onSelectorGridSelectionChange: function (selModel) {
        var window = selModel.view.up('window'),
            hasSelection = selModel.hasSelection();

        window.down('#select')[hasSelection ? 'enable' : 'disable']();
    },

    // Privates. 

    createEditor: function () {
        var grid = this.getGrid(),
            model = Ext.ModelManager.getModel(grid.getStore().model);

        if (Ext.isString(this.selectorWidget)) {
            this.selectorWidget = {
                xtype: this.selectorWidget
            };
        }

        return Ext.create({
            xtype: 'selectorwindow',
            title: l10n.ns('core').value('selectorWindowTitle'),
            items: [this.selectorWidget]
        });
    }
});